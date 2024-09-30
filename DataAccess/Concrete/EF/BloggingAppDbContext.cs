using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EF
{
    public class BloggingAppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=BloggingAppDb;Trusted_Connection=true;TrustServerCertificate=true");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFollower> UsersFollower { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperations { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<Comment> Comments { get; set; }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Post>().HasMany(p => p.)
        }
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OperationClaim>().HasData(
                new OperationClaim
                {
                    Id = 1,
                    Name = "User",
                },
                new OperationClaim
                {
                    Id = 2,
                    Name = "Admin"
                },
                new OperationClaim
                {
                    Id = 3,
                    Name = "Moderator"
                },
                new OperationClaim
                {
                    Id = 4,
                    Name = "post.delete"
                },
                new OperationClaim
                {
                    Id = 5,
                    Name = "comment.delete"
                }
            );
            modelBuilder.Entity<UserOperationClaim>().HasData(
                 new UserOperationClaim
                 {
                     Id = 1,
                     UserId = 1,
                     OperationClaimId = 2
                 },
                 new UserOperationClaim
                 {
                     Id = 2,
                     UserId = 1,
                     OperationClaimId = 3
                 },
                 new UserOperationClaim
                 {
                     Id = 3,
                     UserId = 1,
                     OperationClaimId = 4
                 },
                 new UserOperationClaim
                 {
                     Id = 4,
                     UserId = 1,
                     OperationClaimId = 5
                 }
            );
        }
    }
}
