using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public class ServiceContext : IServiceContext
    {
        private static Dictionary<Type, dynamic> singletoned = new Dictionary<Type, dynamic>(); //Singleton
        internal Dictionary<Type, dynamic> Singletoned => singletoned;
        internal Dictionary<Type, dynamic> Scoped { get; } = new Dictionary<Type, dynamic>(); //FlyWeight
        public IServiceFactory Collection { get; }
        public ServiceContext(IServiceFactory serviceFactory)
            => this.Collection = serviceFactory;
    }
}
