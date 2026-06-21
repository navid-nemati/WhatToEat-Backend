using Authentication_Practice.Models.Account;
using Authentication_Practice.Models.Food;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Practice.Data
{
    public class AppDbContext : IdentityDbContext<AppUser> //IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Food>()
            //    .HasMany(i => i.Ingredients)
            //    .WithOne(f => f.Food)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IngredientsOfFood>()
                .HasOne(f => f.Food)
                .WithMany(f => f.IngredientsOfFood)
                .HasForeignKey(f => f.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IngredientsOfFood>()
                .HasOne(i => i.Ingredient)
                .WithMany(i => i.IngredientsOfFood)
                .HasForeignKey(i => i.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Category>()
            //    .HasMany(f => f.Foods)
            //    .WithOne(c => c.Category)
            //    .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientsOfFood> IngredientsOfFoods { get; set; }
    }
}
