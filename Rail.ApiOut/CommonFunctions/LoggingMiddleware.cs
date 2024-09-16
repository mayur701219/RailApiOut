using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Rail.ApiOut.IServices;
using System.IO;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Rail.ApiOut.CommonFunctions
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;


        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RailDBContext dBContext)
        {
            string request = string.Empty;
            string response = string.Empty;
            string correlationId = httpContext.Request.Headers["correlationId"];
            try
            {
                request = await GetRequestBody(httpContext.Request);

                using (var buffer = new MemoryStream())
                {
                    var stream = httpContext.Response.Body;
                    httpContext.Response.Body = buffer;

                    await _next.Invoke(httpContext);

                    buffer.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(buffer);
                    using (var bufferReader = new StreamReader(buffer))
                    {
                        response = await bufferReader.ReadToEndAsync();
                        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                        await LogRequestToDatabaseAsync(dBContext, request, response, correlationId, httpContext);

                        await httpContext.Response.Body.CopyToAsync(stream);
                        httpContext.Response.Body = stream;

                    }
                }
            }
            catch (Exception ex) { }
        }


        private async Task<string?> GetRequestBody(HttpRequest request)
        {
            string requestContent = string.Empty;
            if (request.Method == HttpMethods.Post && request.ContentLength > 0)
            {
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);

                requestContent = Encoding.UTF8.GetString(buffer);

                request.Body.Position = 0;
            }
            requestContent += request.QueryString.Value ?? request.QueryString.Value;
            return requestContent;
        }


        private async Task LogRequestToDatabaseAsync(RailDBContext dbContext, string request, string response, string correlationId, HttpContext httpContext)
        {
            try
            {
                string url = $"{httpContext.Request.Path}{httpContext.Request.QueryString}".ToLower();
                string stage = string.Empty;

                if (url.Contains("auth"))
                {
                    stage = "LOGIN";
                }                
                else if (url.Contains("search"))
                {
                    stage = "SEARCH";
                }
                else if (url.Contains("createdbooking"))
                {
                    stage = "BOOKING";
                }
                else if (url.Contains("addpax"))
                {
                    stage = "UPDATE TRAVELER";
                }
                else if (url.Contains("prebooking"))
                {
                    stage = "PREBOOKING";
                }
                else if (url.Contains("processbooking"))
                {
                    stage = "CONFIRM BOOKING";
                }


                var logEntry = new ApiOutLogs
                {
                    Request = request,
                    Url = url,
                    CreatedDate = DateTime.UtcNow,
                    correlationId = correlationId,
                    Stage = stage,
                    Response = httpContext.Response.StatusCode + " | " + response
                };
                dbContext.apiOutLogs.Add(logEntry);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //private async Task LogResponseToDatabaseAsync(RailDBContext dbContext, Guid logId, string response)
        //{
        //    // Find the log entry by LogId
        //    var logEntry = await dbContext.apiOutLogs.FirstOrDefaultAsync(x => x.LogId == logId);


        //    if (logEntry != null)
        //    {
        //        // Update the response
        //        logEntry.Response = response;

        //        // Save changes to the database
        //        await dbContext.SaveChangesAsync();
        //    }
        //}
    }

}
