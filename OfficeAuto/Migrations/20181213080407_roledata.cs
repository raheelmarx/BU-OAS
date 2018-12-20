using Microsoft.EntityFrameworkCore.Migrations;

namespace OfficeAuto.Migrations
{
    public partial class roledata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeptId",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: 0);
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "DeptId",
               table: "AspNetRoles");
        }
    }
}
