using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public class HtmlAction : IActionResult
    {
        public string Response { get; }
        public string ContentType { get; } = "text/html";
        public HtmlAction(string response)
            => this.Response = response;
    }
}
