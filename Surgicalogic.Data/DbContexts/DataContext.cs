using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Data.Entities;

namespace Surgicalogic.Data.DbContexts
{
    public class DataContext : IdentityDbContext<User>
    {
        /// <summary>
        ///  The entity framework context with a Surgicalogic DbSet 
        /// </summary>
        /// <param name="options"></param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<OperatingRoom> OperatingRooms { get; set; }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<PersonnelTitle> PersonnelTitles { get; set; }
        public DbSet<WorkType> WorkTypes { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }
        public DbSet<OperatingRoomEquipment> OperatingRoomEquipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<OperatingRoomEquipment>()
                .HasOne(o => o.OperatingRoom)
                .WithMany(o => o.OperatingRoomEquipments)
                .HasForeignKey(o => o.OperatingRoomId);

            modelBuilder.Entity<OperatingRoomEquipment>()
                .HasOne(o => o.Equipment)
                .WithMany(o => o.OperatingRoomEquipments)
                .HasForeignKey(o => o.EquipmentId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
