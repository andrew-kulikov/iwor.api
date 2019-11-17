using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using iwor.core.Entities;
using iwor.DAL;
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
            services.AddDbContext<ApplicationDbContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("AuctionDB"),
                    options => options.MigrationsAssembly(typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName()
                        .Name)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddControllers();

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
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
                    Title = "Swagger XML Api Demo",
                    Version = "v1",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app);
            SeedDatabase(app).Wait();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

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