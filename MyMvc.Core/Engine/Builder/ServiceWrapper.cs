using MyMvc.Interfaces;
using System;

namespace MyMvc.Core
{
    internal class ServiceWrapper<T> : IServiceWrapper<T>, IServiceWrapper
    {
        public ServiceType ServiceType { get; }
        public Type Type { get; }
        public ServiceWrapper(ServiceType serviceType, Type type)
        {
            this.ServiceType = serviceType;
            this.Type = type;
        }
        public T Create(IHttpContext httpContext)
            => ConstructorWrapper.GetRightConstructor(this.Type).Create<T>(httpContext);
    }
}
