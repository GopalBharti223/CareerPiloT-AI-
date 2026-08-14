using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPilot_AI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResumeAnalyses",
                columns: table => new
                {
                    ResumeAnalysisId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    ATSScore = table.Column<int>(type: "int", nullable: false),
                    MissingSkills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weakness = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Suggestion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnalyzedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeAnalyses", x => x.ResumeAnalysisId);
                });

            migrationBuilder.CreateTable(
                name: "Resumes",
                columns: table => new
                {
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.ResumeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResumeAnalyses");

            migrationBuilder.DropTable(
                name: "Resumes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
