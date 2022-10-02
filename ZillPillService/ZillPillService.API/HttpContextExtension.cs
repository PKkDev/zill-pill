using ZillPillService.Domain.Exceptions;

namespace ZillPillService.API
{
    public static class HttpContextExtension
    {
        public static int GetUserId(this HttpContext httpContext)
        {
            var userIdStr = httpContext.User.Claims.ToList().Find(x => x.Type == "UserIdentity");
            if (userIdStr == null)
                throw new ApiException("Wrong User Claims");
            var userId = Convert.ToInt32(userIdStr.Value);
            return userId;
        }
    }
}
