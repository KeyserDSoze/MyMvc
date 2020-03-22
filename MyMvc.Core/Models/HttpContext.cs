using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public class HttpContext
    {
        public HttpRequest Request { get; }
        public HttpResponse Response { get; }
    }
}
