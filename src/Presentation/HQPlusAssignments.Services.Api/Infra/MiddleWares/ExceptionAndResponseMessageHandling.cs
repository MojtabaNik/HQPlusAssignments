using HQPlusAssignments.Application.Core.Exceptions;
using HQPlusAssignments.Resources.SystemErrors;
using HQPlusAssignments.Resources.SystemMessages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HQPlusAssignments.Services.Api.Infra.MiddleWares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    /// <summary>
    /// Exception Handling
    /// </summary>
    public class ExceptionAndResponseMessageHandling
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="env"></param>
        public ExceptionAndResponseMessageHandling(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        /// <summary>
        /// Invoke method which runs on middle ware , when a request come out from application
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            var responseObject = new object();

            bool hasError = true;
            try
            {
                await _next(httpContext);
                hasError = false;

                var message = "";

                //Success message of web api
                switch (httpContext.Request.Method.ToLower())
                {
                    case "post":
                        message = SystemMessagesResourceKeys.PostSuccess;
                        httpContext.Response.StatusCode = StatusCodes.Status201Created;
                        break;
                    case "get":
                        message = SystemMessagesResourceKeys.GetSuccess;
                        break;
                    case "put":
                        message = SystemMessagesResourceKeys.PutSuccess;
                        break;
                    case "delete":
                        message = SystemMessagesResourceKeys.DeleteSuccess;
                        break;
                    default:
                        message = "Unknown Result";
                        httpContext.Response.StatusCode = StatusCodes.Status200OK;
                        break;
                }


                if (httpContext.Response.Body.Length == 0)
                {
                    //httpContext.Response.Clear();
                    httpContext.Response.ContentType = "application/json";
                    string json = JsonConvert.SerializeObject(new { Message = message });
                    await httpContext.Response.WriteAsync(json).ConfigureAwait(false);
                }


            }
            catch (UserFriendlyException ex)
            {
                httpContext.Response.Clear();
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                responseObject = new { ex.Message };
            }
            catch (Exception ex)
            {
                httpContext.Response.Clear();
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                //ToDo:Log Exception

                if (!_env.IsDevelopment())
                {
                    responseObject = new { Message = SystemErrorResourceKeys.SystemUnhandledException };
                }
                else
                {
                    responseObject = new { Message = SystemErrorResourceKeys.SystemUnhandledException, Exception = ex };
                }
            }

            if (hasError)
            {
                string json = JsonConvert.SerializeObject(responseObject);
                await httpContext.Response.WriteAsync(json).ConfigureAwait(false);
            }
        }
    }

    internal class ValidationMessage
    {
        public string PropertyName { get; set; }
        public IEnumerable<string> Messages { get; set; }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    /// <summary>
    /// Helper Class
    /// </summary>
    public static class ExceptionHandlingExtensions
    {
        /// <summary>
        /// This method is used for handling all kind of system exceptions and create response for them
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionAndResponseMessageHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionAndResponseMessageHandling>();
        }
    }
}
