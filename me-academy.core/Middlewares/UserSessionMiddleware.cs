using System.Security.Claims;
using me_academy.core.Models.Input.Auth;
using Microsoft.AspNetCore.Http;

namespace me_academy.core.Middlewares;

public class UserSessionMiddleware
{
    private readonly RequestDelegate _next;

    public UserSessionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserSession session)
    {
        if (context.User.Identities.Any(x => x.IsAuthenticated))
        {
            int.TryParse(context.User.Claims.SingleOrDefault(c => c.Type == "sid")?.Value, out int UserId);

            session.UserId = UserId;
            session.Uid = context.User.Claims.SingleOrDefault(c => c.Type == "uid")?.Value;
            session.Name = context.User.Claims.SingleOrDefault(c => c.Type == "name")?.Value;
            session.Roles = context.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(x => x.Value).ToList();
        }

        // Call the next delegate/middleware in the pipeline
        await _next.Invoke(context);
    }
}