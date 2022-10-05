using Microsoft.EntityFrameworkCore;
using ZillPillService.Domain.DTO.MedicalProduct;
using ZillPillService.Domain.DTO.Shedullers;
using ZillPillService.Domain.Exceptions;
using ZillPillService.Domain.Query;
using ZillPillService.Domain.Query.User;
using ZillPillService.Infrastructure.Context;
using ZillPillService.Infrastructure.Entities;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.Application.Services
{
    public class MedicalProductService : IMedicalProductService
    {
        private readonly AppDataBaseContext _context;

        public MedicalProductService(AppDataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<MedicalProductDto>> GetMedicalProductsListAsync(
            GetFilteredMedicalProductQuery filter, int offset, int limit, CancellationToken ct)
        {
            var query = _context.MedicinalProduct
                .Include(x => x.Chemicals)
                .Include(x => x.Certificate)
                .Include(x => x.Image)
                .OrderByDescending(x => x.Id)
                .AsQueryable();

            if (filter.CountryId is Int32 countryId)
                query = query.Where(x => x.CountryDictionaryId == countryId);

            if (filter.ProductName is String productName)
                query = query.Where(x => x.Name.ToLower().Contains(productName.ToLower()));

            if (filter.WithCertificate is bool withCertificate)
            {
                if (withCertificate)
                    query = query.Where(x => x.Certificate.Approved.Equals("зарегистрировано"));
                else
                    query = query.Where(x => !x.Certificate.Approved.Equals("зарегистрировано"));
            }

            if (filter.ChemicalName is String chemicalName)
                query = query.Where(x => x.Chemicals.Any(y => y.Name.ToLower().Contains(chemicalName.ToLower())));

            var entities = await query
                .Skip(offset)
                .Take(limit)
                .Select(x => new MedicalProductDto(x.Id, x.Name, x.Image.Data))
                .ToListAsync(ct);
            return entities;
        }

        public async Task<MedicalProductDetailDto> GetMedicalProductsDetailAsync(
            int productId, CancellationToken ct)
        {
            var entity = await _context.MedicinalProduct
                .Where(x => x.Id == productId)
                .Include(x => x.Image)
                .Include(x => x.Certificate)
                .Include(x => x.Releases)
                .Include(x => x.Chemicals)
                .Select(x => new MedicalProductDetailDto()
                {
                    ProductId = x.Id,
                    ImageData = x.Image.Data,
                    Name = x.Name,
                    Characteristics = x.Characteristics,
                    Releases = x.Releases.Select(x => x.Name).ToList(),
                    Chemicals = x.Chemicals.Select(x => x.Name).ToList(),
                    Certificate = new MedicalProductCertificateDto(x.Certificate.License, x.Certificate.RegisterDate, x.Certificate.Approved)
                })
                .FirstOrDefaultAsync(ct);

            if (entity == null) throw new ApiException("Product not found");

            return entity;
        }

        #region user

        public async Task<List<UserMedicalProductDto>> GetMedProdForUserAsync(
            int userId, CancellationToken ct)
        {
            var entities = await _context.UserMedicinalProduct
                .Where(x => x.UserId == userId)
                .Include(x => x.MedicinalProduct)
                .ThenInclude(y => y.Image)
                .Select(x => new UserMedicalProductDto(x.MedicinalProduct.Id, x.MedicinalProduct.Name, x.MedicinalProduct.Image.Data, x.Id))
                .ToListAsync(ct);

            foreach (var entity in entities)
            {
                var totalShedullers = await _context.MedicationSheduller
                    .Where(x => x.UserMedicinalProductId == entity.RelationId)
                    .Select(x => x.IsAccepted)
                    .ToListAsync(ct);

                entity.TotalToAccept = totalShedullers.Count();
                entity.TotalAccepted = totalShedullers.Count(x => x);
                entity.Progress = entity.TotalToAccept != 0 ? (double)entity.TotalAccepted / entity.TotalToAccept : 0.0;
            }

            return entities;
        }

        public async Task AddMedProdToUserAsync(int userId, int productId, CancellationToken ct)
        {
            var userProd = await _context.UserMedicinalProduct
                .Where(x => x.UserId == userId && x.MedicinalProductId == productId)
                .FirstOrDefaultAsync(ct);

            if (userProd != null)
                throw new ApiException("dublicate product");

            var product = await _context.MedicinalProduct
                .FirstOrDefaultAsync(x => x.Id == productId, ct);

            if (product == null)
                throw new ApiException("med product not found");

            var user = await _context.User
                .FirstOrDefaultAsync(x => x.Id == userId, ct);

            if (user == null)
                throw new ApiException("user not found");

            var newUserProd = new UserMedicinalProduct()
            {
                User = user,
                MedicinalProduct = product
            };

            await _context.UserMedicinalProduct.AddAsync(newUserProd, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task RemoveMedProdFromUserAsync(int relationId, CancellationToken ct)
        {
            var entity = await _context.UserMedicinalProduct
                .Where(x => x.Id == relationId)
                .FirstOrDefaultAsync(ct);

            if (entity == null)
                throw new ApiException("relation not found");

            _context.UserMedicinalProduct.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }

        #endregion user

        #region sheduller

        public async Task<SetShedullerToUserDto> GetMedProdShedullerForUserAsync(
            int relationId, CancellationToken ct)
        {
            var entity = await _context.UserMedicinalProduct
                .Include(x => x.Shedullers)
                .Include(x => x.MedicinalProduct)
                .Where(x => x.Id == relationId)
                .FirstOrDefaultAsync(ct);

            if (entity == null)
                throw new ApiException("relation not found");

            var dayOfWeeks = entity.Shedullers.Any() ? entity.Shedullers.Select(x => x.Date.DayOfWeek).Distinct().ToList() : new();

            List<ShedullerItem> ShedullerItems = new();
            if (entity.Shedullers.Any())
                ShedullerItems = entity.Shedullers
                    .GroupBy(x => x.Date)
                    .First()
                    .Select(x => new ShedullerItem(x.Date, x.Time, x.Quantity))
                    .ToList();

            SetShedullerToUserDto result = new()
            {
                ShedullerType = entity.ShedullerType,
                DateStart = entity.DateStart,
                DateEnd = entity.DateEnd,
                RelationId = entity.Id,
                ProductId = entity.MedicinalProduct.Id,
                DayOfWeeks = dayOfWeeks,
                ShedullerItems = ShedullerItems
            };
            return result;
        }

        public async Task SetMedProdShedullerForUserAsync(
            SetShedullerToUserDto query, CancellationToken ct)
        {
            var entity = await _context.UserMedicinalProduct
                .Include(x => x.Shedullers)
                .Include(x => x.MedicinalProduct)
                .Where(x => x.Id == query.RelationId)
                .FirstOrDefaultAsync(ct);

            if (entity == null)
                throw new ApiException("relation not found");

            entity.ShedullerType = query.ShedullerType;
            entity.DateEnd = query.DateEnd;
            entity.DateStart = query.DateStart;
            entity.Shedullers = query.ShedullerItems
                .Select(x => new MedicationSheduller()
                {
                    Date = x.Date,
                    Time = x.Time,
                    Quantity = x.Quantity,
                    IsAccepted = DateTime.Today > x.Date,
                    IsSended = DateTime.Today > x.Date,
                    UnionUtcDate = x.Date.Add(x.Time).ToUniversalTime(),
                }).ToList();

            _context.UserMedicinalProduct.Update(entity);
            await _context.SaveChangesAsync(ct);
        }

        #endregion sheduller

        #region sheduller items

        public async Task<IEnumerable<ShedullerItemDetailDto>> GetShedullerItemsByDayAsync(int userId, DateTime date, CancellationToken ct)
        {
            var shedullers = await _context.UserMedicinalProduct
                .Where(x => x.UserId == userId)
                .Include(x => x.MedicinalProduct)
                .ThenInclude(x => x.Image)
                .SelectMany(x => x.Shedullers.Where(x => x.Date == date))
                .Select(x => new ShedullerItemDetailDto(x.Id, x.Date, x.Time, x.Quantity, x.IsSended, x.IsAccepted,
                x.UserMedicinalProduct.MedicinalProduct.Name, x.UserMedicinalProduct.MedicinalProduct.Image.Data))
                .ToListAsync(ct);

            return shedullers;
        }

        public async Task AcceptShedullerItemAsync(int shedullerItemId, CancellationToken ct)
        {
            var entity = await _context.MedicationSheduller
                .FirstOrDefaultAsync(x => x.Id == shedullerItemId, ct);

            if (entity == null)
                throw new ApiException("sheduller not found");

            entity.IsAccepted = true;

            _context.MedicationSheduller.Update(entity);
            await _context.SaveChangesAsync(ct);
        }

        #endregion sheduller items
    }
}
