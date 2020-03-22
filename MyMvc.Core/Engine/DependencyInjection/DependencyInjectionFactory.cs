using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public class DependencyInjectionFactory
    {
        private static Dictionary<Type, object> Singletoned = new Dictionary<Type, object>(); //Singleton
        private Dictionary<Type, object> Scoped = new Dictionary<Type, object>(); //FlyWeight
        private static readonly object TrafficLight = new object(); //trafficlight for lock during singleton
        public object Create(ServiceType serviceType, Type type)
        {
            switch (serviceType)
            {
                case ServiceType.Singleton:
                    if (!Singletoned.ContainsKey(type))
                    {
                        lock (TrafficLight)
                        {
                            if (!Singletoned.ContainsKey(type))
                            {
                                Singletoned.Add(type, CreateWidthDependency(type));
                            }
                        }
                    }
                    return Singletoned[type];
                case ServiceType.Scoped:
                    if (!Scoped.ContainsKey(type))
                        Scoped.Add(type, CreateWidthDependency(type));
                    return Scoped[type];
                case ServiceType.Transient:
                    return CreateWidthDependency(type);
                default:
                    throw new NotImplementedException($"Added a service type without implementation --> {serviceType}");
            }
        }
        private object CreateWidthDependency(Type type)
        {
            throw new NotImplementedException("To finalize");
        }
    }
}
