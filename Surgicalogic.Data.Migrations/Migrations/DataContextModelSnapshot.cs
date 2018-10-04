﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Surgicalogic.Data.DbContexts;

namespace Surgicalogic.Data.Migrations.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.AppointmentCalendar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AppointmentDate");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("PatientId");

                    b.Property<int>("PersonnelId");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("PersonnelId");

                    b.ToTable("AppointmentCalendars");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<int?>("PersonnelId");

                    b.HasKey("Id");

                    b.HasIndex("PersonnelId");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.DoctorCalendar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("PersonnelId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("PersonnelId");

                    b.ToTable("DoctorCalendars");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(100);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<int>("EquipmentTypeId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsPortable");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("EquipmentTypeId");

                    b.ToTable("Equipments");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.EquipmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("EquipmentTypes");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .HasMaxLength(1000);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.HasKey("Id");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperatingRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<double?>("Height");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsAvailable");

                    b.Property<double?>("Length");

                    b.Property<string>("Location");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<double?>("Width");

                    b.HasKey("Id");

                    b.ToTable("OperatingRooms");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperatingRoomCalendar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("OperatingRoomId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("OperatingRoomId");

                    b.ToTable("OperatingRoomCalendars");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperatingRoomEquipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("EquipmentId");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("OperatingRoomId");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("OperatingRoomId");

                    b.ToTable("OperatingRoomEquipments");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperatingRoomOperationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("OperatingRoomId");

                    b.Property<int>("OperationTypeId");

                    b.HasKey("Id");

                    b.HasIndex("OperatingRoomId");

                    b.HasIndex("OperationTypeId");

                    b.ToTable("OperatingRoomOperationTypes");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<int>("OperationTime");

                    b.Property<int>("OperationTypeId");

                    b.HasKey("Id");

                    b.HasIndex("OperationTypeId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperationBlockedOperatingRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("OperatingRoomId");

                    b.Property<int>("OperationId");

                    b.HasKey("Id");

                    b.HasIndex("OperatingRoomId");

                    b.HasIndex("OperationId");

                    b.ToTable("OperationBlockedOperatingRooms");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperationPersonnel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("OperationId");

                    b.Property<int>("PersonnelId");

                    b.HasKey("Id");

                    b.HasIndex("OperationId");

                    b.HasIndex("PersonnelId");

                    b.ToTable("OperationPersonnels");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperationPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("OperatingRoomId");

                    b.Property<DateTime>("OperationDate");

                    b.Property<int>("OperationId");

                    b.Property<DateTime>("RealizedEndDate");

                    b.Property<DateTime>("RealizedStartDate");

                    b.HasKey("Id");

                    b.HasIndex("OperatingRoomId");

                    b.HasIndex("OperationId");

                    b.ToTable("OperationPlans");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BranchId");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.ToTable("OperationTypes");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperationTypeEquipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("EquipmentId");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("OperationTypeId");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("OperationTypeId");

                    b.ToTable("OperationTypeEquipments");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(500);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("IdentityNumber")
                        .HasMaxLength(11);

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("Phone")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Personnel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BranchId");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("PersonnelCategoryId");

                    b.Property<string>("PersonnelCode")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("PictureUrl");

                    b.Property<int>("WorkTypeId");

                    b.HasKey("Id");

                    b.HasIndex("PersonnelCategoryId");

                    b.HasIndex("WorkTypeId");

                    b.ToTable("Personnels");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.PersonnelBranch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BranchId");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("PersonnelId");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("PersonnelId");

                    b.ToTable("PersonnelBranches");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.PersonnelCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<bool>("SuitableForMultipleOperation");

                    b.HasKey("Id");

                    b.ToTable("PersonnelCategories");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int?>("IntValue");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Key");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<int>("SettingDataTypeId");

                    b.Property<string>("StringValue");

                    b.Property<string>("TimeValue");

                    b.HasKey("Id");

                    b.HasIndex("SettingDataTypeId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.SettingDataType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("SettingDataTypes");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.WorkType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("WorkTypes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Surgicalogic.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.AppointmentCalendar", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.Patient", "Patient")
                        .WithMany("AppointmentCalendars")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Surgicalogic.Data.Entities.Personnel", "Personnel")
                        .WithMany("AppointmentCalendars")
                        .HasForeignKey("PersonnelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Branch", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.Personnel")
                        .WithMany("Branches")
                        .HasForeignKey("PersonnelId");
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.DoctorCalendar", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.Personnel", "Personnel")
                        .WithMany("DoctorCalendars")
                        .HasForeignKey("PersonnelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Equipment", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.EquipmentType", "EquipmentType")
                        .WithMany("Equipments")
                        .HasForeignKey("EquipmentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperatingRoomCalendar", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.OperatingRoom", "OperatingRoom")
                        .WithMany("OperatingRoomCalendars")
                        .HasForeignKey("OperatingRoomId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperatingRoomEquipment", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.Equipment", "Equipment")
                        .WithMany("OperatingRoomEquipments")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Surgicalogic.Data.Entities.OperatingRoom", "OperatingRoom")
                        .WithMany("OperatingRoomEquipments")
                        .HasForeignKey("OperatingRoomId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperatingRoomOperationType", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.OperatingRoom", "OperatingRoom")
                        .WithMany("OperatingRoomOperationTypes")
                        .HasForeignKey("OperatingRoomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Surgicalogic.Data.Entities.OperationType", "OperationType")
                        .WithMany("OperatingRoomOperationTypes")
                        .HasForeignKey("OperationTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Operation", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.OperationType", "OperationType")
                        .WithMany("Operations")
                        .HasForeignKey("OperationTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperationBlockedOperatingRoom", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.OperatingRoom", "OperatingRoom")
                        .WithMany("OperationBlockedOperatingRooms")
                        .HasForeignKey("OperatingRoomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Surgicalogic.Data.Entities.Operation", "Operation")
                        .WithMany("OperationBlockedOperatingRooms")
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperationPersonnel", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.Operation", "Operation")
                        .WithMany("OperationPersonels")
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Surgicalogic.Data.Entities.Personnel", "Personnel")
                        .WithMany("OperationPersonels")
                        .HasForeignKey("PersonnelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperationPlan", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.OperatingRoom", "OperatingRoom")
                        .WithMany()
                        .HasForeignKey("OperatingRoomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Surgicalogic.Data.Entities.Operation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperationType", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.Branch", "Branch")
                        .WithMany("OperationTypes")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.OperationTypeEquipment", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.Equipment", "Equipment")
                        .WithMany("OperationTypeEquipment")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Surgicalogic.Data.Entities.OperationType", "OperationType")
                        .WithMany("OperationTypeEquipment")
                        .HasForeignKey("OperationTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Personnel", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.PersonnelCategory", "PersonnelCategory")
                        .WithMany("Personnels")
                        .HasForeignKey("PersonnelCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Surgicalogic.Data.Entities.WorkType", "WorkType")
                        .WithMany("Personnels")
                        .HasForeignKey("WorkTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.PersonnelBranch", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.Branch", "Branch")
                        .WithMany("PersonnelBranches")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Surgicalogic.Data.Entities.Personnel", "Personnel")
                        .WithMany("PersonnelBranches")
                        .HasForeignKey("PersonnelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Surgicalogic.Data.Entities.Setting", b =>
                {
                    b.HasOne("Surgicalogic.Data.Entities.SettingDataType", "SettingDataType")
                        .WithMany()
                        .HasForeignKey("SettingDataTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
