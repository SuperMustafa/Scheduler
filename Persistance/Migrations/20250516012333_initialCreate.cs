using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SelectedDays = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuildingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleDevices_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleDevices_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleExecutionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttributesSent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleExecutionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleExecutionLogs_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDeviceAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleDeviceId = table.Column<int>(type: "int", nullable: false),
                    AttributeKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDeviceAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleDeviceAttributes_ScheduleDevices_ScheduleDeviceId",
                        column: x => x.ScheduleDeviceId,
                        principalTable: "ScheduleDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Building A" },
                    { 2, "Building B" }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "AccessToken", "BuildingId", "UnitName", "UnitType" },
                values: new object[,]
                {
                    { 1, "007ZOOgKyvfxNG11f9Vs", 1, "AC101", "AirConditioner" },
                    { 2, "DEVICE_TOKEN_2", 2, "AC102", "AirConditioner" }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "BuildingId", "CreatedAt", "EndTime", "IsActive", "ScheduleName", "SelectedDays", "StartTime" },
                values: new object[] { 1, 1, new DateTime(2025, 5, 16, 4, 23, 32, 742, DateTimeKind.Local).AddTicks(6390), new TimeSpan(0, 12, 0, 0, 0), true, "Morning Cooling", "Monday,Wednesday,Friday", new TimeSpan(0, 8, 0, 0, 0) });

            migrationBuilder.InsertData(
                table: "ScheduleDevices",
                columns: new[] { "Id", "DeviceId", "ScheduleId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "ScheduleDeviceAttributes",
                columns: new[] { "Id", "AttributeKey", "AttributeValue", "ScheduleDeviceId" },
                values: new object[,]
                {
                    { 1, "temperature", "22", 1 },
                    { 2, "fanSpeed", "Medium", 1 },
                    { 3, "temperature", "24", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_BuildingId",
                table: "Devices",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDeviceAttributes_ScheduleDeviceId",
                table: "ScheduleDeviceAttributes",
                column: "ScheduleDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDevices_DeviceId",
                table: "ScheduleDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDevices_ScheduleId",
                table: "ScheduleDevices",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleExecutionLogs_ScheduleId",
                table: "ScheduleExecutionLogs",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_BuildingId",
                table: "Schedules",
                column: "BuildingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleDeviceAttributes");

            migrationBuilder.DropTable(
                name: "ScheduleExecutionLogs");

            migrationBuilder.DropTable(
                name: "ScheduleDevices");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
