using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("671b4025-a134-45e0-9e67-0c7f1a308967"),
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("868d8763-125c-4384-8149-c12e52e50529"),
                column: "Active",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("671b4025-a134-45e0-9e67-0c7f1a308967"),
                column: "Active",
                value: false);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("868d8763-125c-4384-8149-c12e52e50529"),
                column: "Active",
                value: false);
        }
    }
}
