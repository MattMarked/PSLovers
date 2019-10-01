using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PSLovers2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<FavoriteForUser> FavoriteForUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
