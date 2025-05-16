using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class newUpdates04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "ScheduleDevices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ScheduleDevices",
                keyColumn: "Id",
                keyValue: 1,
                column: "AccessToken",
                value: "Tokken01");

            migrationBuilder.UpdateData(
                table: "ScheduleDevices",
                keyColumn: "Id",
                keyValue: 2,
                column: "AccessToken",
                value: "Tokken02");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 16, 21, 54, 43, 777, DateTimeKind.Local).AddTicks(509));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "ScheduleDevices");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 16, 15, 44, 35, 426, DateTimeKind.Local).AddTicks(919));
        }
    }
}
