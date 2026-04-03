using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDDTemplate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdministrativeModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "modules",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "icon", "label" },
                values: new object[] { "pi pi-building", "Administrativo" });

            migrationBuilder.InsertData(
                table: "modules",
                columns: new[] { "id", "icon", "label", "parent_id" },
                values: new object[] { 2, "pi pi-users", "Usuários", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "modules",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "modules",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "icon", "label" },
                values: new object[] { null, "Usuários" });
        }
    }
}
