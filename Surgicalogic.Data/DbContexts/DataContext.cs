using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Data.Entities;

namespace Surgicalogic.Data.DbContexts
{
    public class DataContext : IdentityDbContext<User, IdentityRole<int>,int>
    {
        /// <summary>
        ///  The entity framework context with a Surgicalogic DbSet 
        /// </summary>
        /// <param name="options"></param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AppointmentCalendar> AppointmentCalendars { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<DoctorCalendar> DoctorCalendars { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<OperationBlockedOperatingRoom> OperationBlockedOperatingRooms { get; set; }
        public DbSet<OperationPlan> OperationPlans { get; set; }
        public DbSet<OperationPersonnel> OperationPersonnels { get; set; }
        public DbSet<OperatingRoom> OperatingRooms { get; set; }
        public DbSet<OperatingRoomCalendar> OperatingRoomCalendars { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }
        public DbSet<OperatingRoomEquipment> OperatingRoomEquipments { get; set; }
        public DbSet<OperatingRoomOperationType> OperatingRoomOperationTypes { get; set; }
        public DbSet<OperationTypeEquipment> OperationTypeEquipments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<PersonnelBranch> PersonnelBranches { get; set; }
        public DbSet<PersonnelCategory> PersonnelCategories { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SettingDataType> SettingDataTypes { get; set; }
        public DbSet<WorkType> WorkTypes { get; set; }


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

            modelBuilder.Entity<OperatingRoomOperationType>()
                .HasOne(o => o.OperatingRoom)
                .WithMany(o => o.OperatingRoomOperationTypes)
                .HasForeignKey(o => o.OperatingRoomId);

            modelBuilder.Entity<OperatingRoomOperationType>()
                .HasOne(o => o.OperationType)
                .WithMany(o => o.OperatingRoomOperationTypes)
                .HasForeignKey(o => o.OperationTypeId);

            modelBuilder.Entity<PersonnelBranch>()
                .HasOne(o => o.Personnel)
                .WithMany(o => o.PersonnelBranches)
                .HasForeignKey(o => o.PersonnelId);

            modelBuilder.Entity<PersonnelBranch>()
                .HasOne(o => o.Branch)
                .WithMany(o => o.PersonnelBranches)
                .HasForeignKey(o => o.BranchId);

            modelBuilder.Entity<OperationTypeEquipment>()
                .HasOne(o => o.OperationType)
                .WithMany(o => o.OperationTypeEquipment)
                .HasForeignKey(o => o.OperationTypeId);

            modelBuilder.Entity<OperationTypeEquipment>()
                .HasOne(o => o.Equipment)
                .WithMany(o => o.OperationTypeEquipment)
                .HasForeignKey(o => o.EquipmentId);

            modelBuilder.Entity<OperationPersonnel>()
                .HasOne(o => o.Personnel)
                .WithMany(o => o.OperationPersonels)
                .HasForeignKey(o => o.PersonnelId);

            modelBuilder.Entity<OperationPersonnel>()
                .HasOne(o => o.Operation)
                .WithMany(o => o.OperationPersonels)
                .HasForeignKey(o => o.OperationId);

            modelBuilder.Entity<OperationBlockedOperatingRoom>()
                .HasOne(o => o.OperatingRoom)
                .WithMany(o => o.OperationBlockedOperatingRooms)
                .HasForeignKey(o => o.OperatingRoomId);

            modelBuilder.Entity<OperationBlockedOperatingRoom>()
                .HasOne(o => o.Operation)
                .WithMany(o => o.OperationBlockedOperatingRooms)
                .HasForeignKey(o => o.OperationId);



            base.OnModelCreating(modelBuilder);
        }
    }
}
