using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMvc.Core
{
    public class ApplicationBuilder : IApplicationBuilder, IApplicationStarter
    {
        private IDIMvc DIMvc;
        public ApplicationBuilder(IDIMvc dIMvc)
            => this.DIMvc = dIMvc;

        private static readonly IList<IMiddlewareWrapper> Middlewares = new List<IMiddlewareWrapper>();
        public void UseMiddleware<TMiddleware>() where TMiddleware : IMiddleware
            => Middlewares.Add(new MiddlewareWrapper<TMiddleware>(this.DIMvc.ServiceFactory));

        public async Task Next(HttpContext httpContext)
        {
            if (httpContext.MiddlewareIndex < Middlewares.Count)
                await Middlewares[httpContext.MiddlewareIndex].Middleware(httpContext).Invoke(httpContext);
        }
    }
    internal interface IMiddlewareWrapper
    {
        IMiddleware Middleware(HttpContext httpContext);
    }
    internal class MiddlewareWrapper<T> : IMiddlewareWrapper
        where T : IMiddleware
    {
        private IServiceFactory ServiceFactory;
        public IMiddleware Middleware(HttpContext httpContext) 
            => this.ServiceFactory.GetService<T>(httpContext);
        public MiddlewareWrapper(IServiceFactory serviceFactory)
            => this.ServiceFactory = serviceFactory;
    }
}
