using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMvc.Core
{
    //https://www.w3.org/Protocols/HTTP/1.0/spec.html#Response
    public class HttpResponse : IHttpResponse
    {
        public string ContentType { get; set; } = "text/plain";
        public byte[] Body { get; set; }
        public string AcceptRanges { get; set; } = "bytes";
        public int? CacheControl { get; set; }
        public int StatusCode { get; set; } = 200;
        public byte[] Fetch()
            => this.Body == null ? GetHeader() : GetHeader().Concat(this.Body).ToArray();
        private byte[] GetHeader()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (CacheControl != null)
                stringBuilder.Append($"Cache-Control: public, max-age={CacheControl}{SeparatorHttp}");
            return Encoding.ASCII.GetBytes(string.Format(TTPHeader, this.StatusCode, this.AcceptRanges, this.ContentType, this.Body?.Length, stringBuilder.ToString()));
        }
        //separator between headers
        private const string SeparatorHttp = "\r\n";
        //response as Http protocol
        private const string TTPHeader = "HTTP/1.1 {0}\r\n" +
        "Server: jrap\r\n" +
        "accept-ranges: {1}\r\n" +
        "Content-Type: {2}\r\n" +
        "{4}" +
        "Content-Length: {3}\r\n\r\n"; //\r\n\r\n" this is the separator from body and head
    }
}
