using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class NullableLanguageFieldOnSegmentItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArgLanguage",
                table: "VideoSegmentQueueItem",
                unicode: false,
                maxLength: 32,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArgLanguage",
                table: "VideoSegmentQueueItem");
        }
    }
}
