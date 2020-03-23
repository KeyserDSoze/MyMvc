using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private static readonly object TrafficLight = new object(); //trafficlight for lock during singleton
        public T GetService<T>(HttpContext httpContext)
        {
            Type typeToCreate = typeof(T);
            if (!Services.ContainsKey(typeToCreate))
                throw new ArgumentNullException($"{typeToCreate} doesn't add to service collection.");

            ServiceContext serviceContext = httpContext.Service as ServiceContext;
            IServiceWrapper<T> serviceWrapper = Services[typeToCreate] as IServiceWrapper<T>;
            switch (serviceWrapper.ServiceType)
            {
                case ServiceType.Singleton:
                    if (!serviceContext.Singletoned.ContainsKey(typeToCreate))
                    {
                        lock (TrafficLight)
                        {
                            if (!serviceContext.Singletoned.ContainsKey(typeToCreate))
                            {
                                serviceContext.Singletoned.Add(typeToCreate, serviceWrapper.Create(httpContext));
                            }
                        }
                    }
                    return serviceContext.Singletoned[typeToCreate];
                case ServiceType.Scoped:
                    if (!serviceContext.Scoped.ContainsKey(typeToCreate))
                        serviceContext.Scoped.Add(typeToCreate, serviceWrapper.Create(httpContext));
                    return serviceContext.Scoped[typeToCreate];
                case ServiceType.Transient:
                    return serviceWrapper.Create(httpContext);
                default:
                    throw new NotImplementedException($"Added a service type without implementation --> {serviceWrapper.ServiceType}");
            }
        }

        private static readonly Dictionary<Type, MethodInfo> GetServiceMethods = new Dictionary<Type, MethodInfo>();
        private static readonly object ServiceTrafficLight = new object();
        public dynamic GetService(Type type, HttpContext httpContext)
        {
            if (!GetServiceMethods.ContainsKey(type))
            {
                lock (ServiceTrafficLight)
                {
                    if (!GetServiceMethods.ContainsKey(type))
                    {
                        GetServiceMethods.Add(type, this.GetType().GetMethods().FirstOrDefault(x => x.Name == "GetService" && x.IsGenericMethod).MakeGenericMethod(type));
                    }
                }
            }
            return GetServiceMethods[type].Invoke(this, new object[1] { httpContext });
        }
        public bool HasService(Type type)
            => Services.ContainsKey(type);
    }
}
