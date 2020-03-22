using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMvc.Core
{
    public partial class ServiceCollection : IServiceCollection
    {
        private static readonly Dictionary<Type, IServiceWrapper> Services = new Dictionary<Type, IServiceWrapper>();
        private IDIMvc DIMvc;
        public ServiceCollection(IDIMvc dIMvc)
            => this.DIMvc = dIMvc;
        public void AddService<TService>(ServiceType serviceType)
           => this.AddService<TService, TService>(serviceType);
        public void AddService<TInterface, TService>(ServiceType serviceType)
            => Services.Add(typeof(TInterface), new ServiceWrapper<TService>(serviceType));

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
    }
    public partial class ServiceCollection : IServiceFactory
    {
        private static Dictionary<Type, dynamic> Singletoned = new Dictionary<Type, dynamic>(); //Singleton
        private Dictionary<Type, dynamic> Scoped = new Dictionary<Type, dynamic>(); //FlyWeight
        private static readonly object TrafficLight = new object(); //trafficlight for lock during singleton
        public T GetService<T>()
        {
            Type typeToCreate = typeof(T);
            if (!Services.ContainsKey(typeToCreate))
                throw new ArgumentNullException($"{typeToCreate} doesn't add to service collection.");

            IServiceWrapper<T> serviceWrapper = Services[typeToCreate] as IServiceWrapper<T>;
            switch (serviceWrapper.ServiceType)
            {
                case ServiceType.Singleton:
                    if (!Singletoned.ContainsKey(typeToCreate))
                    {
                        lock (TrafficLight)
                        {
                            if (!Singletoned.ContainsKey(typeToCreate))
                            {
                                Singletoned.Add(typeToCreate, serviceWrapper.Create());
                            }
                        }
                    }
                    return Singletoned[typeToCreate];
                case ServiceType.Scoped:
                    if (!Scoped.ContainsKey(typeToCreate))
                        Scoped.Add(typeToCreate, serviceWrapper.Create());
                    return Scoped[typeToCreate];
                case ServiceType.Transient:
                    return serviceWrapper.Create();
                default:
                    throw new NotImplementedException($"Added a service type without implementation --> {serviceWrapper.ServiceType}");
            }
        }
        public dynamic GetService(Type type) 
            => this.GetType().GetMethod("GetService").MakeGenericMethod(type).Invoke(this, null);
        public bool HasService(Type type) 
            => Services.ContainsKey(type);
    }
}
