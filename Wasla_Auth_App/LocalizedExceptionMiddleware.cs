using Microsoft.Extensions.Localization;

namespace Wasla_Auth_App
{
    public class LocalizedExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer<Messages> _localizer;

        public LocalizedExceptionMiddleware(RequestDelegate next, IStringLocalizer<Messages> localizer)
        {
            _next = next;
            _localizer = localizer;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                context.Response.StatusCode = 400;
                var message = _localizer[ex.MessageKey]; // Use key from exception
                await context.Response.WriteAsJsonAsync(new { Message = message });
            }
        }
    }

    public class CustomException : Exception
    {
        public string MessageKey { get; }

        public CustomException(string messageKey)
        {
            MessageKey = messageKey;
        }
    }

}
