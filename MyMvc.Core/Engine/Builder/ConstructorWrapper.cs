using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyMvc.Core
{
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
        public static ConstructorWrapper GetRightConstructor(Type typeToCreate)
        {
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
        public T Create<T>(IHttpContext httpContext)
        {
            try
            {
                return (T)this.ConstructorInfo.Invoke(this.Types.Select(x => MyDIMvc.Instance.ServiceFactory.GetService(x, httpContext)).ToArray());
            }
            catch (Exception ex)
            {
                string olaf = ex.ToString();
                return default;
            }
        }
    }
}
