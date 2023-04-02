using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Reciper.Authentication;

public class JwtAuthFilter : IAsyncAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public JwtAuthFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedObjectResult("No Token");
            return;
        }
        var token = authHeader.Substring("Bearer ".Length).Trim();
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedObjectResult("Invalid Token");
            return;
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);
            if (principal == null)
            {
                context.Result = new UnauthorizedObjectResult("Invalid Token");
                return;
            }

            // Token is valid, add the user principal to the context
            context.HttpContext.User = principal;
        }
        catch (Exception ex)
        {
            context.Result = new UnauthorizedObjectResult("Invalid Token: " + ex.Message);
            return;
        }
    }
    
}