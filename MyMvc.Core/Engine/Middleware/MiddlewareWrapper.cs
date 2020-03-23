using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    internal class MiddlewareWrapper<T> : IMiddlewareWrapper
       where T : IMiddleware
    {
        private IServiceFactory ServiceFactory;
        public IMiddleware Middleware(IHttpContext httpContext)
            => this.ServiceFactory.GetService<T>(httpContext);
        public MiddlewareWrapper(IServiceFactory serviceFactory)
            => this.ServiceFactory = serviceFactory;
    }
}
