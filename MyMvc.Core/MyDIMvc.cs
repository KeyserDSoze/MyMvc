using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public interface IDIMvc
    {
        IApplicationBuilder ApplicationBuilder { get; }
        IApplicationStarter ApplicationStarter { get; }
        IServiceCollection ServiceCollection { get; }
        IServiceFactory ServiceFactory { get; }
        IHttpListener HttpListener { get; }
    }
    public class MyDIMvc : IDIMvc
    {
        private static IDIMvc Me;
        internal static IDIMvc Instance => Me ??= new MyDIMvc();
        private MyDIMvc() { }

        private IApplicationBuilder applicationBuilder;
        public IApplicationBuilder ApplicationBuilder => applicationBuilder ??= new ApplicationBuilder(this);
        public IApplicationStarter ApplicationStarter => this.ApplicationBuilder as IApplicationStarter;

        public IServiceCollection ServiceCollection { get; } = new ServiceCollection();
        public IServiceFactory ServiceFactory => this.ServiceCollection as IServiceFactory;

        private IHttpListener httpListener;
        public IHttpListener HttpListener => httpListener ??= new AsynchronousSocketListener(this);
    }
}
