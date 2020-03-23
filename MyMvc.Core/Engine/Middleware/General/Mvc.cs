using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
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
            string controllerName = splitter.Length > 1 ? splitter[1] : "Home";
            string actionName = splitter.Length > 2 ? splitter[2] : "Index";
            object controller = httpContext.Service.Collection.FindService($".Controllers.{controllerName}", httpContext);
            IActionResult result = controller.GetType().GetMethod(actionName).Invoke(controller, null) as IActionResult; //here the parameters for example instead null
            httpContext.Response.Body = Encoding.ASCII.GetBytes(result.Response);
            httpContext.Response.ContentType = result.ContentType;
            await this.NextInvoke(httpContext);
        }
    }
}
