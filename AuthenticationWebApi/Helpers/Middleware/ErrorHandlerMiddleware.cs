using AuthenticationWebApi.Helpers.ApplicationException;
using AuthenticationWebApi.Helpers.Jwt;
using AuthenticationWebApi.Models.Account;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Text.Json;

namespace AuthenticationWebApi.Helpers.Middleware
{
    public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        public async Task Invoke(HttpContext context, UserManager<Account> userManager, IJwtUtils jwtUtils)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var accountId = jwtUtils.ValidateJwtToken(token);

                if (accountId != null)
                {
                    // attach account to context on successful jwt validation
                    var account = await userManager.FindByIdAsync(accountId);

                    context.Items["Account"] = account;
                }

                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case BadRequestException:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case NotFoundException:
                    // custom application error
                    case KeyNotFoundException:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ForbiddenException:
                        // access denied error
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    case AppException:
                    // custom application error
                    default:
                        // unhandled error
                        logger.LogError(error, error.Message);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
