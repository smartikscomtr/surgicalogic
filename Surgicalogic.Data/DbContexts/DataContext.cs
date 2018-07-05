using Microsoft.EntityFrameworkCore;
using Surgicalogic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Surgicalogic.Data.DbContexts
{
    public class DataContext : IdentityDbContext<User>
    {
        /// <summary>
        ///  The entity framework context with a Surgicalogic DbSet 
        /// </summary>
        /// <param name="options"></param>
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<OperatingRoom> OperatingRooms { get; set; }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<PersonnelTitle> PersonnelTitles { get; set; }
        public DbSet<WorkType> WorkTypes { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.EquipmentType)
                .WithMany(e => e.Equipments);

            modelBuilder.Entity<EquipmentType>()
                .HasMany(e => e.Equipments)
                .WithOne(e => e.EquipmentType)
                .IsRequired();



            base.OnModelCreating(modelBuilder);
        }
    }
}
