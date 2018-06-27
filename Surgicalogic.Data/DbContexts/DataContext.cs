using Microsoft.EntityFrameworkCore;
using Surgicalogic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Surgicalogic.Data.DbContexts
{
    public class DataContext : DbContext
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
        public DbSet<WorkType> WorkType { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            //modelBuilder.Entity<Equipment>().ToTable("Equipments");

            //base.OnModelCreating(modelBuilder);
        }
    }
}
