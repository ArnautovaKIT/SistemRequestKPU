using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemRequestKPU.Migrations
{
    /// <inheritdoc />
    public partial class Seedestata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Complexes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Location", "Name", "Type" },
                values: new object[] { "г. Москва, ул. Промышленная, д. 10", "Газоперерабатывающий комплекс", "ГПЗ" });

            migrationBuilder.InsertData(
                table: "Complexes",
                columns: new[] { "Id", "Location", "Name", "Type" },
                values: new object[] { 2, "г. Санкт-Петербург, пр. Энергетиков, д. 5", "Нефтеперерабатывающий комплекс", "НПЗ" });

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Specifications",
                value: "Диапазон 0-100 бар, класс точности 0.5");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Specifications",
                value: "Управление КИПиА, 16 каналов ввода-вывода");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Model", "Specifications" },
                values: new object[] { "Rosemount 8600", "Контроль расхода газа, диапазон 0-5000 м³/ч" });

            migrationBuilder.InsertData(
                table: "EquipmentTypes",
                columns: new[] { "Id", "Manufacturer", "Model", "Name", "Specifications" },
                values: new object[,]
                {
                    { 4, "Yokogawa", "YT-500", "Датчик температуры", "Диапазон -50°C до +300°C, класс точности 0.2" },
                    { 5, "Festo", "MPYE-5-1/8", "Электромагнитный клапан", "Управление потоком, давление до 10 бар" }
                });

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 1,
                column: "InstallationDate",
                value: new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "InstallationDate", "Name" },
                values: new object[] { new DateTime(2018, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "КС-5" });

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Газопровод-10");

            migrationBuilder.InsertData(
                table: "TechnologicalUnits",
                columns: new[] { "Id", "Code", "Description", "Name", "WorkshopId" },
                values: new object[,]
                {
                    { 1, "GRS-1", "Основная газораспределительная станция комплекса", "Газораспределительная станция", 1 },
                    { 2, "KU-2", "Компрессорная установка для повышения давления", "Компрессорная установка", 1 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$npWjEbaLSV5.pU5CNP4b.eEOsZFgm3mPhYiIswFpeX9B66mU7dSpu");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 2, "dispatcher@example.com", "$2a$11$b/oJ9t4zFDXLD85hqyfNIO3BCJ5gXDRKZAsFGyNEGt4MkPPRfz.ci", 2, "dispatcher" },
                    { 3, "executor1@example.com", "$2a$11$oPLB3Ke53iq.2oxJhYHmXeGGZh2PoP6wv2RmK/NOSqirMkXdZ.tGO", 1, "executor1" },
                    { 4, "executor2@example.com", "$2a$11$gmvr2aHbIRqpQpPVzzcBfO3jc0dYytzYpB0xoixVTMylrmk/LHasK", 1, "executor2" },
                    { 5, "applicant@example.com", "$2a$11$FzqWnnS2U9NcXEFoGLADJOS0JnyIXAUefTcQp8sb5UmXMI9Oq3tK.", 0, "applicant" }
                });

            migrationBuilder.UpdateData(
                table: "Workshops",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "Name", "ResponsiblePersonId" },
                values: new object[] { "CA-1", "Цех автоматизации", 2 });

            migrationBuilder.InsertData(
                table: "EquipmentInstances",
                columns: new[] { "Id", "CurrentStatus", "EquipmentTypeId", "FactoryNumber", "InstallationDate", "InventoryNumber", "LastMaintenanceDate", "NextMaintenanceDate", "StationNumber", "TechnicalNumber", "TechnicalObjectId", "TechnologicalUnitId" },
                values: new object[,]
                {
                    { 1, "В работе", 1, "FAC-1001", new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "INV-001", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), "ST-001", "TECH-001", 1, 1 },
                    { 2, "В работе", 2, "FAC-1002", new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "INV-002", new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), "ST-002", "TECH-002", 1, 1 },
                    { 3, "Требует обслуживания", 3, "FAC-1003", new DateTime(2018, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), "INV-003", new DateTime(2022, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), "ST-003", "TECH-003", 2, 2 },
                    { 4, "В работе", 4, "FAC-1004", new DateTime(2019, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "INV-004", new DateTime(2023, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), "ST-004", "TECH-004", 3, null }
                });

            migrationBuilder.InsertData(
                table: "TechnicalObjects",
                columns: new[] { "Id", "ComplexId", "InstallationDate", "Name", "ObjectType" },
                values: new object[,]
                {
                    { 4, 2, new DateTime(2021, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Резервуар-1", "Резервуар" },
                    { 5, 2, new DateTime(2021, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Резервуар-2", "Резервуар" },
                    { 6, 2, new DateTime(2019, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Насосная станция", "Насосная станция" }
                });

            migrationBuilder.InsertData(
                table: "Workshops",
                columns: new[] { "Id", "Code", "Name", "ResponsiblePersonId" },
                values: new object[,]
                {
                    { 2, "KIP-2", "Цех КИПиА", 3 },
                    { 3, "GP-3", "Главный производственный цех", 4 }
                });

            migrationBuilder.InsertData(
                table: "EquipmentInstances",
                columns: new[] { "Id", "CurrentStatus", "EquipmentTypeId", "FactoryNumber", "InstallationDate", "InventoryNumber", "LastMaintenanceDate", "NextMaintenanceDate", "StationNumber", "TechnicalNumber", "TechnicalObjectId", "TechnologicalUnitId" },
                values: new object[] { 7, "В работе", 3, "FAC-1007", new DateTime(2019, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "INV-007", new DateTime(2023, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), "ST-007", "TECH-007", 6, null });

            migrationBuilder.InsertData(
                table: "EquipmentParameters",
                columns: new[] { "Id", "AccuracyClass", "EquipmentInstanceId", "MaxValue", "MeasurementRange", "MinValue", "NormalValue", "ParameterName", "Unit" },
                values: new object[,]
                {
                    { 1, "0.5", 1, 100.0, "0-100 бар", 0.0, 50.0, "Давление", "бар" },
                    { 2, "1.0", 1, 80.0, "-20°C до +80°C", -20.0, 25.0, "Температура", "°C" },
                    { 3, "1.0", 2, 240.0, "220-240 В", 220.0, 230.0, "Напряжение питания", "В" },
                    { 4, "1.0", 2, 50.0, "0-50°C", 0.0, 25.0, "Температура окружающей среды", "°C" },
                    { 5, "0.5", 3, 5000.0, "0-5000 м³/ч", 0.0, 2500.0, "Расход газа", "м³/ч" },
                    { 6, "0.5", 3, 100.0, "0-100 бар", 0.0, 50.0, "Давление", "бар" }
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "AssigneeId", "CreatedAt", "CreatorId", "EquipmentInstanceId", "Priority", "Requirements", "Status", "TechnicalObjectId", "TechnicalSpecs", "TechnologicalUnitId", "UniqueNumber", "WorkType", "WorkshopId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 12, 8, 12, 15, 19, 19, DateTimeKind.Utc).AddTicks(4607), 5, 1, 2, "Требуется замена датчика давления МП-100 на ГРС-1", 0, 1, "Не работает датчик давления на ГРС-1. Необходима замена.", 1, "REQ-001", 2, 1 },
                    { 2, 3, new DateTime(2025, 12, 10, 12, 15, 19, 19, DateTimeKind.Utc).AddTicks(4622), 5, 2, 1, "Настройка параметров контроллера и проверка работы системы", 1, 1, "Необходима настройка контроллера автоматики AC800 на ГРС-1", 1, "REQ-002", 1, 1 },
                    { 3, 4, new DateTime(2025, 12, 5, 12, 15, 19, 19, DateTimeKind.Utc).AddTicks(4631), 5, 4, 0, "Монтаж и настройка датчика температуры YT-500", 2, 3, "Установка нового датчика температуры на газопроводе-10", null, "REQ-003", 0, 1 },
                    { 4, null, new DateTime(2025, 12, 12, 12, 15, 19, 19, DateTimeKind.Utc).AddTicks(4639), 5, 3, 3, "Срочный ремонт узла компрессора, замена уплотнений", 3, 2, "Аварийная утечка газа на компрессорной станции КС-5", 2, "REQ-004", 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "TechnologicalUnits",
                columns: new[] { "Id", "Code", "Description", "Name", "WorkshopId" },
                values: new object[,]
                {
                    { 3, "SKD-3", "Система контроля и регулирования давления газа", "Система контроля давления", 2 },
                    { 4, "SIM-4", "Система измерения расхода газа на выходе", "Система измерения расхода", 2 },
                    { 5, "RP-5", "Резервуары для хранения нефтепродуктов", "Резервуарный парк", 3 }
                });

            migrationBuilder.InsertData(
                table: "EquipmentInstances",
                columns: new[] { "Id", "CurrentStatus", "EquipmentTypeId", "FactoryNumber", "InstallationDate", "InventoryNumber", "LastMaintenanceDate", "NextMaintenanceDate", "StationNumber", "TechnicalNumber", "TechnicalObjectId", "TechnologicalUnitId" },
                values: new object[,]
                {
                    { 5, "В работе", 5, "FAC-1005", new DateTime(2021, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), "INV-005", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ST-005", "TECH-005", 4, 5 },
                    { 6, "В работе", 1, "FAC-1006", new DateTime(2021, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), "INV-006", new DateTime(2023, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), "ST-006", "TECH-006", 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "AssigneeId", "CreatedAt", "CreatorId", "EquipmentInstanceId", "Priority", "Requirements", "Status", "TechnicalObjectId", "TechnicalSpecs", "TechnologicalUnitId", "UniqueNumber", "WorkType", "WorkshopId" },
                values: new object[] { 5, 3, new DateTime(2025, 12, 14, 12, 15, 19, 19, DateTimeKind.Utc).AddTicks(4646), 5, 5, 2, "Калибровка прибора Rosemount 8600 и проверка показаний", 1, 4, "Настройка системы измерения расхода на резервуаре-1", 5, "REQ-005", 1, 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "EquipmentParameters",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EquipmentParameters",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EquipmentParameters",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EquipmentParameters",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EquipmentParameters",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EquipmentParameters",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EquipmentInstances",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Workshops",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Complexes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Workshops",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Complexes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Location", "Name", "Type" },
                values: new object[] { "Воткинск", "Комплекс 1", "Комплексная станция" });

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Specifications",
                value: "Диапазон 0-100 бар");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Specifications",
                value: "Управление КИПиА");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Model", "Specifications" },
                values: new object[] { "Rosemount", "Контроль расхода газа" });

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
                columns: new[] { "InstallationDate", "Name" },
                values: new object[] { new DateTime(2018, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Компрессорная станция-5" });

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Газопроводный участок-10");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$MWQxjtwB6PZ0opkJR3h4/Oi67p.otm.D/whrE44oJjlPQdsu9eaOG");

            migrationBuilder.UpdateData(
                table: "Workshops",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "Name", "ResponsiblePersonId" },
                values: new object[] { "C1", "Цех 1", 1 });
        }
    }
}
