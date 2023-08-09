using Azure.Core;
using task.data;
using task.data.Access;
using task.data.Essentials;

namespace task.Helper
{

    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, CustomerSession session)
        {
            var request = context.Request;
            string url = request.Path.Value.ToLower();
            bool ResponseEnded = false;
            if ((!url.StartsWith("/api/login/access")))
            {
                Microsoft.Extensions.Primitives.StringValues AuthHeader = "";
                context.Request.Headers.TryGetValue("Authorization", out AuthHeader);
                string UserHeader = AuthHeader.ToString();
                bool IsValid = true;
                if (!string.IsNullOrEmpty(UserHeader))
                {
                    try
                    {
                        UserHeader = UserHeader.Substring(7);
                        session = DataToken.IsJWTTokenValid(UserHeader);
                        if (session.IsValid)
                        {
                           session.CustomerId = session.Claims.Find(m => m.Name == "id")?.Value.ToString();
                        }
                        else
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync("{ \"error\" : \"Token Invalid\",\"Message\" : \"Token Missed\",\"type\" : \"error\"}");
                            ResponseEnded = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("{ \"error\" : \"Token Invalid\",\"Message\" : \"Token Invalid\",\"type\" : \"error\", \"header\":\"" + UserHeader + "\"}");
                        ResponseEnded = true;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{ \"error\" : \"Token Invalid\",\"Message\" : \"Token Invalid\",\"type\" : \"error\", \"header\":\"" + UserHeader + "\"}");
                    ResponseEnded = true;
                }
            }
            if (!ResponseEnded)
            {
                await _next(context);
            }
        }
    }
}
