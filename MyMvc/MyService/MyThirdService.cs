using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.MyService
{
    public interface IMyThirdService 
    {
        string Value { get; }
    }
    public class MyThirdService : IMyThirdService
    {
        public string Value { get; }
        public MyThirdService() 
        {
            this.Value = Guid.NewGuid().ToString("N");
        }
    }
}
