using DutchTreat.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace DutchTreat
{
    public class Program
  {
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
            var isSeed = args.Length == 1 && args[0].ToLower() == "seed";
            if (isSeed)
            {
            RunSeed(host);
            }
            else
            {
            host.Run();
            }
    }
		// manaual
		public static void RunSeed(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
            var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
            seeder.SeedAsync().Wait();
            }
        }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(AddConfiguration)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
        // manaual
        private static void AddConfiguration(HostBuilderContext ctx, IConfigurationBuilder bldr)
        {
            bldr.Sources.Clear();
            bldr.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
        }
  }
}
