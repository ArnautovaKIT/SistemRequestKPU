using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemRequestKPU.Migrations
{
    /// <inheritdoc />
    public partial class datafail : Migration
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
                    Description = table.Column<string>(type: "text", nullable: false),
                    TechnicalObjectId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologicalUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnologicalUnits_TechnicalObjects_TechnicalObjectId",
                        column: x => x.TechnicalObjectId,
                        principalTable: "TechnicalObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                    TechnicalObjectId = table.Column<int>(type: "integer", nullable: true),
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
                    { 1, "Площадка ГКС", "Газокомпрессорная станция", "ГКС" },
                    { 2, "Площадка ГРС", "Газораспределительная станция", "ГРС" }
                });

            migrationBuilder.InsertData(
                table: "EquipmentTypes",
                columns: new[] { "Id", "Manufacturer", "Model", "Name", "Specifications" },
                values: new object[,]
                {
                    { 1, "KPU", "CTRL-MAIN", "Главный контроллер управления", "Контроллер" },
                    { 2, "KPU", "ASU", "Система автоматического управления", "АСУ" },
                    { 3, "KPU", "RPM-TURB", "Датчик оборотов двигателя основной турбины", "Датчик оборотов" },
                    { 4, "KPU", "TEMP-TURB", "Датчик температуры основной турбины", "Датчик температуры" },
                    { 5, "KPU", "RPM-COMP", "Датчик оборотов нагнетателя", "Датчик оборотов" },
                    { 6, "KPU", "AUX", "Вспомогательное оборудование", "Вспомогательное" },
                    { 7, "KPU", "PRESS-FILTER", "Датчик давления основного фильтра", "Датчик давления" },
                    { 8, "KPU", "PRESS-IN", "Датчик давления на входе", "Датчик давления" },
                    { 9, "KPU", "TEMP-IN", "Датчик температуры на входе", "Датчик температуры" },
                    { 10, "KPU", "PRESS-REG", "Датчик регулятора давления", "Датчик давления" },
                    { 11, "KPU", "VIB-REG", "Датчик вибрации регулятора", "Датчик вибрации" },
                    { 12, "KPU", "TEMP-REG", "Датчик температуры регулятора", "Датчик температуры" },
                    { 13, "KPU", "CTRL-BOARD", "Контроллер платы управления", "Контроллер" },
                    { 14, "KPU", "TEMP-HEATER", "Датчик температуры подогревателя", "Датчик температуры" },
                    { 15, "KPU", "FLAME", "Датчик наличия пламени", "Датчик пламени" },
                    { 16, "KPU", "ODOR-LEVEL", "Датчик уровня расхода одоранта", "Датчик уровня" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "admin@example.com", "$2a$11$RdP4N9/ic6c22zNIFDry5OV9lArcoiWYhxAaTlvia/oShaja2gGwy", 3, "admin" },
                    { 2, "dispatcher@example.com", "$2a$11$JS3m9UoF9XzBMx9OPk3.c.Eh7Wfk6pwqAtb7AY7e0g/uimS/hSrRy", 2, "dispatcher" },
                    { 3, "executor1@example.com", "$2a$11$N.zPWI4RnJ2X1hmEhZJj5O9bTZZ9ohiW6L0k4cFwiM22ptwmjhHem", 1, "executor1" },
                    { 4, "executor2@example.com", "$2a$11$BmVM91CKi9BFK/UcocMCI..Co.Psf6gx2o/ieK.AAPXvtJt7jCmLq", 1, "executor2" },
                    { 5, "applicant@example.com", "$2a$11$ohOk23FnBCHoI86p9ZmnHujdTYY.mha3JYr6ITiPqhanwxh04D.Dm", 0, "applicant" }
                });

            migrationBuilder.InsertData(
                table: "TechnicalObjects",
                columns: new[] { "Id", "ComplexId", "InstallationDate", "IsGRS", "Name", "ObjectType" },
                values: new object[,]
                {
                    { 1, 1, null, false, "Газокомпрессорная станция", "ГКС" },
                    { 2, 2, null, false, "Газораспределительная станция", "ГРС" }
                });

            migrationBuilder.InsertData(
                table: "Workshops",
                columns: new[] { "Id", "Code", "Name", "ResponsiblePersonId" },
                values: new object[,]
                {
                    { 1, "КЦ1", "Компрессорный цех №1", 1 },
                    { 2, "КЦ2", "Компрессорный цех №2", 1 },
                    { 3, "КЦ3", "Компрессорный цех №3", 1 },
                    { 4, "ГРС1", "ГРС №1", 1 },
                    { 5, "ГРС2", "ГРС №2", 1 },
                    { 6, "ГРС3", "ГРС №3", 1 }
                });

            migrationBuilder.InsertData(
                table: "TechnologicalUnits",
                columns: new[] { "Id", "Code", "Description", "Name", "TechnicalObjectId", "WorkshopId" },
                values: new object[,]
                {
                    { 1, "ГПА-1.1", "", "Газоперекачивающий агрегат №1.1", 1, 1 },
                    { 2, "ГПА-1.2", "", "Газоперекачивающий агрегат №1.2", 1, 1 },
                    { 3, "ГПА-2.1", "", "Газоперекачивающий агрегат №2.1", 1, 2 },
                    { 4, "ГПА-2.2", "", "Газоперекачивающий агрегат №2.2", 1, 2 },
                    { 5, "ГПА-3.1", "", "Газоперекачивающий агрегат №3.1", 1, 3 },
                    { 6, "ГПА-3.2", "", "Газоперекачивающий агрегат №3.2", 1, 3 },
                    { 7, "УОГ-1", "", "Узел очистки газа", 2, 4 },
                    { 8, "УРГ-1", "", "Узел редуцирования газа", 2, 4 },
                    { 9, "УПОГ-1", "", "Узел подогрева и одоризации газа", 2, 4 },
                    { 10, "УОГ-2", "", "Узел очистки газа", 2, 5 },
                    { 11, "УРГ-2", "", "Узел редуцирования газа", 2, 5 },
                    { 12, "УПОГ-2", "", "Узел подогрева и одоризации газа", 2, 5 },
                    { 13, "УОГ-3", "", "Узел очистки газа", 2, 6 },
                    { 14, "УРГ-3", "", "Узел редуцирования газа", 2, 6 },
                    { 15, "УПОГ-3", "", "Узел подогрева и одоризации газа", 2, 6 }
                });

            migrationBuilder.InsertData(
                table: "EquipmentInstances",
                columns: new[] { "Id", "CurrentStatus", "EquipmentTypeId", "FactoryNumber", "InstallationDate", "InventoryNumber", "LastMaintenanceDate", "NextMaintenanceDate", "StationNumber", "TechnicalNumber", "TechnicalObjectId", "TechnologicalUnitId" },
                values: new object[,]
                {
                    { 1, "", 1, "", null, "ГПА1.1-001", null, null, "", "", 1, 1 },
                    { 2, "", 2, "", null, "ГПА1.1-002", null, null, "", "", 1, 1 },
                    { 3, "", 3, "", null, "ГПА1.1-003", null, null, "", "", 1, 1 },
                    { 4, "", 4, "", null, "ГПА1.1-004", null, null, "", "", 1, 1 },
                    { 5, "", 5, "", null, "ГПА1.1-005", null, null, "", "", 1, 1 },
                    { 6, "", 6, "", null, "ГПА1.1-006", null, null, "", "", 1, 1 },
                    { 7, "", 1, "", null, "ГПА1.2-001", null, null, "", "", 1, 2 },
                    { 8, "", 2, "", null, "ГПА1.2-002", null, null, "", "", 1, 2 },
                    { 9, "", 3, "", null, "ГПА1.2-003", null, null, "", "", 1, 2 },
                    { 10, "", 4, "", null, "ГПА1.2-004", null, null, "", "", 1, 2 },
                    { 11, "", 5, "", null, "ГПА1.2-005", null, null, "", "", 1, 2 },
                    { 12, "", 6, "", null, "ГПА1.2-006", null, null, "", "", 1, 2 },
                    { 13, "", 1, "", null, "ГПА2.1-001", null, null, "", "", 1, 3 },
                    { 14, "", 2, "", null, "ГПА2.1-002", null, null, "", "", 1, 3 },
                    { 15, "", 3, "", null, "ГПА2.1-003", null, null, "", "", 1, 3 },
                    { 16, "", 4, "", null, "ГПА2.1-004", null, null, "", "", 1, 3 },
                    { 17, "", 5, "", null, "ГПА2.1-005", null, null, "", "", 1, 3 },
                    { 18, "", 6, "", null, "ГПА2.1-006", null, null, "", "", 1, 3 },
                    { 19, "", 1, "", null, "ГПА2.2-001", null, null, "", "", 1, 4 },
                    { 20, "", 2, "", null, "ГПА2.2-002", null, null, "", "", 1, 4 },
                    { 21, "", 3, "", null, "ГПА2.2-003", null, null, "", "", 1, 4 },
                    { 22, "", 4, "", null, "ГПА2.2-004", null, null, "", "", 1, 4 },
                    { 23, "", 5, "", null, "ГПА2.2-005", null, null, "", "", 1, 4 },
                    { 24, "", 6, "", null, "ГПА2.2-006", null, null, "", "", 1, 4 },
                    { 25, "", 1, "", null, "ГПА3.1-001", null, null, "", "", 1, 5 },
                    { 26, "", 2, "", null, "ГПА3.1-002", null, null, "", "", 1, 5 },
                    { 27, "", 3, "", null, "ГПА3.1-003", null, null, "", "", 1, 5 },
                    { 28, "", 4, "", null, "ГПА3.1-004", null, null, "", "", 1, 5 },
                    { 29, "", 5, "", null, "ГПА3.1-005", null, null, "", "", 1, 5 },
                    { 30, "", 6, "", null, "ГПА3.1-006", null, null, "", "", 1, 5 },
                    { 31, "", 1, "", null, "ГПА3.2-001", null, null, "", "", 1, 6 },
                    { 32, "", 2, "", null, "ГПА3.2-002", null, null, "", "", 1, 6 },
                    { 33, "", 3, "", null, "ГПА3.2-003", null, null, "", "", 1, 6 },
                    { 34, "", 4, "", null, "ГПА3.2-004", null, null, "", "", 1, 6 },
                    { 35, "", 5, "", null, "ГПА3.2-005", null, null, "", "", 1, 6 },
                    { 36, "", 6, "", null, "ГПА3.2-006", null, null, "", "", 1, 6 },
                    { 37, "", 7, "", null, "ГРС1-УОГ-001", null, null, "", "", 2, 7 },
                    { 38, "", 8, "", null, "ГРС1-УОГ-002", null, null, "", "", 2, 7 },
                    { 39, "", 9, "", null, "ГРС1-УОГ-003", null, null, "", "", 2, 7 },
                    { 40, "", 10, "", null, "ГРС1-УРГ-001", null, null, "", "", 2, 8 },
                    { 41, "", 11, "", null, "ГРС1-УРГ-002", null, null, "", "", 2, 8 },
                    { 42, "", 12, "", null, "ГРС1-УРГ-003", null, null, "", "", 2, 8 },
                    { 43, "", 13, "", null, "ГРС1-УРГ-004", null, null, "", "", 2, 8 },
                    { 44, "", 14, "", null, "ГРС1-УПОГ-001", null, null, "", "", 2, 9 },
                    { 45, "", 15, "", null, "ГРС1-УПОГ-002", null, null, "", "", 2, 9 },
                    { 46, "", 16, "", null, "ГРС1-УПОГ-003", null, null, "", "", 2, 9 },
                    { 47, "", 13, "", null, "ГРС1-УПОГ-004", null, null, "", "", 2, 9 },
                    { 48, "", 7, "", null, "ГРС2-УОГ-001", null, null, "", "", 2, 10 },
                    { 49, "", 8, "", null, "ГРС2-УОГ-002", null, null, "", "", 2, 10 },
                    { 50, "", 9, "", null, "ГРС2-УОГ-003", null, null, "", "", 2, 10 },
                    { 51, "", 10, "", null, "ГРС2-УРГ-001", null, null, "", "", 2, 11 },
                    { 52, "", 11, "", null, "ГРС2-УРГ-002", null, null, "", "", 2, 11 },
                    { 53, "", 12, "", null, "ГРС2-УРГ-003", null, null, "", "", 2, 11 },
                    { 54, "", 13, "", null, "ГРС2-УРГ-004", null, null, "", "", 2, 11 },
                    { 55, "", 14, "", null, "ГРС2-УПОГ-001", null, null, "", "", 2, 12 },
                    { 56, "", 15, "", null, "ГРС2-УПОГ-002", null, null, "", "", 2, 12 },
                    { 57, "", 16, "", null, "ГРС2-УПОГ-003", null, null, "", "", 2, 12 },
                    { 58, "", 13, "", null, "ГРС2-УПОГ-004", null, null, "", "", 2, 12 },
                    { 59, "", 7, "", null, "ГРС3-УОГ-001", null, null, "", "", 2, 13 },
                    { 60, "", 8, "", null, "ГРС3-УОГ-002", null, null, "", "", 2, 13 },
                    { 61, "", 9, "", null, "ГРС3-УОГ-003", null, null, "", "", 2, 13 },
                    { 62, "", 10, "", null, "ГРС3-УРГ-001", null, null, "", "", 2, 14 },
                    { 63, "", 11, "", null, "ГРС3-УРГ-002", null, null, "", "", 2, 14 },
                    { 64, "", 12, "", null, "ГРС3-УРГ-003", null, null, "", "", 2, 14 },
                    { 65, "", 13, "", null, "ГРС3-УРГ-004", null, null, "", "", 2, 14 },
                    { 66, "", 14, "", null, "ГРС3-УПОГ-001", null, null, "", "", 2, 15 },
                    { 67, "", 15, "", null, "ГРС3-УПОГ-002", null, null, "", "", 2, 15 },
                    { 68, "", 16, "", null, "ГРС3-УПОГ-003", null, null, "", "", 2, 15 },
                    { 69, "", 13, "", null, "ГРС3-УПОГ-004", null, null, "", "", 2, 15 }
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
                name: "IX_TechnologicalUnits_TechnicalObjectId",
                table: "TechnologicalUnits",
                column: "TechnicalObjectId");

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
                name: "TechnologicalUnits");

            migrationBuilder.DropTable(
                name: "TechnicalObjects");

            migrationBuilder.DropTable(
                name: "Workshops");

            migrationBuilder.DropTable(
                name: "Complexes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
