﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZillPillService.Infrastructure.Context;

#nullable disable

namespace ZillPillService.Infrastructure.Migrations
{
    [DbContext(typeof(AppDataBaseContext))]
    [Migration("20220926074236_MigrationV2")]
    partial class MigrationV2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc.1.22426.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MedicinalProductUser", b =>
                {
                    b.Property<int>("MedicinalProductsId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("MedicinalProductsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("MedicinalProductUser");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicationSheduller", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<int>("UserMedicinalProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserMedicinalProductId");

                    b.ToTable("MedicationSheduller");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicinalProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Characteristics")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MedicinalProduct");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicinalProductCertificate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Approved")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("License")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MedicinalProductId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MedicinalProductId")
                        .IsUnique();

                    b.ToTable("MedicinalProductCertificate");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicinalProductChemical", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MedicinalProductId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MedicinalProductId");

                    b.ToTable("MedicinalProductChemical");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicinalProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("MedicinalProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicinalProductId")
                        .IsUnique();

                    b.ToTable("MedicinalProductImage");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicinalProductRelease", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MedicinalProductId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MedicinalProductId");

                    b.ToTable("MedicinalProductRelease");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Phone" }, "Phone_Index")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.UserMedicinalProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MedicinalProductId")
                        .HasColumnType("int");

                    b.Property<int?>("ShedullerType")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicinalProductId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMedicinalProduct");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex(new[] { "Email" }, "Email_Index")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.ToTable("UserProfile");
                });

            modelBuilder.Entity("MedicinalProductUser", b =>
                {
                    b.HasOne("ZillPillService.Infrastructure.Entities.MedicinalProduct", null)
                        .WithMany()
                        .HasForeignKey("MedicinalProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ZillPillService.Infrastructure.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicationSheduller", b =>
                {
                    b.HasOne("ZillPillService.Infrastructure.Entities.UserMedicinalProduct", "UserMedicinalProduct")
                        .WithMany("Shedullers")
                        .HasForeignKey("UserMedicinalProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserMedicinalProduct");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicinalProductCertificate", b =>
                {
                    b.HasOne("ZillPillService.Infrastructure.Entities.MedicinalProduct", "MedicinalProduct")
                        .WithOne("Certificate")
                        .HasForeignKey("ZillPillService.Infrastructure.Entities.MedicinalProductCertificate", "MedicinalProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicinalProduct");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicinalProductChemical", b =>
                {
                    b.HasOne("ZillPillService.Infrastructure.Entities.MedicinalProduct", "MedicinalProduct")
                        .WithMany("Chemicals")
                        .HasForeignKey("MedicinalProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicinalProduct");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicinalProductImage", b =>
                {
                    b.HasOne("ZillPillService.Infrastructure.Entities.MedicinalProduct", "MedicinalProduct")
                        .WithOne("Image")
                        .HasForeignKey("ZillPillService.Infrastructure.Entities.MedicinalProductImage", "MedicinalProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicinalProduct");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicinalProductRelease", b =>
                {
                    b.HasOne("ZillPillService.Infrastructure.Entities.MedicinalProduct", "MedicinalProduct")
                        .WithMany("Releases")
                        .HasForeignKey("MedicinalProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicinalProduct");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.UserMedicinalProduct", b =>
                {
                    b.HasOne("ZillPillService.Infrastructure.Entities.MedicinalProduct", "MedicinalProduct")
                        .WithMany("UserMedicinalProduct")
                        .HasForeignKey("MedicinalProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ZillPillService.Infrastructure.Entities.User", "User")
                        .WithMany("UserMedicinalProduct")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicinalProduct");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.UserProfile", b =>
                {
                    b.HasOne("ZillPillService.Infrastructure.Entities.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("ZillPillService.Infrastructure.Entities.UserProfile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.MedicinalProduct", b =>
                {
                    b.Navigation("Certificate")
                        .IsRequired();

                    b.Navigation("Chemicals");

                    b.Navigation("Image")
                        .IsRequired();

                    b.Navigation("Releases");

                    b.Navigation("UserMedicinalProduct");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.User", b =>
                {
                    b.Navigation("Profile")
                        .IsRequired();

                    b.Navigation("UserMedicinalProduct");
                });

            modelBuilder.Entity("ZillPillService.Infrastructure.Entities.UserMedicinalProduct", b =>
                {
                    b.Navigation("Shedullers");
                });
#pragma warning restore 612, 618
        }
    }
}
