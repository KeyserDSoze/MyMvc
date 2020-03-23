using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string controllerName = splitter.Length > 1 && !string.IsNullOrWhiteSpace(splitter[1]) ? splitter[1] : "Home";
            string actionName = splitter.Length > 2 && !string.IsNullOrWhiteSpace(splitter[2]) ? splitter[2] : "Index";
            IController controller = httpContext.Service.Collection.FindService($".Controllers.{controllerName}", httpContext);
            IActionResult result = GetMethod(controller, controllerName, actionName).Invoke(controller, null) as IActionResult; //here the parameters for example instead null
            httpContext.Response.Body = Encoding.ASCII.GetBytes(result.Response);
            httpContext.Response.ContentType = result.ContentType;
            await this.NextInvoke(httpContext);
        }
        private static readonly Dictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();
        private static readonly object TrafficLight = new object();
        private MethodInfo GetMethod(IController controller, string controllerName, string actionName)
        {
            string key = $"{controllerName}/{actionName}";
            if (!Methods.ContainsKey(key))
            {
                lock (TrafficLight)
                {
                    if (!Methods.ContainsKey(key))
                    {
                        Methods.Add(key, controller.GetType().GetMethod(actionName));
                    }
                }
            }
            return Methods[key];
        }
    }
    public static class MvcExtensions
    {
        public static void AddMvc(this IServiceCollection serviceCollection, IStartup startup)
        {
            foreach (Type type in startup.GetType().Assembly.GetTypes().Where(x => typeof(IController).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface))
                serviceCollection.GetType().GetMethods().FirstOrDefault(x => x.Name == "AddScoped" && x.IsGenericMethod).MakeGenericMethod(type).Invoke(serviceCollection, null);
        }
    }
}
