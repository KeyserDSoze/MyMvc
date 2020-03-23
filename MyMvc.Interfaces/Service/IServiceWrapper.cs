using System;

namespace MyMvc.Interfaces
{
    public interface IServiceWrapper
    {
        Type Type { get; }
        ServiceType ServiceType { get; }
    }
    public interface IServiceWrapper<T> : IServiceWrapper
    {
        T Create(IHttpContext httpContext);
    }
}
