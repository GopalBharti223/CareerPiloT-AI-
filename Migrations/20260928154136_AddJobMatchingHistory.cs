using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPilot_AI.Migrations
{
    /// <inheritdoc />
    public partial class AddJobMatchingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobMatchingHistories",
                columns: table => new
                {
                    JobMatchingHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchScore = table.Column<int>(type: "int", nullable: false),
                    MatchedSkills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MissingSkills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchedKeywords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MissingKeywords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienceMatch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Suggestions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobMatchingHistories", x => x.JobMatchingHistoryId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobMatchingHistories");
        }
    }
}
