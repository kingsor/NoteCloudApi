using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using NoteCloud.Helpers;
using System;
using Microsoft.Extensions.Options;

namespace NoteCloud.Middleware {
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;
        private Secrets _secrets;
 
        public AuthorizeMiddleware(IOptions<Secrets> secrets, RequestDelegate next)
        {
            _secrets = secrets.Value;
            _next = next;
        }
 
        public async Task Invoke(HttpContext context)
        {
            bool isLogin = context.Request.Path.ToString().Equals("/users/login");
            bool isLoginSlash = context.Request.Path.ToString().Equals("/users/login/");
            bool isCreateUser = context.Request.Path.ToString().Equals("/users");
            bool isCreateUserSlash = context.Request.Path.ToString().Equals("/users/");
            bool isRoot = context.Request.Path.ToString().Equals("/");
            if(!(isRoot || isLogin || isLoginSlash || isCreateUser || isCreateUserSlash)) {
                if (!context.Request.Headers.Keys.Contains("Authorize"))
                {
                    context.Response.StatusCode = 400; //Bad Request                
                    await context.Response.WriteAsync("Authorize attribute is missing " + context.Request.Path.ToString());
                    return;
                }
                else
                {
                    try {
                        if(!JWT.Verify(_secrets, context.Request.Headers["Authorize"]))
                        {
                            context.Response.StatusCode = 401; //UnAuthorized
                            await context.Response.WriteAsync("Invalid auth");
                            return;
                        }
                    } catch (Exception ex) {
                        context.Response.StatusCode = 401; //UnAuthorized
                        System.Console.WriteLine(ex.ToString());
                        await context.Response.WriteAsync("Invalid auth");
                        return;
                    }
                }
            }

            await _next.Invoke(context);
        }
    }
}