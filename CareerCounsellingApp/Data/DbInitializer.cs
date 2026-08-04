using CareerCounsellingApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Data;

    public static class DbInitializer
    {
    public static void Seed(AppDbContext db)
    {
      

        if (!db.Users.Any())
        {
            db.Users.Add(new User
            {
                Username = "admin",
                Password = "admin123",
                Role = "Admin"
            });

            db.SaveChanges();
        }
    }

    private static void EnsureQuestionImageStorage(AppDbContext db)
    {
        db.Database.ExecuteSqlRaw(@"
CREATE TABLE IF NOT EXISTS QuestionImages (
    Id INTEGER NOT NULL CONSTRAINT PK_QuestionImages PRIMARY KEY AUTOINCREMENT,
    QuestionId INTEGER NOT NULL,
    ImageData BLOB NULL,
    CONSTRAINT FK_QuestionImages_Questions_QuestionId FOREIGN KEY (QuestionId) REFERENCES Questions (Id) ON DELETE CASCADE
);");

        db.Database.ExecuteSqlRaw(@"
CREATE UNIQUE INDEX IF NOT EXISTS IX_QuestionImages_QuestionId
ON QuestionImages (QuestionId);");

        if (ColumnExists(db, "Questions", "ImageData"))
        {
            db.Database.ExecuteSqlRaw(@"
INSERT OR IGNORE INTO QuestionImages (QuestionId, ImageData)
SELECT Id, ImageData
FROM Questions
WHERE ImageData IS NOT NULL;");
        }
    }

    private static bool ColumnExists(AppDbContext db, string tableName, string columnName)
    {
        using var connection = db.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();
        command.CommandText = $"PRAGMA table_info({tableName})";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            if (string.Equals(reader[1]?.ToString(), columnName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}

