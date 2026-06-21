using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication_Practice.Migrations
{
    /// <inheritdoc />
    public partial class addIngredientsOfFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsOfFood_Foods_FoodId",
                table: "IngredientsOfFood");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsOfFood_Ingredients_IngredientId",
                table: "IngredientsOfFood");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientsOfFood",
                table: "IngredientsOfFood");

            migrationBuilder.RenameTable(
                name: "IngredientsOfFood",
                newName: "IngredientsOfFoods");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientsOfFood_IngredientId",
                table: "IngredientsOfFoods",
                newName: "IX_IngredientsOfFoods_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientsOfFood_FoodId",
                table: "IngredientsOfFoods",
                newName: "IX_IngredientsOfFoods_FoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientsOfFoods",
                table: "IngredientsOfFoods",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsOfFoods_Foods_FoodId",
                table: "IngredientsOfFoods",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsOfFoods_Ingredients_IngredientId",
                table: "IngredientsOfFoods",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsOfFoods_Foods_FoodId",
                table: "IngredientsOfFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsOfFoods_Ingredients_IngredientId",
                table: "IngredientsOfFoods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientsOfFoods",
                table: "IngredientsOfFoods");

            migrationBuilder.RenameTable(
                name: "IngredientsOfFoods",
                newName: "IngredientsOfFood");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientsOfFoods_IngredientId",
                table: "IngredientsOfFood",
                newName: "IX_IngredientsOfFood_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientsOfFoods_FoodId",
                table: "IngredientsOfFood",
                newName: "IX_IngredientsOfFood_FoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientsOfFood",
                table: "IngredientsOfFood",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsOfFood_Foods_FoodId",
                table: "IngredientsOfFood",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsOfFood_Ingredients_IngredientId",
                table: "IngredientsOfFood",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
