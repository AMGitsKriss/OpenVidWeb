using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class CascadeDeleteQueuedJobs3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoSegmentQueue_Video",
                table: "VideoSegmentQueue");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoSegmentQueue_Video",
                table: "VideoSegmentQueue",
                column: "VideoID",
                principalTable: "Video",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoSegmentQueue_Video",
                table: "VideoSegmentQueue");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoSegmentQueue_Video",
                table: "VideoSegmentQueue",
                column: "VideoID",
                principalTable: "Video",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
