using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyMvc.Core
{
    public partial class ServiceCollection : IServiceCollection
    {
        private static readonly Dictionary<string, IServiceWrapper> ServicesByName = new Dictionary<string, IServiceWrapper>();
        public void AddService<TService>(ServiceType serviceType)
           => this.AddService<TService, TService>(serviceType);
        public void AddService<TInterface, TService>(ServiceType serviceType)
        {
            string key = typeof(TInterface).GetKey();
            if (!ServicesByName.ContainsKey(key))
                ServicesByName.Add(key, new ServiceWrapper<TInterface>(serviceType, typeof(TService)));
            else
                throw new ArgumentException($"Too many services with same name for {typeof(TInterface).AssemblyQualifiedName}, service is {typeof(TService).AssemblyQualifiedName}");
        }

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
        public T GetService<T>(IHttpContext httpContext)
        {
            string key = typeof(T).GetKey();
            if (!ServicesByName.ContainsKey(key))
                throw new ArgumentNullException($"{key} doesn't add to service collection.");

            ServiceContext serviceContext = httpContext.Service as ServiceContext;
            IServiceWrapper<T> serviceWrapper = ServicesByName[key] as IServiceWrapper<T>;
            switch (serviceWrapper.ServiceType)
            {
                case ServiceType.Singleton:
                    if (!serviceContext.Singletoned.ContainsKey(key))
                    {
                        lock (TrafficLight)
                        {
                            if (!serviceContext.Singletoned.ContainsKey(key))
                            {
                                serviceContext.Singletoned.Add(key, serviceWrapper.Create(httpContext));
                            }
                        }
                    }
                    return serviceContext.Singletoned[key];
                case ServiceType.Scoped:
                    if (!serviceContext.Scoped.ContainsKey(key))
                        serviceContext.Scoped.Add(key, serviceWrapper.Create(httpContext));
                    return serviceContext.Scoped[key];
                case ServiceType.Transient:
                    return serviceWrapper.Create(httpContext);
                default:
                    throw new NotImplementedException($"Added a service type without implementation --> {serviceWrapper.ServiceType}");
            }
        }

        private static readonly Dictionary<string, MethodInfo> GetServiceMethods = new Dictionary<string, MethodInfo>();
        private static readonly object ServiceTrafficLight = new object();
        public dynamic GetService(Type type, IHttpContext httpContext)
        {
            string key = type.GetKey();
            if (!GetServiceMethods.ContainsKey(key))
            {
                lock (ServiceTrafficLight)
                {
                    if (!GetServiceMethods.ContainsKey(key))
                    {
                        GetServiceMethods.Add(key, this.GetType().GetMethods().FirstOrDefault(x => x.Name == "GetService" && x.IsGenericMethod).MakeGenericMethod(type));
                    }
                }
            }
            return GetServiceMethods[key].Invoke(this, new object[1] { httpContext });
        }
        public dynamic GetService(string name, IHttpContext httpContext)
            => GetService(ServicesByName[name].Type, httpContext);
        public dynamic FindService(string notCompleteName, IHttpContext httpContext)
            => GetService(ServicesByName.FirstOrDefault(x => x.Key.Contains(notCompleteName)).Value.Type, httpContext);
        public bool HasService(Type type)
            => ServicesByName.ContainsKey(type.GetKey());
    }
    internal static class TypeExtensions
    {
        public static string GetKey(this Type type)
            => type.AssemblyQualifiedName.Split(',').First();
    }
}
