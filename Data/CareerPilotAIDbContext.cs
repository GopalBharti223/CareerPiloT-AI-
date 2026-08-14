    using CareerPilot_AI.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Data;

    namespace CareerPilot_AI.Data
    {
        public class CareerPilotAIDbContext : DbContext
        {
     
           public CareerPilotAIDbContext(DbContextOptions<CareerPilotAIDbContext> options) : base(options)
            {

            }

            public DbSet<ResumeAnalysis> ResumeAnalyses { get; set; }
            public DbSet<Resume> Resumes { get; set; }

            public DbSet<User> Users { get; set; }

            public DbSet<PasswordResetOTP> PasswordResetOTPs { get; set; }

        


        }
    }
