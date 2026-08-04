using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCounsellingApp.Migrations
{
    public partial class AddQuestionImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Questions",
                type: "BLOB",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Questions");
        }
    }
}
