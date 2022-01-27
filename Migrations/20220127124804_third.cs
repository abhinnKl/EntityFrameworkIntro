using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkIntro.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingridients_Dishes_DishId",
                table: "Ingridients");

            migrationBuilder.DropIndex(
                name: "IX_Ingridients_DishId",
                table: "Ingridients");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "Ingridients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DishId",
                table: "Ingridients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ingridients_DishId",
                table: "Ingridients",
                column: "DishId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingridients_Dishes_DishId",
                table: "Ingridients",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
