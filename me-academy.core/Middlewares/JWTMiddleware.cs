﻿using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using me_academy.core.Constants;
using me_academy.core.Interfaces;
using me_academy.core.Models.Configurations;
using me_academy.core.Models.View.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace me_academy.core.Middlewares;

public class JWTMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ICacheService _cacheService;
    private readonly ILogger<JWTMiddleware> _logger;

    public JWTMiddleware(RequestDelegate next, ICacheService cacheService)
    {
        _next = next;
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    public async Task Invoke(HttpContext context, IOptions<JwtConfig> jwtConfig)
    {
        // continue if action called is anonymous.
        if (IsAnonymous(context))
        {
            await _next(context);
            return;
        }

        // get the token
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        // continue if token is null
        if (token == null)
        {
            await _next(context);
            return;
        }

        // attach the token to the request
        if (await AttachAccountToContext(context, token, jwtConfig.Value))
        {
            await _next(context);
        }
    }

    private static bool IsAnonymous(HttpContext context)
    {
        // Check if the request is handled by an MVC endpoint
        var endpoint = context.GetEndpoint();
        if (endpoint is RouteEndpoint routeEndpoint)
        {
            // Check if the action method is decorated with AllowAnonymous attribute
            var actionDescriptor = routeEndpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

            bool? methodAllowAnonymousAttribute =
                actionDescriptor?.MethodInfo.GetCustomAttributes(inherit: true)
                .OfType<AllowAnonymousAttribute>().Any();

            bool actionIsAnonymous = methodAllowAnonymousAttribute.HasValue && methodAllowAnonymousAttribute.Value;

            return actionIsAnonymous;
        }

        return false;
    }

    private async Task<bool> AttachAccountToContext(HttpContext context, string token, JwtConfig jwtConfig)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            string id = jwtToken.Claims.First(x => x.Type == "sid").Value;
            string uid = jwtToken.Claims.First(x => x.Type == "uid").Value;

            // get request domain
            string domain = context.Request.Headers["Origin"].ToString();

            //check if token is string in the cache
            string sToken = await _cacheService.GetToken($"{AuthKeys.TokenCacheKey}:{domain}:{uid}");
            if (string.IsNullOrEmpty(sToken) || sToken != token)
            {
                context.Items["User"] = null;
                context.User = null;
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return false;
            };

            // attach account to context on successful jwt validation
            context.Items["User"] = new UserView()
            {
                Uid = uid,
                Id = int.Parse(id)
            };

            return true;
        }
        catch (Exception ex)
        {
            // do nothing if jwt validation fails
            // account is not attached to context so request won't have access to secure routes
        }

        return false;
    }
}