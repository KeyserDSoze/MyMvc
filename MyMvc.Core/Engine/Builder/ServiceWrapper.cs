using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyMvc.Core
{
    internal interface IServiceWrapper
    {
        ServiceType ServiceType { get; }
    }
    internal interface IServiceWrapper<T> : IServiceWrapper
    {
        T Create();
    }
    internal class ServiceWrapper<T> : IServiceWrapper<T>, IServiceWrapper
    {
        public ServiceType ServiceType { get; }
        public ServiceWrapper(ServiceType serviceType)
            => this.ServiceType = serviceType;
        public T Create()
            => ConstructorWrapper.GetRightConstructor<T>().Create<T>();
    }
    internal class ConstructorWrapper
    {
        private static readonly Dictionary<Type, ConstructorWrapper> Constructors = new Dictionary<Type, ConstructorWrapper>();
        private static readonly object TrafficLight = new object();
        private ConstructorInfo ConstructorInfo { get; }
        private IList<Type> Types { get; }
        private ConstructorWrapper(ConstructorInfo constructorInfo, IList<Type> types)
        {
            this.ConstructorInfo = constructorInfo;
            this.Types = types;
        }
        public static ConstructorWrapper GetRightConstructor<T>()
        {
            Type typeToCreate = typeof(T);
            if (!Constructors.ContainsKey(typeToCreate))
            {
                lock (TrafficLight)
                {
                    if (!Constructors.ContainsKey(typeToCreate))
                    {
                        foreach (ConstructorInfo constructorInfo in typeToCreate.GetConstructors().OrderByDescending(x => x.GetParameters().Length))
                        {
                            IList<Type> parameters = constructorInfo.GetParameters().Select(x => x.ParameterType).ToList();
                            if (parameters.All(x => MyDIMvc.Instance.ServiceFactory.HasService(x)))
                            {
                                Constructors.Add(typeToCreate, new ConstructorWrapper(constructorInfo, parameters));
                                break;
                            }
                        }
                        if (!Constructors.ContainsKey(typeToCreate))
                            throw new ArgumentException($"Constructor without correct parameters for {typeToCreate}");
                    }
                }
            }
            return Constructors[typeToCreate];
        }
        public T Create<T>()
            => (T)this.ConstructorInfo.Invoke(this.Types.Select(x => MyDIMvc.Instance.ServiceFactory.GetService(x)).ToArray());
    }
}
