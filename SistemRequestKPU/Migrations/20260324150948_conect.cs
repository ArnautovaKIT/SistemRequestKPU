using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemRequestKPU.Migrations
{
    /// <inheritdoc />
    public partial class conect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TechnicalObjectId",
                table: "TechnologicalUnits",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 1,
                column: "TechnicalObjectId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 2,
                column: "TechnicalObjectId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 3,
                column: "TechnicalObjectId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TechnologicalUnits",
                keyColumn: "Id",
                keyValue: 4,
                column: "TechnicalObjectId",
                value: 2);

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$cjdnnXGom3XXZpY7c2SRrOxs1G6GpW1N1UiM2mCQDWPeFgDTSHb82");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$/34lK4o2aWth4tGo2KGElurDdrmj6sIbAtoXPNVKFRYKscuFRjoPm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$f5nbsKuxpbbiM2Irp7psoe8.pvL1.9156FNJSrlOsep3v1iLlm.y2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$h1GverJ.w5pCLv5tQIp8GekGrS2F957HT7bSkpQjmn5/OWX7pPXEK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "$2a$11$OE/VtaNlWxDLH8L89SWqQ.AsTgAi73q5pig47ta8zxIANWVAj8QKW");
        }
    }
}
