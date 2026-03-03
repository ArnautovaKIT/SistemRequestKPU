using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemRequestKPU.Migrations
{
    /// <inheritdoc />
    public partial class Makeequirementsullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "Requests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "Requests",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 8, 12, 15, 19, 19, DateTimeKind.Utc).AddTicks(4607));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 10, 12, 15, 19, 19, DateTimeKind.Utc).AddTicks(4622));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 12, 15, 19, 19, DateTimeKind.Utc).AddTicks(4631));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 12, 12, 15, 19, 19, DateTimeKind.Utc).AddTicks(4639));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 14, 12, 15, 19, 19, DateTimeKind.Utc).AddTicks(4646));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$npWjEbaLSV5.pU5CNP4b.eEOsZFgm3mPhYiIswFpeX9B66mU7dSpu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$b/oJ9t4zFDXLD85hqyfNIO3BCJ5gXDRKZAsFGyNEGt4MkPPRfz.ci");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$oPLB3Ke53iq.2oxJhYHmXeGGZh2PoP6wv2RmK/NOSqirMkXdZ.tGO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$gmvr2aHbIRqpQpPVzzcBfO3jc0dYytzYpB0xoixVTMylrmk/LHasK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "$2a$11$FzqWnnS2U9NcXEFoGLADJOS0JnyIXAUefTcQp8sb5UmXMI9Oq3tK.");
        }
    }
}
