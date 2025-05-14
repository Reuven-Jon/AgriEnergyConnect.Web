using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AgriEnergyConnect.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Farmers_FarmerId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FarmerId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FarmerId1",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Farmers",
                columns: new[] { "Id", "Address", "Email", "FullName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, null, "alice@farm.co.za", "Alice Green", null },
                    { 2, null, "bob@farm.co.za", "Bob Fields", null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ApplicationUserId", "Category", "CategoryId", "CreatedAt", "Description", "FarmerId", "Name", "ProductionDate" },
                values: new object[,]
                {
                    { 1, null, "Grains", null, new DateTime(2025, 5, 14, 16, 32, 56, 58, DateTimeKind.Utc).AddTicks(617), null, 1, "Sunflower Seeds", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, null, "Grains", null, new DateTime(2025, 5, 14, 16, 32, 56, 58, DateTimeKind.Utc).AddTicks(622), null, 2, "Organic Wheat", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Farmers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Farmers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FarmerId1",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmerId1",
                table: "Products",
                column: "FarmerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Farmers_FarmerId1",
                table: "Products",
                column: "FarmerId1",
                principalTable: "Farmers",
                principalColumn: "Id");
        }
    }
}
