using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class CascadeDeleteQueuedJobs1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoSegmentQueue_Video",
                table: "VideoSegmentQueue");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoSegmentQueueItem_Video",
                table: "VideoSegmentQueueItem");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoSegmentQueue_Video",
                table: "VideoSegmentQueue",
                column: "VideoID",
                principalTable: "Video",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoSegmentQueueItem_Video",
                table: "VideoSegmentQueueItem",
                column: "VideoID",
                principalTable: "Video",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoSegmentQueue_Video",
                table: "VideoSegmentQueue");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoSegmentQueueItem_Video",
                table: "VideoSegmentQueueItem");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoSegmentQueue_Video",
                table: "VideoSegmentQueue",
                column: "VideoID",
                principalTable: "Video",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoSegmentQueueItem_Video",
                table: "VideoSegmentQueueItem",
                column: "VideoID",
                principalTable: "Video",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
