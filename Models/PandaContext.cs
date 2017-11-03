using Microsoft.EntityFrameworkCore;
 
namespace trashpanda.Models
{
    public class PandaContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public PandaContext(DbContextOptions<PandaContext> options) : base(options) { }
        public DbSet<User> users {get;set;}
        public DbSet<Post> posts {get;set;}
        public DbSet<Like> likes {get;set;}


    }
}