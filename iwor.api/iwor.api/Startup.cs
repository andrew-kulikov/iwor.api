using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using iwor.api.Providers;
using iwor.core.Entities;
using iwor.core.Repositories;
using iwor.core.Services;
using iwor.core.Specifications;
using iwor.DAL;
using iwor.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

namespace iwor.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<ApplicationDbContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("AuctionDB"),
                    options => options.MigrationsAssembly(typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName()
                        .Name)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]));
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = signingKey,
                        ValidateAudience = true,
                        ValidAudience = Configuration["Tokens:Audience"],
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddSwaggerGen(c =>
            {
                //The generated Swagger JSON file will have these properties.
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "iWor API Administrator",
                    Version = "v1"
                });
                var securityDefinition = new OpenApiSecurityScheme
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                };

                c.AddSecurityDefinition("jwt_auth", securityDefinition);

                // Make sure swagger UI requires a Bearer token specified
                var securityScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "jwt_auth",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                var securityRequirements = new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                };
                c.AddSecurityRequirement(securityRequirements);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            //services.AddTransient<IEmailSender, EmailSender>();
            //services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(ISpecification<>), typeof(BaseSpecification<>));

            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<IBookmarkService, BookmarkService>();
            services.AddScoped<IUserService, UserService>();

            services.AddSingleton<IConstantsProvider, ConstantsProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app);
            SeedDatabase(app).Wait();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            app.Use(async (context, next) => { await next(); });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Admin V1"); });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static async Task SeedDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var userManager = (UserManager<ApplicationUser>) scope.ServiceProvider
                .GetService(typeof(UserManager<ApplicationUser>));

            var roleManager = (RoleManager<IdentityRole>) scope.ServiceProvider
                .GetService(typeof(RoleManager<IdentityRole>));

            var roles = new List<string> {"Admin", "Company", "User"};
            roles.ForEach(roleName =>
            {
                var roleExists = roleManager.RoleExistsAsync(roleName).Result;
                if (roleExists) return;

                var role = new IdentityRole {Name = roleName};
                roleManager.CreateAsync(role).Wait();
            });

            var admin = await userManager.FindByNameAsync("admin");
            if (admin == null)
            {
                var user = new ApplicationUser {UserName = "admin", Email = "admin@admin.com"};
                const string password = "123qweA!";

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded) await userManager.AddToRoleAsync(user, "Admin");
            }

            admin = await userManager.FindByNameAsync("admin");

            SeedAuctions(app, admin);
        }

        private static void SeedAuctions(IApplicationBuilder app, ApplicationUser user)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            var auctions = new List<Auction>
            {
                new Auction
                {
                    Status = AuctionStatus.Open,
                    Name = "Mazda e433",
                    Created = DateTime.Now,
                    Description = "Super azaza kekek",
                    EndDate = DateTime.Now.AddDays(10),
                    OwnerId = user.Id,
                    StartPrice = 1000,
                    LogoUrl = "https://cdn.pixabay.com/photo/2018/05/28/22/11/message-in-a-bottle-3437294_1280.jpg"
                },
                new Auction
                {
                    Status = AuctionStatus.Open,
                    Name = "Butylka 228",
                    Created = DateTime.Now,
                    Description = "Erwin 123 business",
                    EndDate = DateTime.Now.AddDays(12),
                    OwnerId = user.Id,
                    StartPrice = 1200,
                    LogoUrl = "https://cdn.pixabay.com/photo/2014/12/12/22/08/glass-565914_960_720.jpg"
                }
            };

            foreach (var auction in auctions)
                if (context.Auctions.ToList().All(a => a.Name != auction.Name))
                {
                    var newAuction = context.Auctions.Add(auction);

                    var sp = newAuction.Entity.StartPrice;
                    var raise = new PriceRaise
                    {
                        AuctionId = newAuction.Entity.Id,
                        StartPrice = sp,
                        RaisedUserId = user.Id,
                        EndPrice = sp + 200
                    };
                    var raise2 = new PriceRaise
                    {
                        AuctionId = newAuction.Entity.Id,
                        StartPrice = sp + 200,
                        RaisedUserId = user.Id,
                        EndPrice = sp + 322
                    };
                    var raise3 = new PriceRaise
                    {
                        AuctionId = newAuction.Entity.Id,
                        StartPrice = sp + 322,
                        RaisedUserId = user.Id,
                        EndPrice = sp + 550
                    };

                    context.SaveChanges();

                    context.PriceRaises.AddRange(new List<PriceRaise> {raise, raise3, raise2});

                    context.SaveChanges();
                }
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.Migrate();
        }
    }
}