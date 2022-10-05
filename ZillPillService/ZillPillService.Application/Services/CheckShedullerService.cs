using Microsoft.EntityFrameworkCore;
using ZillPillService.Domain.Query.Notification;
using ZillPillService.Infrastructure.Context;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.Application.Services
{
    public class CheckShedullerService : ICheckShedullerService
    {
        private readonly AppDataBaseContext _context;
        private readonly INotificationService _notifService;

        public CheckShedullerService(AppDataBaseContext context, INotificationService notifService)
        {
            _context = context;
            _notifService = notifService;
        }

        public async Task CheckSheduller()
        {
            var nowUTC = DateTime.Now.ToUniversalTime();

            var entities = await _context.MedicationSheduller
                .Include(x => x.UserMedicinalProduct)
                .ThenInclude(x => x.User)
                .Include(x => x.UserMedicinalProduct)
                .ThenInclude(x => x.MedicinalProduct)
                .Where(x => !x.IsSended && x.UnionUtcDate <= nowUTC)
                .ToListAsync();

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    var topick = $"sheduller_{entity.UserMedicinalProduct.User.Phone}";
                    NotificationQuery mesage = new()
                    {
                        Type = Domain.Model.NotificationTypeEnum.Sheduller,
                        Describle = $"Пора принимать {entity.UserMedicinalProduct.MedicinalProduct.Name}, {entity.Quantity} шт.",
                        Title = "Напоминание"
                    };
                    await _notifService.SendAll(topick, mesage);

                    entity.IsSended = true;
                }

                _context.MedicationSheduller.UpdateRange(entities);
                await _context.SaveChangesAsync();
            }
        }
    }
}
