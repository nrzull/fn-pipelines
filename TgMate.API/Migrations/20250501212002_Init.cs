using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgMate.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "telegram_pipelines",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    scheduled_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    cancelled_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_telegram_pipelines", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "telegram_pipeline_units",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    fn = table.Column<string>(type: "text", nullable: false),
                    input = table.Column<string>(type: "text", nullable: false),
                    output = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    input_holder_id = table.Column<Guid>(type: "uuid", nullable: true),
                    pipeline_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_telegram_pipeline_units", x => x.id);
                    table.ForeignKey(
                        name: "fk_telegram_pipeline_units_telegram_pipeline_units_input_holde",
                        column: x => x.input_holder_id,
                        principalTable: "telegram_pipeline_units",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_telegram_pipeline_units_telegram_pipeline_units_parent_id",
                        column: x => x.parent_id,
                        principalTable: "telegram_pipeline_units",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_telegram_pipeline_units_telegram_pipelines_pipeline_id",
                        column: x => x.pipeline_id,
                        principalTable: "telegram_pipelines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_telegram_pipeline_units_input_holder_id",
                table: "telegram_pipeline_units",
                column: "input_holder_id");

            migrationBuilder.CreateIndex(
                name: "ix_telegram_pipeline_units_parent_id",
                table: "telegram_pipeline_units",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_telegram_pipeline_units_pipeline_id",
                table: "telegram_pipeline_units",
                column: "pipeline_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "telegram_pipeline_units");

            migrationBuilder.DropTable(
                name: "telegram_pipelines");
        }
    }
}
