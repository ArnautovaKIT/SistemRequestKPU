using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemRequestKPU.Migrations
{
    /// <inheritdoc />
    public partial class newmodelsrequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TechnologicalUnitId",
                table: "Requests",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkshopId",
                table: "Requests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$MWQxjtwB6PZ0opkJR3h4/Oi67p.otm.D/whrE44oJjlPQdsu9eaOG");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_TechnologicalUnitId",
                table: "Requests",
                column: "TechnologicalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_WorkshopId",
                table: "Requests",
                column: "WorkshopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_TechnologicalUnits_TechnologicalUnitId",
                table: "Requests",
                column: "TechnologicalUnitId",
                principalTable: "TechnologicalUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Workshops_WorkshopId",
                table: "Requests",
                column: "WorkshopId",
                principalTable: "Workshops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_TechnologicalUnits_TechnologicalUnitId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Workshops_WorkshopId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_TechnologicalUnitId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_WorkshopId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "TechnologicalUnitId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "Requests");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$3nDndA8e0dLSpxZcZ9iXJu0jE4cYl1B1yk95zUjEbVVRNwQy7UB5O");
        }
    }
}
