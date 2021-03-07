using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DBTestCreator_1.Migrations
{
    public partial class Add_GUID_As_FK_To_Event : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Events");
        }
    }
}
