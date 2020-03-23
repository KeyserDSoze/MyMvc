using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyMvc.Core
{
    public class StaticFiles : IMiddleware
    {
        public IMiddleware Next { get; set; }
        public async Task Run(HttpContext httpContext)
        {
            if (httpContext.Request.Url.Path == "/favicon.ico")
            {
                httpContext.Response.ContentType = "image/x-icon";
                httpContext.Response.CacheControl = 60 * 60 * 2;
                httpContext.Response.Body = await File.ReadAllBytesAsync($"{Environment.CurrentDirectory}/StaticFiles/favicon.ico");
            }
            else
                await this.NextInvoke(httpContext);
        }
    }
}
