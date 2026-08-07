using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication_Practice.Migrations
{
    /// <inheritdoc />
    public partial class ChangeShoppingListItemmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FoodId",
                table: "ShoppingListItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_FoodId",
                table: "ShoppingListItems",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_Foods_FoodId",
                table: "ShoppingListItems",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_Foods_FoodId",
                table: "ShoppingListItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingListItems_FoodId",
                table: "ShoppingListItems");

            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "ShoppingListItems");
        }
    }
}
