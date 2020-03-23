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
            string[] splitter = httpContext.Request.Url.Path.Split('/');
            string controller = splitter[0];
            if (!string.IsNullOrWhiteSpace(controller))
                controller = "Home";
            string action = splitter.Length > 1 ? splitter[1] : "Index";
            //reflection here
            httpContext.Response.Body = Encoding.ASCII.GetBytes($"<div>Hello World {DateTime.UtcNow}!!!</div>");
            await this.NextInvoke(httpContext);
        }
    }
}
