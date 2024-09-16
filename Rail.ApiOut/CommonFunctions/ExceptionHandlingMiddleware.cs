using Rail.ApiOut.IServices;
using Rail.BO.ApiOutModels;
using Rail.BO.Entities;
using System.Text;

namespace Rail.ApiOut.CommonFunctions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ILogService logService)
        {
            string request = string.Empty;
            string response = string.Empty;
            try
            {
                request = await GetRequestBody(httpContext.Request);

                await _next(httpContext);

            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, request, response, logService);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, string request, string response, ILogService logService)
        {
            LogToDB(request, response, context, logService, exception);

            await context.Response.WriteAsJsonAsync(new CommonResponseModel { status = "error", data = null, message = exception.Message });
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

        private void LogToDB(string req, string res, HttpContext context, ILogService logService, Exception? exception = null)
        {
            ErrorLogsModel errorLogsModel = new ErrorLogsModel
            {
                Request = req,
                Response = res,
                Error = exception?.Message ?? null,
                StackTrace = exception?.StackTrace ?? null,
                URL = context.Request.Path,
                AgentDevice = context.Request.Headers["User-Agent"].ToString(),
                StatusCode = context.Response.StatusCode.ToString(),
                Remark = "Rail.ApiOut"
            };

            logService.Log(errorLogsModel);
        }
    }
}
