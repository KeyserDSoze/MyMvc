using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyMvc.Core
{
    public abstract class Controller : IController
    {
        public IHttpContext HttpContext { get; internal set; }
        private static readonly Dictionary<string, string[]> Pages = new Dictionary<string, string[]>();
        private static readonly object TrafficLight = new object();
        protected IActionResult View(string controller, string action, object model)
        {
            string[] lines = this.GetPage(controller, action);
            StringBuilder response = new StringBuilder();
            foreach (string line in lines)
            {
                //todo: here we can implement an interpreter
                if (line.StartsWith("@model"))
                    continue;
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.Contains("@Model"))
                    response.AppendLine(line.Replace("@Model", model.ToString()));
                else
                    response.AppendLine(line);
            }
            return new HtmlAction(response.ToString());
        }
        private string[] GetPage(string controller, string action)
        {
            string key = $"{controller}/{action}";
            if (!Pages.ContainsKey(key))
            {
                lock (TrafficLight)
                {
                    if (!Pages.ContainsKey(key))
                    {
                        Pages.Add(key, File.ReadAllLines(@$"{Environment.CurrentDirectory}\Views\{key}.txt"));
                    }
                }
            }
            return Pages[key];
        }
    }
}
