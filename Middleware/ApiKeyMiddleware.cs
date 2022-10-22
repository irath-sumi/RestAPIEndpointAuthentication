using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace SalesLiftPOC.Middleware
{
    /// <summary>
    /// The middleware class must include:
    ///  A public constructor with a parameter of type RequestDelegate. A public method named Invoke or InvokeAsync.This method must:
    ///    Return a Task.
    ///    Accept a first parameter of type HttpContext.
    /// </summary>
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private
        const string APIKEY = "XApiKey";
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            
            if (!context.Request.Headers.TryGetValue(APIKEY, out
                    var extractedApiKey))
            {
                using MemoryStream stream = new MemoryStream();
                ReadOnlyMemory<byte> readOnlyMemory = new ReadOnlyMemory<byte>(stream.ToArray());

                // set the status code
                context.Response.StatusCode = 401;
                // Set the content type
                context.Response.ContentType = "application/text; charset=utf-8";

                await context.Response.WriteAsync("Api Key was not provided!!");
                return;
            }
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(APIKEY);
            if (!apiKey.Equals(extractedApiKey))
            {
                // set the status code
                context.Response.StatusCode = 401;
                
                // Write to reponse body
                await context.Response.WriteAsync("Unauthorized client");
                return;
            }
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
