using ZillPillService.Infrastructure.Entities;

namespace ZillPillService.Infrastructure.ServicesContract
{
    public interface IJwtGenerator
    {
        public string CreateToken(User user);
    }
}
