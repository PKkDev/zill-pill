namespace ZillPillService.Infrastructure.ServicesContract
{
    public interface ICheckShedullerService
    {
        public Task CheckShedullersAsync(CancellationToken ct);

        public Task SendSystemMessgeAsync(string body, CancellationToken ct);
    }
}
