using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Repositories;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace DutchTreat
{
    public class Startup
    {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {       // ajout des services
            services.AddTransient<IMailService, NullMailService>();
            services.AddDbContext<DBContextDutch>();
            services.AddTransient<DutchSeeder>();
            services.AddScoped<IDutchRepository, DutchRepository>();
            // configurer les vues,mvc, serialisation json boucle recursive 
            services.AddControllersWithViews()  
              .AddRazorRuntimeCompilation()
              .AddNewtonsoftJson(conf => conf.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            // configurer l'auto mappage
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddRazorPages();
    }
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // Add Error Page
        app.UseExceptionHandler("/error");
      }

      app.UseStaticFiles();

      app.UseRouting();

      app.UseEndpoints(cfg =>
      {
        cfg.MapControllerRoute("Fallback",
          "{controller}/{action}/{id?}",
          new { controller = "App", action = "Index" });

        cfg.MapRazorPages();
      });
    }
  }
}
