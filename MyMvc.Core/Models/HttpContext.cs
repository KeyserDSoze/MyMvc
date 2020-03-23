using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public class HttpContext
    {
        public HttpRequest Request { get; }
        public HttpResponse Response { get; }
        public IServiceContext Service { get; }
        internal int MiddlewareIndex { get; set; } = 0;
        internal HttpContext(HttpRequest request, HttpResponse response)
        {
            this.Request = request;
            this.Response = response;
            this.Service = new ServiceContext(MyDIMvc.Instance.ServiceFactory);
        }
    }
}
