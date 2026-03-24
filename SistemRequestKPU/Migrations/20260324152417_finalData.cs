using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemRequestKPU.Migrations
{
    /// <inheritdoc />
    public partial class finalData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Complexes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Complexes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CurrentStatus", "InventoryNumber" },
                values: new object[] { "", "ГПА1.1-КУ-001" });

            migrationBuilder.UpdateData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CurrentStatus", "InventoryNumber" },
                values: new object[] { "", "ГПА1.1-АСУ-001" });

            migrationBuilder.UpdateData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CurrentStatus", "InventoryNumber" },
                values: new object[] { "", "ГПА1.1-ДОД-001" });

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 1,
                column: "InstallationDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 2,
                column: "InstallationDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Code", "Description", "Name", "TechnicalObjectId", "WorkshopId" },
                values: new object[] { "ГПА-2.2", "", "Газоперекачивающий агрегат №2.2", 1, 2 });

            migrationBuilder.InsertData(
                table: "TechnologicalUnits",
                columns: new[] { "Id", "Code", "Description", "Name", "TechnicalObjectId", "WorkshopId" },
                values: new object[,]
                {
                    { 5, "ГПА-3.1", "", "Газоперекачивающий агрегат №3.1", 1, 3 },
                    { 6, "ГПА-3.2", "", "Газоперекачивающий агрегат №3.2", 1, 3 },
                    { 7, "УОГ", "", "Узел очистки газа", 2, 4 },
                    { 8, "УРГ", "", "Узел редуцирования газа", 2, 4 },
                    { 9, "УПОГ", "", "Узел подогрева и одоризации газа", 2, 4 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$woj0R5P2pi9VZdbeDO9ROezoiaaVSB8ZP2aKbEFgAouhByvgCXi1S");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$T/q7iRi4Cb.Zdf5.WZ23.uIS0pKC7Pnpg2Ch9ZJnkPPo4j89Zws9G");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$ka7hiJ520wOzTaOmmErT6.JWrQVHKPRlTZ7yyx/.Hzx6gsaSEqxWC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$8t3RMwBFgDrJLqpNxltqdejEnEAJoCOvn.4Skpw/H2vrx3xp.89ou");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "$2a$11$RUllYUgwYwSuz/jj/1/OxOUk92OnyCntohSRLxbDuG20bvBmrenwW");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.InsertData(
                table: "Complexes",
                columns: new[] { "Id", "Location", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Воткинск", "Газокомпрессорная станция", "ГКС" },
                    { 2, "Чайковский", "Газораспределительная станция", "ГРС" }
                });

            migrationBuilder.UpdateData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CurrentStatus", "InventoryNumber" },
                values: new object[] { "В работе", "INV-001" });

            migrationBuilder.UpdateData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CurrentStatus", "InventoryNumber" },
                values: new object[] { "В работе", "INV-002" });

            migrationBuilder.UpdateData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CurrentStatus", "InventoryNumber" },
                values: new object[] { "В работе", "INV-003" });

            migrationBuilder.InsertData(
                table: "EquipmentTypes",
                columns: new[] { "Id", "Manufacturer", "Model", "Name", "Specifications" },
                values: new object[,]
                {
                    { 1, "Siemens", "S7-1500", "Главный контроллер управления", "Управление ГПА" },
                    { 2, "ABB", "AC800M", "Система автоматического управления", "АСУ ТП" },
                    { 3, "Emerson", "Rosemount", "Датчик оборотов двигателя", "Измерение оборотов" }
                });

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 1,
                column: "InstallationDate",
                value: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 2,
                column: "InstallationDate",
                value: new DateTime(2019, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Агрегат 1.1");

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Агрегат 1.2");

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Агрегат 2.1");

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Code", "Description", "Name", "TechnicalObjectId", "WorkshopId" },
                values: new object[] { "РУ-1", "Редукционный узел", "Редукционный узел №1", 2, 4 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$bvKYTC3IGz4ajmB/Gghr3eUna1n2.TRC5qKni/3FDkcFfUb8MXpBq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$.1gbrO.sLdwmqt5jlwdVk.6WLEqXmMauc424IEeGAUXO/AM2gV6ny");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$TSJuZRGSWy6aCGgHuvufnuIs.NmxD1kCuteNIEwWFdl8egGQmaEfi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$I1yW4TLE/9PQUxA7vJEoYun5aGjSU4vpQYOB9BUlXb1jSHgcALKGm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "$2a$11$clmjN6zpZm/N5GZH6rgm2Ov/eieP4NVtUMcUpTOMHe3l3uHMzoXOm");
        }
    }
}
