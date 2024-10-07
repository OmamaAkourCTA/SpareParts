
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Text.Json;


namespace SparePartsModule.API
{
    public class CustomExceptionMiddleware
    {

        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //Continue processing
                if (_next != null)
                    await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                //Log the exception if you want 
                if (!context.Response.HasStarted)
                {
                    ApiResponseModel? response;

                    if (ex is ManagerProcessException managerProcessException)
                    {
                        response = ApiResponseFactory.CreateManageProcessErrorResponse(managerProcessException.StatusCde,
                            info: ex.Message, stackTrace: managerProcessException.IsInternal ? null : ex.StackTrace);
                    }
                    else
                    {
                        response = ApiResponseFactory.CreateErrorResponse("000002", ex.Message+"-"+ex.InnerException, stackTrace: ex.StackTrace);
                    }
                    var json = JsonSerializer.Serialize(response);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 400;

                    await context.Response.WriteAsync(json);
                }
            }
        }
    }
}

