using System;
using System.Net.Http.Headers;

namespace Brimma.LOSService.DTO
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        //public int ErrorCode { get; set; }
        public ErrorResponse ErrorResponse { get; set; }
        public HttpResponseHeaders HttpResponseHeaders { get; set; }
    }
}
