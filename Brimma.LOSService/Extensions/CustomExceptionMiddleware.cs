using Brimma.LOSService.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Brimma.LOSService.Extensions
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SaveNLog saveNLogger;
        public CustomExceptionMiddleware(RequestDelegate next, SaveNLog saveNLogger)
        {
            _next = next;
            this.saveNLogger = saveNLogger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            saveNLogger.SaveLogFile("CustomExceptionMiddleware", "HandleExceptionAsync", exception.StackTrace, exception.Message);
            int statusCode = (int)HttpStatusCode.InternalServerError;
            ErrorDetails errorDetails = new ErrorDetails();
            var result = JsonConvert.SerializeObject(errorDetails.CreateErrorResponse(statusCode, exception.Message));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
