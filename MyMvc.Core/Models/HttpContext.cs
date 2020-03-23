using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public class HttpContext : IHttpContext
    {
        public IHttpRequest Request { get; }
        public IHttpResponse Response { get; }
        public IServiceContext Service { get; }
        internal int MiddlewareIndex { get; set; } = 0;
        internal HttpContext(IHttpRequest request, IHttpResponse response)
        {
            this.Request = request;
            this.Response = response;
            this.Service = new ServiceContext(MyDIMvc.Instance.ServiceFactory);
        }
    }
}
