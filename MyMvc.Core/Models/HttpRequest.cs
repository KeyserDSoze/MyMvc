using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MyMvc.Core
{

    public class HttpRequest : IHttpRequest
    {
        public string Method { get; }
        public string HttpProtocol { get; }
        public HttpUrl Url { get; }
        public IPAddress IpAddress { get; }
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();
        public HttpRequest(byte[] request)
        {
            string[] requestAsString = Encoding.ASCII.GetString(request).Split('\n');
            string[] firstLine = requestAsString[0].Split(' ');
            this.Method = firstLine[0]; //Get, Post, Update, etc.....
            this.Url = new HttpUrl(firstLine[1]); //path and querystring
            this.HttpProtocol = firstLine[2]; //http 1.0, 1.1, 1.2 etc.
            for (int i = 1; i < requestAsString.Length; i++)
            {
                string[] header = requestAsString[i].Split(':');
                if (header.Length > 1 && !this.Headers.ContainsKey(header[0]))
                    this.Headers.Add(header[0], header[1].Trim());
                else
                    break;
                //stop the next because it's the body of message
            }
            //after three \n i can find the body of http message
        }
    }
}
