using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Interfaces
{
    public interface IHostBuilder
    {
        IHostBuilder UseStartup<T>() where T : IStartup, new();
        IHostBuilder Build();
        void Run();
    }
}
