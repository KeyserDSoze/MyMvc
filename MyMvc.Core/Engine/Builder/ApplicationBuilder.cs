using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMvc.Core
{
    public class ApplicationBuilder : IApplicationBuilder, IApplicationStarter
    {
        private readonly IDIMvc DIMvc;
        public ApplicationBuilder(IDIMvc dIMvc)
            => this.DIMvc = dIMvc;

        private static readonly IList<IMiddlewareWrapper> Middlewares = new List<IMiddlewareWrapper>();
        public void UseMiddleware<TMiddleware>() where TMiddleware : IMiddleware
            => Middlewares.Add(new MiddlewareWrapper<TMiddleware>(this.DIMvc.ServiceFactory));

        public async Task Next(IHttpContext httpContext)
        {
            HttpContext myHttpContext = httpContext as HttpContext;
            if (myHttpContext.MiddlewareIndex < Middlewares.Count)
                await Middlewares[myHttpContext.MiddlewareIndex].Middleware(httpContext).Run(httpContext);
        }
    }
}
