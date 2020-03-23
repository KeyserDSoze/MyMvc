using MyMvc.Interfaces;

namespace MyMvc.Core
{
    internal class ServiceWrapper<T> : IServiceWrapper<T>, IServiceWrapper
    {
        public ServiceType ServiceType { get; }
        public ServiceWrapper(ServiceType serviceType)
            => this.ServiceType = serviceType;
        public T Create(IHttpContext httpContext)
            => ConstructorWrapper.GetRightConstructor<T>().Create<T>(httpContext);
    }
}
