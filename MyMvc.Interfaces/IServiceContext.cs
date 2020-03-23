using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Interfaces
{
    public interface IServiceContext
    {
        IServiceFactory Collection { get; }
    }
}
