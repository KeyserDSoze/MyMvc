using MyMvc.Core;
using MyMvc.MyService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMvc.MyMiddleware
{
    public class MyFirstMiddleware : IMiddleware
    {
        private MyFirstService MyFirstService;
        public MyFirstMiddleware(MyFirstService myFirstService)
        {
            this.MyFirstService = myFirstService;
        }
        public async Task Run(HttpContext httpContext)
        {
            Console.WriteLine(MyFirstService.Value);
            await this.Invoke(httpContext);
        }
    }
}
