using System.Threading.Tasks;

namespace MyMvc.Interfaces
{
    public interface IMiddleware
    {
        //It's not a real chain of responsibility but it's fundamental is that.
        Task Run(IHttpContext httpContext);
    }
}
