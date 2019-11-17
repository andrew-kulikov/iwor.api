using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace iwor.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls("https://localhost:55221", "http://localhost:55220");
                });
        }
    }
}