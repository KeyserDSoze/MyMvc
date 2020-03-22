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
        public static IDIMvc Instance => Me ??= new MyDIMvc();
        private MyDIMvc() { }
        private IApplicationBuilder applicationBuilder;
        public IApplicationBuilder ApplicationBuilder => applicationBuilder ??= new ApplicationBuilder(this);
        public IApplicationStarter ApplicationStarter => this.ApplicationBuilder as IApplicationStarter;

        private IServiceCollection serviceCollection;
        public IServiceCollection ServiceCollection => serviceCollection ??= new ServiceCollection(this);
        public IServiceFactory ServiceFactory => this.ServiceCollection as IServiceFactory;

        private IHttpListener httpListener;
        public IHttpListener HttpListener => httpListener ??= new AsynchronousSocketListener(this);
    }
}
