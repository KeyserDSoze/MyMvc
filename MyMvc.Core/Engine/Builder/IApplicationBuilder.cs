using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public interface IApplicationBuilder
    {
        void AddMiddleware<TMiddleware>() where TMiddleware : IMiddleware;
        void Start(HttpContext httpContext);
    }
}
