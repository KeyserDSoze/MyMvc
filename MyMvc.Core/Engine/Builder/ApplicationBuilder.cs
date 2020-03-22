using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public class ApplicationBuilder : IApplicationBuilder
    {
        private static readonly IList<Type> Middlewares = new List<Type>();
        public void AddMiddleware<TMiddleware>() where TMiddleware : IMiddleware
            => Middlewares.Add(typeof(TMiddleware));

        public void Start(HttpContext httpContext)
        {
            
        }
    }
}
