using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class SourceNameNotUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VideoSource_Unique",
                table: "VideoSource");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VideoSource_Unique",
                table: "VideoSource",
                column: "MD5",
                unique: true);
        }
    }
}
