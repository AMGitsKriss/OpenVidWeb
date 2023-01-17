using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class CascadeDeleteQueuedJobs2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoSegmentQueueItem_Video",
                table: "VideoSegmentQueueItem");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoSegmentQueueItem_Video",
                table: "VideoSegmentQueueItem",
                column: "VideoID",
                principalTable: "Video",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoSegmentQueueItem_Video",
                table: "VideoSegmentQueueItem");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoSegmentQueueItem_Video",
                table: "VideoSegmentQueueItem",
                column: "VideoID",
                principalTable: "Video",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
