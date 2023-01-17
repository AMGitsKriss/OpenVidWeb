using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class SegmentTableToHaveShakaArgsIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "VideoSegmentQueueItem");

            migrationBuilder.DropColumn(
                name: "InputDirectory",
                table: "VideoSegmentQueueItem");

            migrationBuilder.AddColumn<string>(
                name: "ArgInputFile",
                table: "VideoSegmentQueueItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArgInputFolder",
                table: "VideoSegmentQueueItem",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArgStream",
                table: "VideoSegmentQueueItem",
                unicode: false,
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArgStreamFolder",
                table: "VideoSegmentQueueItem",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "VideoSegmentQueue",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArgInputFile",
                table: "VideoSegmentQueueItem");

            migrationBuilder.DropColumn(
                name: "ArgInputFolder",
                table: "VideoSegmentQueueItem");

            migrationBuilder.DropColumn(
                name: "ArgStream",
                table: "VideoSegmentQueueItem");

            migrationBuilder.DropColumn(
                name: "ArgStreamFolder",
                table: "VideoSegmentQueueItem");

            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "VideoSegmentQueue");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "VideoSegmentQueueItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InputDirectory",
                table: "VideoSegmentQueueItem",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                defaultValue: "");
        }
    }
}
