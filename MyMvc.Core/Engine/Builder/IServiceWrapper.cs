using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    internal interface IServiceWrapper
    {
        ServiceType ServiceType { get; }
    }
    internal interface IServiceWrapper<T> : IServiceWrapper
    {
        T Create(HttpContext httpContext);
    }
}
