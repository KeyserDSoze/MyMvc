using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMvc.Core
{
    public interface IApplicationBuilder
    {
        void UseMiddleware<TMiddleware>() where TMiddleware : IMiddleware;
    }
    public interface IApplicationStarter
    {
        Task Next(HttpContext httpContext);
    }
    public static class ApplicationBuilderExtensions 
    {
        public static void UseStaticFiles(this IApplicationBuilder applicationBuilder) 
            => applicationBuilder.UseMiddleware<StaticFiles>();
        public static void UseMvc(this IApplicationBuilder applicationBuilder) 
            => applicationBuilder.UseMiddleware<Mvc>();
    }
}
