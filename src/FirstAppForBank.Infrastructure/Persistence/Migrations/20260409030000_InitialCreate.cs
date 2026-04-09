using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAppForBank.Infrastructure.Persistence.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "categories",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_categories", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "services",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                Criticality = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                ServiceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                CurrentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Owner = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                RunbookUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                DashboardUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false),
                last_status_changed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_services", x => x.Id);
                table.ForeignKey(
                    name: "FK_services_categories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_services_CategoryId",
            table: "services",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_services_Name",
            table: "services",
            column: "Name",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "services");

        migrationBuilder.DropTable(
            name: "categories");
    }
}
