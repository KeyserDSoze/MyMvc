﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public interface IStartup 
    {
        void ConfigureServices(IServiceCollection serviceCollection);
        void Configure(IApplicationBuilder applicationBuilder);
    }
}
