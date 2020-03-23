using System;

namespace MyMvc.Interfaces
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
    public interface IServiceFactory
    {
        dynamic GetService(Type type, IHttpContext httpContext);
        T GetService<T>(IHttpContext httpContext);
        dynamic GetService(string name, IHttpContext httpContext);
        dynamic FindService(string notCompleteName, IHttpContext httpContext);
        bool HasService(Type type);
    }
}
