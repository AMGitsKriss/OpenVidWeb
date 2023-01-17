using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class StreamIdForSegmentITems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArgStreamId",
                table: "VideoSegmentQueueItem",
                unicode: false,
                maxLength: 32,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArgStreamId",
                table: "VideoSegmentQueueItem");
        }
    }
}
