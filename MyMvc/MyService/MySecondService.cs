using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.MyService
{
    public class MySecondService
    {
        public string Value { get; }
        public MySecondService() 
        {
            this.Value = Guid.NewGuid().ToString("N");
        }
    }
}
