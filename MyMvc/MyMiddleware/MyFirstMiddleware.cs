using MyMvc.Core;
using MyMvc.Interfaces;
using MyMvc.MyService;
using System;
using System.Threading.Tasks;

namespace MyMvc.MyMiddleware
{
    public class MyFirstMiddleware : IMiddleware
    {
        private readonly MyFirstService MyFirstService;
        public MyFirstMiddleware(MyFirstService myFirstService)
        {
            this.MyFirstService = myFirstService;
        }
        public async Task Run(IHttpContext httpContext)
        {
            Console.WriteLine(MyFirstService.Value);
            await this.NextInvoke(httpContext);
        }
    }
}
