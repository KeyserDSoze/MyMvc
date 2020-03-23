using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Interfaces
{
    public interface IController
    {
        IHttpContext HttpContext { get; }
    }
}
