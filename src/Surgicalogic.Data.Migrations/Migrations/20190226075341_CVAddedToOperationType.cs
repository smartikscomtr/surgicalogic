using Microsoft.EntityFrameworkCore.Migrations;

namespace Surgicalogic.Data.Migrations.Migrations
{
    public partial class CVAddedToOperationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CoefficientOfVariation",
                table: "OperationTypes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoefficientOfVariation",
                table: "OperationTypes");
        }
    }
}
