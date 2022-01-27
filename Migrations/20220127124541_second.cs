using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkIntro.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyID",
                table: "Ingridients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Ingridients_MyID",
                table: "Ingridients",
                column: "MyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Ingridients_MyID",
                table: "Ingridients");

            migrationBuilder.DropColumn(
                name: "MyID",
                table: "Ingridients");
        }
    }
}
