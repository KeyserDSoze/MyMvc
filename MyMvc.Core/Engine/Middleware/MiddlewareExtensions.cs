using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMvc.Core
{
    public static class MiddlewareExtensions
    {
        public static async Task NextInvoke(this IMiddleware middleware, IHttpContext httpContext)
        {
            (httpContext as HttpContext).MiddlewareIndex++;
            await MyDIMvc.Instance.ApplicationStarter.Next(httpContext);
        }
    }
}
