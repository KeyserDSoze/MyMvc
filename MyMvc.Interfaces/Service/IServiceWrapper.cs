namespace MyMvc.Interfaces
{
    public interface IServiceWrapper
    {
        ServiceType ServiceType { get; }
    }
    public interface IServiceWrapper<T> : IServiceWrapper
    {
        T Create(IHttpContext httpContext);
    }
}
