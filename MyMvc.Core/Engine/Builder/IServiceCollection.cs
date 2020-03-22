using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public interface IServiceCollection
    {
        void AddService<TService>(ServiceType serviceType);
        void AddService<TInterface, TService>(ServiceType serviceType);
        void AddSingleton<TService>();
        void AddSingleton<TInterface, TService>();
        void AddScoped<TService>();
        void AddScoped<TInterface, TService>();
        void AddTransient<TService>();
        void AddTransient<TInterface, TService>();
    }
}
