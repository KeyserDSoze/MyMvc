using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public class ServiceCollection : IServiceCollection
    {
        private static readonly Dictionary<Type, LifeTimeWrapper> Services = new Dictionary<Type, LifeTimeWrapper>();

        public void AddService<TService>(ServiceType serviceType)
           => this.AddService<TService, TService>(serviceType);
        public void AddService<TInterface, TService>(ServiceType serviceType)
            => Services.Add(typeof(TInterface), new LifeTimeWrapper(typeof(TService), serviceType));

        public void AddSingleton<TService>()
            => this.AddSingleton<TService, TService>();
        public void AddSingleton<TInterface, TService>()
            => this.AddService<TInterface, TService>(ServiceType.Singleton);

        public void AddScoped<TService>()
            => this.AddScoped<TService, TService>();
        public void AddScoped<TInterface, TService>()
            => this.AddService<TInterface, TService>(ServiceType.Scoped);

        public void AddTransient<TService>()
            => this.AddTransient<TService, TService>();
        public void AddTransient<TInterface, TService>()
            => this.AddService<TInterface, TService>(ServiceType.Transient);

        private class LifeTimeWrapper
        {
            public Type Type { get; }
            public ServiceType ServiceType { get; }
            public LifeTimeWrapper(Type type, ServiceType serviceType)
            {
                this.Type = type;
                this.ServiceType = serviceType;
            }
        }
    }
}
