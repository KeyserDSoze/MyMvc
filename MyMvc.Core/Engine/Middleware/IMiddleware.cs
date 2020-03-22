using System.Threading.Tasks;

namespace MyMvc.Core
{
    public interface IMiddleware
    {
        IMiddleware Next { get; set; }
        Task Run(HttpContext httpContext);
    }
    public static class MiddlewareExtensions
    {
        public static async Task Invoke(this IMiddleware middleware, HttpContext httpContext)
        {
            if (middleware.Next != null)
                await middleware.Next.Run(httpContext);
        }
    }
}
