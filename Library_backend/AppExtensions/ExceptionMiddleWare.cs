using Library_backend.ErrorHandling;
using Microsoft.AspNetCore.Diagnostics;

namespace Library_backend.AppExtensions
{
    public static class ExceptionMiddleWare
    {
        public static void CustomExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            // Global Exception Handler
            app.UseExceptionHandler(error =>
            {
                // Exception Handling Logic
                error.Run(async context =>
                {
                    // context.Features.Get<IExceptionHandlerFeature>() to get exception details
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        logger.LogError($"Error occurred: {contextFeature.Error}");
                        var errorDetails = new ErrorDetails
                        {
                            StatusCode = 500,
                            Message = "Something went wrong."
                        };

                        await context.Response.WriteAsJsonAsync(errorDetails);
                    }
                });
            });
        }
    }
}
