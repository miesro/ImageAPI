namespace Indg.ImageAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation(
                $"Incoming Request: {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");

            if (context.Request.ContentLength > 0 &&
                context.Request.ContentType?.Contains("application/json") == true)
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                _logger.LogInformation("Request Body: {Body}", body);
            }

            var originalBody = context.Response.Body;
            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            await _next(context);

            context.Response.Body = originalBody;
            newBody.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(newBody).ReadToEndAsync();
            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);

            _logger.LogInformation("Response Status: {StatusCode}", context.Response.StatusCode);
        }
    }
}
