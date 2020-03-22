using MyMvc.Core;
using System;

namespace MyMvc
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder<HostBuilder>().Build().Run();
        }
        //injected from the superior framework
        public static IHostBuilder CreateHostBuilder<T>()
            where T : IHostBuilder, new()
            => new T().UseStartup<Startup>();
    }
}
