using CareerCounsellingApp.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCounsellingApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260803131500_QuestionImageTable")]
    public partial class QuestionImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS QuestionImages (
    Id INTEGER NOT NULL CONSTRAINT PK_QuestionImages PRIMARY KEY AUTOINCREMENT,
    QuestionId INTEGER NOT NULL,
    ImageData BLOB NULL,
    CONSTRAINT FK_QuestionImages_Questions_QuestionId FOREIGN KEY (QuestionId) REFERENCES Questions (Id) ON DELETE CASCADE
);");

            migrationBuilder.Sql(@"
CREATE UNIQUE INDEX IF NOT EXISTS IX_QuestionImages_QuestionId
ON QuestionImages (QuestionId);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS QuestionImages;");
        }
    }
}
