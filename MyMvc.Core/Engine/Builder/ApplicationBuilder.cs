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
                await Middlewares[httpContext.MiddlewareIndex].Middleware.Invoke(httpContext);
        }
    }
    internal interface IMiddlewareWrapper
    {
        IMiddleware Middleware { get; }
    }
    internal class MiddlewareWrapper<T> : IMiddlewareWrapper
        where T : IMiddleware
    {
        private IServiceFactory ServiceFactory;
        public IMiddleware Middleware => this.ServiceFactory.GetService<T>();
        public MiddlewareWrapper(IServiceFactory serviceFactory)
            => this.ServiceFactory = serviceFactory;
    }
}
