using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class SegmentQueueItemNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "VideoSegmentQueue");

            migrationBuilder.DropColumn(
                name: "InputDirectory",
                table: "VideoSegmentQueue");

            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "VideoSegmentQueue");

            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "VideoSegmentQueue",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "VideoSegmentQueueItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoID = table.Column<int>(nullable: false),
                    VideoSegmentQueueId = table.Column<int>(nullable: false),
                    InputDirectory = table.Column<string>(unicode: false, nullable: false),
                    Height = table.Column<int>(nullable: false),
                    IsDone = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoSegmentQueueItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VideoSegmentQueueItem_Video",
                        column: x => x.VideoID,
                        principalTable: "Video",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoSegmentQueueItem_VideoSegmentQueue",
                        column: x => x.VideoSegmentQueueId,
                        principalTable: "VideoSegmentQueue",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoSegmentQueueItem_VideoID",
                table: "VideoSegmentQueueItem",
                column: "VideoID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoSegmentQueueItem_VideoSegmentQueueId",
                table: "VideoSegmentQueueItem",
                column: "VideoSegmentQueueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoSegmentQueueItem");

            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "VideoSegmentQueue");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "VideoSegmentQueue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InputDirectory",
                table: "VideoSegmentQueue",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "VideoSegmentQueue",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
