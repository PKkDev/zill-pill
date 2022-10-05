using Microsoft.EntityFrameworkCore;
using ZillPillService.Infrastructure.Entities;

namespace ZillPillService.Infrastructure.Context
{
    public class AppDataBaseContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }

        public DbSet<UserMedicinalProduct> UserMedicinalProduct { get; set; }

        public DbSet<CountryDictionary> CountryDictionary { get; set; }

        public DbSet<MedicationSheduller> MedicationSheduller { get; set; }

        public DbSet<MedicinalProduct> MedicinalProduct { get; set; }
        public DbSet<MedicinalProductImage> MedicinalProductImage { get; set; }
        public DbSet<MedicinalProductCertificate> MedicinalProductCertificate { get; set; }
        public DbSet<MedicinalProductRelease> MedicinalProductRelease { get; set; }
        public DbSet<MedicinalProductChemical> MedicinalProductChemical { get; set; }

        public AppDataBaseContext(DbContextOptions<AppDataBaseContext> options)
            : base(options)
        {
            // Add-Migration MigrationV8
            // Update-Database
            // Remove-Migration

            // удаляем бд со старой схемой
            // Database.EnsureDeleted();

            // создаем бд с новой схемой
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // использование Fluent API
            //base.OnModelCreating(modelBuilder);
        }

        public static void SeedInitilData(AppDataBaseContext context)
        {
            if (!context.User.Any() && !context.MedicinalProduct.Any())
            {
                var user = new User()
                {
                    Phone = "89372174165",
                    Code = null,
                    Profile = new UserProfile()
                    {
                        Email = "portnov-kiril997@mail.ru",
                        FirstName = "Kirill",
                    }
                };
                context.User.Add(user);
                context.SaveChanges();

                var pathToFile = Path.Combine(AppContext.BaseDirectory, "files", "initial.jpg");
                var bytes = File.ReadAllBytes(pathToFile);
                var mp1 = new MedicinalProduct()
                {
                    Name = "Отороферон",
                    Manufacturer = "кто-то",
                    Characteristics = "хартеристики",
                    Chemicals = new List<MedicinalProductChemical>()
                    {
                        new MedicinalProductChemical() { Name = "имия 1" },
                        new MedicinalProductChemical() { Name = "имия 2" }
                    },
                    Certificate = new MedicinalProductCertificate()
                    {
                        Approved = "да",
                        License = "лицензия",
                        RegisterDate = DateTime.Today
                    },
                    Releases = new List<MedicinalProductRelease>()
                    {
                        new MedicinalProductRelease() { Name = "выпуск 1" },
                        new MedicinalProductRelease() { Name = "выпуск 2" },
                    },
                    Image = new MedicinalProductImage()
                    {
                        Data = bytes
                    }
                };
                var mp2 = new MedicinalProduct()
                {
                    Name = "Ангерон",
                    Manufacturer = "кто-то4",
                    Characteristics = "хартеристики3",
                    Chemicals = new List<MedicinalProductChemical>()
                    {
                        new MedicinalProductChemical() { Name = "имия 3" },
                        new MedicinalProductChemical() { Name = "имия 4" }
                    },
                    Certificate = new MedicinalProductCertificate()
                    {
                        Approved = "да",
                        License = "лицензия",
                        RegisterDate = DateTime.Today
                    },
                    Releases = new List<MedicinalProductRelease>()
                    {
                        new MedicinalProductRelease() { Name = "выпуск 3" },
                        new MedicinalProductRelease() { Name = "выпуск 4" },
                    },
                    Image = new MedicinalProductImage()
                    {
                        Data = bytes
                    }
                };

                context.MedicinalProduct.Add(mp1);
                context.MedicinalProduct.Add(mp2);
                context.SaveChanges();

                UserMedicinalProduct ump = new()
                {
                    User = user,
                    MedicinalProduct = mp1,
                    Shedullers = new List<MedicationSheduller>()
                    {
                        new MedicationSheduller()
                        {
                           // DayOfWeek = DateTime.Today.DayOfWeek,
                            Time = new TimeSpan(15,0,0)
                        }
                    }
                };
                context.UserMedicinalProduct.Add(ump);
                context.SaveChanges();

                user.MedicinalProducts.Add(mp2);
                context.User.Update(user);
                context.SaveChanges();
            }
        }
    }
}
