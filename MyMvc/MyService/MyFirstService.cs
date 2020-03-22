using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.MyService
{
    public class MyFirstService
    {
        public string Value { get; }
        public MyFirstService() 
        {
            this.Value = Guid.NewGuid().ToString("N");
        }
    }
}
