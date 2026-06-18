using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeveloperRegistry.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "owners",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_owners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "api_keys",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    application_id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    key_hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_keys", x => x.id);
                    table.ForeignKey(
                        name: "FK_api_keys_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "webhooks",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    application_id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    event_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_webhooks", x => x.id);
                    table.ForeignKey(
                        name: "FK_webhooks_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_owners",
                columns: table => new
                {
                    application_id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    owner_id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    added_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_owners", x => new { x.application_id, x.owner_id });
                    table.ForeignKey(
                        name: "FK_application_owners_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_application_owners_owners_owner_id",
                        column: x => x.owner_id,
                        principalTable: "owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_api_keys_application_id_status",
                table: "api_keys",
                columns: new[] { "application_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_api_keys_key_hash",
                table: "api_keys",
                column: "key_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_application_owners_owner_id",
                table: "application_owners",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_applications_is_archived",
                table: "applications",
                column: "is_archived");

            migrationBuilder.CreateIndex(
                name: "IX_applications_name",
                table: "applications",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_owners_email",
                table: "owners",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_webhooks_application_id_event_name",
                table: "webhooks",
                columns: new[] { "application_id", "event_name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "api_keys");

            migrationBuilder.DropTable(
                name: "application_owners");

            migrationBuilder.DropTable(
                name: "webhooks");

            migrationBuilder.DropTable(
                name: "owners");

            migrationBuilder.DropTable(
                name: "applications");
        }
    }
}
