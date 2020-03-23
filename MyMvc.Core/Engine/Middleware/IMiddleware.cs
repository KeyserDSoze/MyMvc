using System.Threading.Tasks;

namespace MyMvc.Core
{
    public interface IMiddleware
    {
        //It's not a real chain of responsibility but it's fundamental is that.
        Task Run(HttpContext httpContext);
    }
    public static class MiddlewareExtensions
    {
        public static async Task NextInvoke(this IMiddleware middleware, HttpContext httpContext)
        {
            httpContext.MiddlewareIndex++;
            await MyDIMvc.Instance.ApplicationStarter.Next(httpContext);
        }
    }
}
