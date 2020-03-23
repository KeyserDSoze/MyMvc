using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyMvc.Core
{
    public class HostBuilder : IHostBuilder
    {
        private IStartup Startup;
        public IHostBuilder UseStartup<T>() where T : IStartup, new()
        {
            this.Startup = new T();
            return this;
        }
        public IHostBuilder Build()
        {
            //preconfigured services, similar to ILog or etc.
            MyDIMvc.Instance.ServiceCollection.AddSingleton<StaticFiles>();
            MyDIMvc.Instance.ServiceCollection.AddScoped<Mvc>();
            //Add Controller as Services here as Scoped service
            this.Startup.ConfigureServices(MyDIMvc.Instance.ServiceCollection);
            this.Startup.Configure(MyDIMvc.Instance.ApplicationBuilder);
            return this;
        }
        public void Run()
        {
            MyDIMvc.Instance.HttpListener.StartListening();
        }
    }
}
