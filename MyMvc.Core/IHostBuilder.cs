using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public interface IHostBuilder
    {
        IHostBuilder UseStartup<T>() where T : IStartup, new();
        IHostBuilder Build();
        void Run();
    }
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
