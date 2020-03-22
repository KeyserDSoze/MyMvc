using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMvc.Core
{
    public interface IApplicationBuilder
    {
        void AddMiddleware<TMiddleware>() where TMiddleware : IMiddleware;
    }
    public interface IApplicationStarter
    {
        Task Next(HttpContext httpContext);
    }
}
