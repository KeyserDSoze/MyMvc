using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Core
{
    public class HttpUrl
    {
        public string Host { get; }
        public string Path { get; }
        public string Protocol { get; }
        public Dictionary<string, string> Querystring { get; }
        public HttpUrl(string requestUrl)
        {
            string[] queryAndPath = requestUrl.Split('?');
            this.Path = queryAndPath[0];
            this.Querystring = new Dictionary<string, string>();
            if (queryAndPath.Length > 1)
            {
                foreach (string query in queryAndPath[1].Split('&'))
                {
                    string[] keyValue = query.Split('=');
                    if (!Querystring.ContainsKey(keyValue[0]))
                        Querystring.Add(keyValue[0], keyValue[1]);
                }
            }
        }
    }
}
