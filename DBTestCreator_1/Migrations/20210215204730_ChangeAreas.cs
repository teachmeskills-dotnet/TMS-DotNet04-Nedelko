using Microsoft.EntityFrameworkCore.Migrations;

namespace DBTestCreator_1.Migrations
{
    public partial class ChangeAreas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doctors_AreaId",
                table: "Doctors");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_AreaId",
                table: "Doctors",
                column: "AreaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doctors_AreaId",
                table: "Doctors");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_AreaId",
                table: "Doctors",
                column: "AreaId",
                unique: true,
                filter: "[AreaId] IS NOT NULL");
        }
    }
}
