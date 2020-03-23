using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public interface IServiceContext
    {
        IServiceFactory Collection { get; }
    }
}
