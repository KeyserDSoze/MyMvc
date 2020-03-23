using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMvc.Interfaces
{
    public interface IApplicationBuilder
    {
        void UseMiddleware<TMiddleware>() where TMiddleware : IMiddleware;
    }
    public interface IApplicationStarter
    {
        Task Next(IHttpContext httpContext);
    }
    
}
