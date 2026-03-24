using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemRequestKPU.Migrations
{
    /// <inheritdoc />
    public partial class migrationDataKPU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Complexes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complexes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Specifications = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ComplexId = table.Column<int>(type: "integer", nullable: false),
                    ObjectType = table.Column<string>(type: "text", nullable: false),
                    InstallationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsGRS = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalObjects_Complexes_ComplexId",
                        column: x => x.ComplexId,
                        principalTable: "Complexes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workshops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    ResponsiblePersonId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workshops_Users_ResponsiblePersonId",
                        column: x => x.ResponsiblePersonId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TechnologicalUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    WorkshopId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologicalUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnologicalUnits_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EquipmentTypeId = table.Column<int>(type: "integer", nullable: false),
                    InventoryNumber = table.Column<string>(type: "text", nullable: false),
                    FactoryNumber = table.Column<string>(type: "text", nullable: false),
                    StationNumber = table.Column<string>(type: "text", nullable: false),
                    TechnicalNumber = table.Column<string>(type: "text", nullable: false),
                    InstallationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TechnicalObjectId = table.Column<int>(type: "integer", nullable: false),
                    TechnologicalUnitId = table.Column<int>(type: "integer", nullable: true),
                    CurrentStatus = table.Column<string>(type: "text", nullable: false),
                    LastMaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentInstances_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentInstances_TechnicalObjects_TechnicalObjectId",
                        column: x => x.TechnicalObjectId,
                        principalTable: "TechnicalObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentInstances_TechnologicalUnits_TechnologicalUnitId",
                        column: x => x.TechnologicalUnitId,
                        principalTable: "TechnologicalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EquipmentInstanceId = table.Column<int>(type: "integer", nullable: false),
                    ParameterName = table.Column<string>(type: "text", nullable: false),
                    MinValue = table.Column<double>(type: "double precision", nullable: true),
                    MaxValue = table.Column<double>(type: "double precision", nullable: true),
                    NormalValue = table.Column<double>(type: "double precision", nullable: true),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    AccuracyClass = table.Column<string>(type: "text", nullable: false),
                    MeasurementRange = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentParameters_EquipmentInstances_EquipmentInstanceId",
                        column: x => x.EquipmentInstanceId,
                        principalTable: "EquipmentInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UniqueNumber = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WorkType = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TechnicalSpecs = table.Column<string>(type: "text", nullable: false),
                    Requirements = table.Column<string>(type: "text", nullable: true),
                    EquipmentInstanceId = table.Column<int>(type: "integer", nullable: false),
                    TechnicalObjectId = table.Column<int>(type: "integer", nullable: false),
                    AssigneeId = table.Column<int>(type: "integer", nullable: true),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    WorkshopId = table.Column<int>(type: "integer", nullable: false),
                    TechnologicalUnitId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_EquipmentInstances_EquipmentInstanceId",
                        column: x => x.EquipmentInstanceId,
                        principalTable: "EquipmentInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_TechnicalObjects_TechnicalObjectId",
                        column: x => x.TechnicalObjectId,
                        principalTable: "TechnicalObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_TechnologicalUnits_TechnologicalUnitId",
                        column: x => x.TechnologicalUnitId,
                        principalTable: "TechnologicalUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Users_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Complexes",
                columns: new[] { "Id", "Location", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Воткинск", "Газокомпрессорная станция", "ГКС" },
                    { 2, "Чайковский", "Газораспределительная станция", "ГРС" }
                });

            migrationBuilder.InsertData(
                table: "EquipmentTypes",
                columns: new[] { "Id", "Manufacturer", "Model", "Name", "Specifications" },
                values: new object[,]
                {
                    { 1, "Siemens", "S7-1500", "Главный контроллер управления", "Управление ГПА" },
                    { 2, "ABB", "AC800M", "Система автоматического управления", "АСУ ТП" },
                    { 3, "Emerson", "Rosemount", "Датчик оборотов двигателя", "Измерение оборотов" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "admin@example.com", "$2a$11$cjdnnXGom3XXZpY7c2SRrOxs1G6GpW1N1UiM2mCQDWPeFgDTSHb82", 3, "admin" },
                    { 2, "dispatcher@example.com", "$2a$11$/34lK4o2aWth4tGo2KGElurDdrmj6sIbAtoXPNVKFRYKscuFRjoPm", 2, "dispatcher" },
                    { 3, "executor1@example.com", "$2a$11$f5nbsKuxpbbiM2Irp7psoe8.pvL1.9156FNJSrlOsep3v1iLlm.y2", 1, "executor1" },
                    { 4, "executor2@example.com", "$2a$11$h1GverJ.w5pCLv5tQIp8GekGrS2F957HT7bSkpQjmn5/OWX7pPXEK", 1, "executor2" },
                    { 5, "applicant@example.com", "$2a$11$OE/VtaNlWxDLH8L89SWqQ.AsTgAi73q5pig47ta8zxIANWVAj8QKW", 0, "applicant" }
                });

            migrationBuilder.InsertData(
                table: "TechnicalObjects",
                columns: new[] { "Id", "ComplexId", "InstallationDate", "IsGRS", "Name", "ObjectType" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Газокомпрессорная станция", "ГКС" },
                    { 2, 2, new DateTime(2019, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Газораспределительная станция", "ГРС" }
                });

            migrationBuilder.InsertData(
                table: "Workshops",
                columns: new[] { "Id", "Code", "Name", "ResponsiblePersonId" },
                values: new object[,]
                {
                    { 1, "КЦ1", "Компрессорный цех №1", 1 },
                    { 2, "КЦ2", "Компрессорный цех №2", 1 },
                    { 3, "КЦ3", "Компрессорный цех №3", 1 },
                    { 4, "РЦ1", "Распределительный цех №1", 1 }
                });

            migrationBuilder.InsertData(
                table: "TechnologicalUnits",
                columns: new[] { "Id", "Code", "Description", "Name", "WorkshopId" },
                values: new object[,]
                {
                    { 1, "ГПА-1.1", "Агрегат 1.1", "Газоперекачивающий агрегат №1.1", 1 },
                    { 2, "ГПА-1.2", "Агрегат 1.2", "Газоперекачивающий агрегат №1.2", 1 },
                    { 3, "ГПА-2.1", "Агрегат 2.1", "Газоперекачивающий агрегат №2.1", 2 },
                    { 4, "РУ-1", "Редукционный узел", "Редукционный узел №1", 4 }
                });

            migrationBuilder.InsertData(
                table: "EquipmentInstances",
                columns: new[] { "Id", "CurrentStatus", "EquipmentTypeId", "FactoryNumber", "InstallationDate", "InventoryNumber", "LastMaintenanceDate", "NextMaintenanceDate", "StationNumber", "TechnicalNumber", "TechnicalObjectId", "TechnologicalUnitId" },
                values: new object[,]
                {
                    { 1, "В работе", 1, "", null, "INV-001", null, null, "", "", 1, 1 },
                    { 2, "В работе", 2, "", null, "INV-002", null, null, "", "", 1, 1 },
                    { 3, "В работе", 3, "", null, "INV-003", null, null, "", "", 1, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentInstances_EquipmentTypeId",
                table: "EquipmentInstances",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentInstances_TechnicalObjectId",
                table: "EquipmentInstances",
                column: "TechnicalObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentInstances_TechnologicalUnitId",
                table: "EquipmentInstances",
                column: "TechnologicalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentParameters_EquipmentInstanceId",
                table: "EquipmentParameters",
                column: "EquipmentInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AssigneeId",
                table: "Requests",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CreatorId",
                table: "Requests",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_EquipmentInstanceId",
                table: "Requests",
                column: "EquipmentInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_TechnicalObjectId",
                table: "Requests",
                column: "TechnicalObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_TechnologicalUnitId",
                table: "Requests",
                column: "TechnologicalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_WorkshopId",
                table: "Requests",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalObjects_ComplexId",
                table: "TechnicalObjects",
                column: "ComplexId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalUnits_WorkshopId",
                table: "TechnologicalUnits",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_ResponsiblePersonId",
                table: "Workshops",
                column: "ResponsiblePersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentParameters");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "EquipmentInstances");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "TechnicalObjects");

            migrationBuilder.DropTable(
                name: "TechnologicalUnits");

            migrationBuilder.DropTable(
                name: "Complexes");

            migrationBuilder.DropTable(
                name: "Workshops");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
