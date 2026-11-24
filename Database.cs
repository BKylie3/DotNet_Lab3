using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace ScholarshipManagementSystem_27109
{
    public static class Database
    {
        private static readonly string DbPath =
            Path.Combine(AppContext.BaseDirectory, "scholarship.db");

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(
                $"Data Source={DbPath};Foreign Keys=True");
        }

        public static void InitializeDatabase()
        {
            using var conn = GetConnection();
            conn.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS Students (
                    StudentId TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Age INTEGER NOT NULL CHECK(Age > 0),
                    Course TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Users (
                    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    Role TEXT NOT NULL
                        CHECK(Role IN ('Student', 'Admin')),
                    StudentId TEXT,
                    FOREIGN KEY(StudentId)
                        REFERENCES Students(StudentId)
                );

                CREATE TABLE IF NOT EXISTS Scholarships (
                    ScholarshipId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    Amount REAL NOT NULL,
                    Deadline TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Applications (
                    ApplicationId INTEGER PRIMARY KEY AUTOINCREMENT,
                    StudentId TEXT NOT NULL,
                    ScholarshipId INTEGER NOT NULL,
                    ApplicationDate TEXT NOT NULL,
                    Status TEXT NOT NULL DEFAULT 'Pending'
                        CHECK(Status IN ('Pending', 'Approved', 'Rejected')),
                    FOREIGN KEY(StudentId)
                        REFERENCES Students(StudentId),
                    FOREIGN KEY(ScholarshipId)
                        REFERENCES Scholarships(ScholarshipId),
                    UNIQUE(StudentId, ScholarshipId)
                );

                CREATE TABLE IF NOT EXISTS Documents (
                    DocumentId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ApplicationId INTEGER NOT NULL,
                    DocumentName TEXT NOT NULL,
                    DocumentData BLOB NOT NULL,
                    FOREIGN KEY(ApplicationId)
                        REFERENCES Applications(ApplicationId)
                        ON DELETE CASCADE
                );
            ";

            using var cmd = new SqliteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}