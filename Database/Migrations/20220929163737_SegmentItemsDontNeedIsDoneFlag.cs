using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class SegmentItemsDontNeedIsDoneFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "VideoSegmentQueueItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "VideoSegmentQueueItem",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
