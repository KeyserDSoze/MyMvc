using MyMvc.Core;
using MyMvc.Interfaces;
using MyMvc.MyMiddleware;
using MyMvc.MyService;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc
{
    public class Startup : IStartup
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<MyFirstService>();
            serviceCollection.AddScoped<MyFirstMiddleware>();
        }
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseStaticFiles();
            applicationBuilder.UseMiddleware<MyFirstMiddleware>();
            applicationBuilder.UseMvc();
        }
    }
}
