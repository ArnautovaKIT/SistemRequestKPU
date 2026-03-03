using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemRequestKPU.Migrations
{
    /// <inheritdoc />
    public partial class newmegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TechnicalObjectId",
                table: "TechnologicalUnits",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGRS",
                table: "TechnicalObjects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 24, 16, 7, 12, 3, DateTimeKind.Utc).AddTicks(2258));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 26, 16, 7, 12, 3, DateTimeKind.Utc).AddTicks(2272));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 21, 16, 7, 12, 3, DateTimeKind.Utc).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 28, 16, 7, 12, 3, DateTimeKind.Utc).AddTicks(2296));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 2, 16, 7, 12, 3, DateTimeKind.Utc).AddTicks(2303));

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsGRS",
                value: false);

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsGRS",
                value: false);

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsGRS",
                value: false);

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsGRS",
                value: false);

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsGRS",
                value: false);

            migrationBuilder.UpdateData(
                table: "TechnicalObjects",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsGRS",
                value: false);

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 1,
                column: "TechnicalObjectId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 2,
                column: "TechnicalObjectId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 3,
                column: "TechnicalObjectId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 4,
                column: "TechnicalObjectId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 5,
                column: "TechnicalObjectId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$TT8Irz7jiJqGtsCZBXMT0OYZEC6vA7yTBAb9Ry9v214ledu5Luc7u");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$w1rics.oxjferaZ8D7p3IujixYVD023RfDLY9//7SjO18V.Tv4zuS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$BEc7JLAkuoxU97iziaUF0.9V15G3OPDtJQw1uL2ym0Vjwzu5er5Xq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$sYbke8EwvzFigJayhyaR4eVMa3rUIoY.Pbe8NZrbZ8seTwub3HYrW");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "$2a$11$z8rqZ9H7i5us9HgeIS.aqeZZaHNtg6IL7pyy0x98I6BBpHuFjf6Fq");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalUnits_TechnicalObjectId",
                table: "TechnologicalUnits",
                column: "TechnicalObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalUnits_TechnicalObjects_TechnicalObjectId",
                table: "TechnologicalUnits",
                column: "TechnicalObjectId",
                principalTable: "TechnicalObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnologicalUnits_TechnicalObjects_TechnicalObjectId",
                table: "TechnologicalUnits");

            migrationBuilder.DropIndex(
                name: "IX_TechnologicalUnits_TechnicalObjectId",
                table: "TechnologicalUnits");

            migrationBuilder.DropColumn(
                name: "TechnicalObjectId",
                table: "TechnologicalUnits");

            migrationBuilder.DropColumn(
                name: "IsGRS",
                table: "TechnicalObjects");

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 8, 12, 50, 16, 530, DateTimeKind.Utc).AddTicks(232));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 10, 12, 50, 16, 530, DateTimeKind.Utc).AddTicks(258));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 12, 50, 16, 530, DateTimeKind.Utc).AddTicks(266));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 12, 12, 50, 16, 530, DateTimeKind.Utc).AddTicks(273));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 14, 12, 50, 16, 530, DateTimeKind.Utc).AddTicks(280));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$IeiXvnAAV95dW0/zfVOhE.Txf.O1uL4vtDu/YjiyoLe/yHPz9sccG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$zCnqHpyaEj0vbRlrE.W4ie2DGIth0EfTfm4MFzf0P2j9alUYEKiV6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$IBopgt/.0k6e4XxgLyvvUOvaLM7VynKsd6aTiBayVxNZdfencfrzO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$rLIDauEecpiBhmoT4TSPaeGNO8uJc9nVjSNA1d90xfNNogT6U8rjS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "$2a$11$62UdAYk9yHx2yE9RBJu3luKnejBYU6QOTn01NVrqY8WuuyHHFLRJG");
        }
    }
}
