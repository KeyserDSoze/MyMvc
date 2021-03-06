﻿using System;
using System.Collections.Generic;
using System.Text;
using MyMvc.Core;
using MyMvc.Interfaces;
using MyMvc.MyService;

namespace MyMvc.Controllers
{
    public class Home : Controller
    {
        private readonly MyFirstService MyFirstService;
        private readonly MyFirstService MyFirstServiceServiceSecondInstance;
        private readonly MySecondService MySecondService;
        private readonly IMyThirdService MyThirdService;
        private readonly IMyThirdService MyThirdServiceSecondInstance;
        public Home(MyFirstService myFirstService, MyFirstService myFirstServiceSecondInstance, MySecondService mySecondService, IMyThirdService myThirdService, IMyThirdService myThirdServiceSecondInstance)
        {
            this.MyFirstService = myFirstService;
            this.MySecondService = mySecondService;
            this.MyThirdService = myThirdService;
            this.MyThirdServiceSecondInstance = myThirdServiceSecondInstance;
            this.MyFirstServiceServiceSecondInstance = myFirstServiceSecondInstance;
        }

        public IActionResult Index()
            => this.View("Home", "Index", $"Scoped: {this.MyFirstService.Value}<br/>Second Scoped: {this.MyFirstServiceServiceSecondInstance.Value}<br/>Singleton: {this.MySecondService.Value}<br/>First Transient: {this.MyThirdService.Value}<br/>Second Transient: {this.MyThirdServiceSecondInstance.Value}");
        public IActionResult Singleton()
            => this.View("Home", "Index", this.MySecondService.Value);
        public IActionResult Scoped()
            => this.View("Home", "Index", this.MyFirstService.Value);
        public IActionResult Transient()
            => this.View("Home", "Index", this.MyThirdService.Value);
    }
}
