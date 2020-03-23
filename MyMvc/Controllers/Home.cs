using System;
using System.Collections.Generic;
using System.Text;
using MyMvc.Core;
using MyMvc.Interfaces;
using MyMvc.MyService;

namespace MyMvc.Controllers
{
    public class Home : Controller
    {
        private MyFirstService MyFirstService;
        public Home(MyFirstService myFirstService)
            => this.MyFirstService = myFirstService;
        public IActionResult Index()
        {
            return this.View("Home", "Ïndex", this.MyFirstService.Value);
        }
    }
}
