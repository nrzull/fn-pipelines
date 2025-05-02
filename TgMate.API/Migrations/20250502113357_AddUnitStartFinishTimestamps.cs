using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgMate.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitStartFinishTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cancelled_at",
                table: "telegram_pipelines");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "finished_at",
                table: "telegram_pipeline_units",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "started_at",
                table: "telegram_pipeline_units",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "finished_at",
                table: "telegram_pipeline_units");

            migrationBuilder.DropColumn(
                name: "started_at",
                table: "telegram_pipeline_units");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "cancelled_at",
                table: "telegram_pipelines",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
