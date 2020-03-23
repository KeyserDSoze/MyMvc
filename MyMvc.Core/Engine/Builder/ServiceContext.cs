using MyMvc.Interfaces;
using System;
using System.Collections.Generic;

namespace MyMvc.Core
{
    public class ServiceContext : IServiceContext
    {
        private static readonly Dictionary<string, dynamic> singletoned = new Dictionary<string, dynamic>(); //Singleton
        internal Dictionary<string, dynamic> Singletoned => singletoned;
        internal Dictionary<string, dynamic> Scoped { get; } = new Dictionary<string, dynamic>(); //FlyWeight
        public IServiceFactory Collection { get; }
        public ServiceContext(IServiceFactory serviceFactory)
            => this.Collection = serviceFactory;
    }
}
