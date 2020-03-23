using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMvc.Core
{
    public class Mvc : IMiddleware
    {
        public IMiddleware Next { get; set; }
        public async Task Run(IHttpContext httpContext)
        {
            httpContext.Response.Body = Encoding.ASCII.GetBytes($"<div>Hello World {DateTime.UtcNow}!!!</div>");
            await this.NextInvoke(httpContext);
        }
    }
}
