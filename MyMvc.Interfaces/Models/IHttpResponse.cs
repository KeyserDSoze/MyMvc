using System;
using System.Collections.Generic;
using System.Text;

namespace MyMvc.Interfaces
{
    public interface IHttpResponse
    {
        string ContentType { get; set; }
#pragma warning disable CA1819
        byte[] Body { get; set; }
        string AcceptRanges { get; set; }
        int? CacheControl { get; set; }
        int StatusCode { get; set; }
        byte[] Fetch();
    }
}
