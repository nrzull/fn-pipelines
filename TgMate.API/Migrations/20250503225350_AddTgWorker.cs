using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgMate.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTgWorker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_telegram_pipeline_units_telegram_pipeline_units_input_holde",
                table: "telegram_pipeline_units");

            migrationBuilder.DropForeignKey(
                name: "fk_telegram_pipeline_units_telegram_pipeline_units_parent_id",
                table: "telegram_pipeline_units");

            migrationBuilder.DropForeignKey(
                name: "fk_telegram_pipeline_units_telegram_pipelines_pipeline_id",
                table: "telegram_pipeline_units");

            migrationBuilder.DropPrimaryKey(
                name: "pk_telegram_pipelines",
                table: "telegram_pipelines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_telegram_pipeline_units",
                table: "telegram_pipeline_units");

            migrationBuilder.RenameTable(
                name: "telegram_pipelines",
                newName: "tg_pipelines");

            migrationBuilder.RenameTable(
                name: "telegram_pipeline_units",
                newName: "tg_pipeline_units");

            migrationBuilder.RenameIndex(
                name: "ix_telegram_pipeline_units_pipeline_id",
                table: "tg_pipeline_units",
                newName: "ix_tg_pipeline_units_pipeline_id");

            migrationBuilder.RenameIndex(
                name: "ix_telegram_pipeline_units_parent_id",
                table: "tg_pipeline_units",
                newName: "ix_tg_pipeline_units_parent_id");

            migrationBuilder.RenameIndex(
                name: "ix_telegram_pipeline_units_input_holder_id",
                table: "tg_pipeline_units",
                newName: "ix_tg_pipeline_units_input_holder_id");

            migrationBuilder.AddColumn<Guid>(
                name: "tg_worker_id",
                table: "tg_pipelines",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "pk_tg_pipelines",
                table: "tg_pipelines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_tg_pipeline_units",
                table: "tg_pipeline_units",
                column: "id");

            migrationBuilder.CreateTable(
                name: "tg_workers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tg_workers", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tg_pipelines_tg_worker_id",
                table: "tg_pipelines",
                column: "tg_worker_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tg_pipeline_units_tg_pipeline_units_input_holder_id",
                table: "tg_pipeline_units",
                column: "input_holder_id",
                principalTable: "tg_pipeline_units",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_tg_pipeline_units_tg_pipeline_units_parent_id",
                table: "tg_pipeline_units",
                column: "parent_id",
                principalTable: "tg_pipeline_units",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_tg_pipeline_units_tg_pipelines_pipeline_id",
                table: "tg_pipeline_units",
                column: "pipeline_id",
                principalTable: "tg_pipelines",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tg_pipelines_tg_workers_tg_worker_id",
                table: "tg_pipelines",
                column: "tg_worker_id",
                principalTable: "tg_workers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tg_pipeline_units_tg_pipeline_units_input_holder_id",
                table: "tg_pipeline_units");

            migrationBuilder.DropForeignKey(
                name: "fk_tg_pipeline_units_tg_pipeline_units_parent_id",
                table: "tg_pipeline_units");

            migrationBuilder.DropForeignKey(
                name: "fk_tg_pipeline_units_tg_pipelines_pipeline_id",
                table: "tg_pipeline_units");

            migrationBuilder.DropForeignKey(
                name: "fk_tg_pipelines_tg_workers_tg_worker_id",
                table: "tg_pipelines");

            migrationBuilder.DropTable(
                name: "tg_workers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_tg_pipelines",
                table: "tg_pipelines");

            migrationBuilder.DropIndex(
                name: "ix_tg_pipelines_tg_worker_id",
                table: "tg_pipelines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_tg_pipeline_units",
                table: "tg_pipeline_units");

            migrationBuilder.DropColumn(
                name: "tg_worker_id",
                table: "tg_pipelines");

            migrationBuilder.RenameTable(
                name: "tg_pipelines",
                newName: "telegram_pipelines");

            migrationBuilder.RenameTable(
                name: "tg_pipeline_units",
                newName: "telegram_pipeline_units");

            migrationBuilder.RenameIndex(
                name: "ix_tg_pipeline_units_pipeline_id",
                table: "telegram_pipeline_units",
                newName: "ix_telegram_pipeline_units_pipeline_id");

            migrationBuilder.RenameIndex(
                name: "ix_tg_pipeline_units_parent_id",
                table: "telegram_pipeline_units",
                newName: "ix_telegram_pipeline_units_parent_id");

            migrationBuilder.RenameIndex(
                name: "ix_tg_pipeline_units_input_holder_id",
                table: "telegram_pipeline_units",
                newName: "ix_telegram_pipeline_units_input_holder_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_telegram_pipelines",
                table: "telegram_pipelines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_telegram_pipeline_units",
                table: "telegram_pipeline_units",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_telegram_pipeline_units_telegram_pipeline_units_input_holde",
                table: "telegram_pipeline_units",
                column: "input_holder_id",
                principalTable: "telegram_pipeline_units",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_telegram_pipeline_units_telegram_pipeline_units_parent_id",
                table: "telegram_pipeline_units",
                column: "parent_id",
                principalTable: "telegram_pipeline_units",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_telegram_pipeline_units_telegram_pipelines_pipeline_id",
                table: "telegram_pipeline_units",
                column: "pipeline_id",
                principalTable: "telegram_pipelines",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
