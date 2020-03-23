using System.Collections.Generic;
using System.Net;

namespace MyMvc.Interfaces
{
    public interface IHttpRequest
    {
        string Method { get; }
        string HttpProtocol { get; }
        HttpUrl Url { get; }
        IPAddress IpAddress { get; }
        Dictionary<string, string> Headers { get; }
    }
}
