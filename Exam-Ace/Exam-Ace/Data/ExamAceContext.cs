using ExamAce.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExamAce.Data
{
    public class ExamAceContext : IdentityDbContext<User>
    {
        public ExamAceContext(DbContextOptions<ExamAceContext> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public void InsertNew(RefreshToken token)
        {
            var tokenModel = RefreshTokens.SingleOrDefault(i => i.UserId == token.UserId);
            if (tokenModel != null)
            {
                RefreshTokens.Remove(tokenModel);
                SaveChanges();
            }
            RefreshTokens.Add(token);
            SaveChanges();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>()
                .HasAlternateKey(c => c.UserId)
                .HasName("refreshToken_UserId");
            modelBuilder.Entity<RefreshToken>()
                .HasAlternateKey(c => c.Token)
                .HasName("refreshToken_Token");

            //modelBuilder.Entity<CourseTeacher>().HasKey(ct => new { ct.CourseId, ct.TeacherId });
            //modelBuilder.Entity<CourseTeacher>()
            //    .HasOne(ct => ct.Course)
            //    .WithMany(c => c.CourseTeacher)
            //    .HasForeignKey(ct => ct.CourseId);

            //modelBuilder.Entity<CourseTeacher>()
            //    .HasOne(ct => ct.Teacher)
            //    .WithMany(c => c.CourseTeacher)
            //    .HasForeignKey(ct => ct.TeacherId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
