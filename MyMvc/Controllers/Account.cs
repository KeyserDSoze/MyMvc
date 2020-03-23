using System;
using System.Collections.Generic;
using System.Text;
using MyMvc.Core;
using MyMvc.Interfaces;
using MyMvc.MyService;

namespace MyMvc.Controllers
{
    public class Account : Controller
    {
        private MyFirstService MyFirstService;
        private MyFirstService MyFirstServiceServiceSecondInstance;
        private MySecondService MySecondService;
        private IMyThirdService MyThirdService;
        private IMyThirdService MyThirdServiceSecondInstance;
        public Account(MyFirstService myFirstService, MyFirstService myFirstServiceSecondInstance, MySecondService mySecondService, IMyThirdService myThirdService, IMyThirdService myThirdServiceSecondInstance)
        {
            this.MyFirstService = myFirstService;
            this.MySecondService = mySecondService;
            this.MyThirdService = myThirdService;
            this.MyThirdServiceSecondInstance = myThirdServiceSecondInstance;
            this.MyFirstServiceServiceSecondInstance = myFirstServiceSecondInstance;
        }

        public IActionResult Index()
            => this.View("Account", "Index", $"Scoped: {this.MyFirstService.Value}<br/>Second Scoped: {this.MyFirstServiceServiceSecondInstance.Value}<br/>Singleton: {this.MySecondService.Value}<br/>First Transient: {this.MyThirdService.Value}<br/>Second Transient: {this.MyThirdServiceSecondInstance.Value}");
        public IActionResult Singleton()
            => this.View("Account", "Index", this.MySecondService.Value);
        public IActionResult Scoped()
            => this.View("Account", "Index", this.MyFirstService.Value);
        public IActionResult Transient()
            => this.View("Account", "Index", this.MyThirdService.Value);
    }
}
