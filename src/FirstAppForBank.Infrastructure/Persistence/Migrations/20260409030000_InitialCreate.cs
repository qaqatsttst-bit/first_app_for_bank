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

        migrationBuilder.CreateTable(
            name: "service_comments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ServiceId = table.Column<Guid>(type: "uuid", nullable: false),
                AuthorName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                CommentText = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_service_comments", x => x.Id);
                table.ForeignKey(
                    name: "FK_service_comments_services_ServiceId",
                    column: x => x.ServiceId,
                    principalTable: "services",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "service_status_history",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ServiceId = table.Column<Guid>(type: "uuid", nullable: false),
                OldStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                NewStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                ChangeSource = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                ChangeSourceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                changed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_service_status_history", x => x.Id);
                table.ForeignKey(
                    name: "FK_service_status_history_services_ServiceId",
                    column: x => x.ServiceId,
                    principalTable: "services",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_service_comments_ServiceId",
            table: "service_comments",
            column: "ServiceId");

        migrationBuilder.CreateIndex(
            name: "IX_services_CategoryId",
            table: "services",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_services_Name",
            table: "services",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_service_status_history_ServiceId",
            table: "service_status_history",
            column: "ServiceId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "service_comments");

        migrationBuilder.DropTable(
            name: "service_status_history");

        migrationBuilder.DropTable(
            name: "services");

        migrationBuilder.DropTable(
            name: "categories");
    }
}
