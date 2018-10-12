using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Surgicalogic.Data.Migrations.Migrations
{
    public partial class PersonnelPhotoAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PersonnelPhoto",
                table: "Personnels",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Patients",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityNumber",
                table: "Patients",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 11);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonnelPhoto",
                table: "Personnels");

            migrationBuilder.AlterColumn<int>(
                name: "Phone",
                table: "Patients",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "IdentityNumber",
                table: "Patients",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 11,
                oldNullable: true);
        }
    }
}
