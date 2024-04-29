using AuthenticationWebApi.Helpers.ApplicationException;
using AuthenticationWebApi.Helpers.Jwt;
using System.Net;
using System.Text.Json;

namespace AuthenticationWebApi.Helpers.Middleware
{
    public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var authentication = jwtUtils.ValidateJwtToken(token);

                context.Items["AccountId"] = authentication.Item1;
                context.Items["AccountRoles"] = authentication.Item2;

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
