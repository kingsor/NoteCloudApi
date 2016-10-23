using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using NoteCloud.Helpers;
using System;

namespace NoteCloud.Middleware {
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;
 
        public AuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }
 
        public async Task Invoke(HttpContext context)
        {
            bool isNotLogin = !context.Request.Path.ToString().Equals("/users/login");
            bool isNotCreateUser = !context.Request.Path.ToString().Equals("/users");
            bool isNotPost = !context.Request.Method.Equals("POST");
            if(isNotPost && (isNotLogin || isNotCreateUser)) {
                if (!context.Request.Headers.Keys.Contains("Authorize"))
                {
                    context.Response.StatusCode = 400; //Bad Request                
                    await context.Response.WriteAsync("Authorize attribute is missing");
                    return;
                }
                else
                {
                    try {
                        if(!JWT.Verify(context.Request.Headers["Authorize"]))
                        {
                            context.Response.StatusCode = 401; //UnAuthorized
                            await context.Response.WriteAsync("Invalid auth");
                            return;
                        }
                    } catch (Exception ex) {
                        context.Response.StatusCode = 401; //UnAuthorized
                        await context.Response.WriteAsync("Invalid auth");
                        return;
                    }
                }
            }

            await _next.Invoke(context);
        }
 
    }
}