using Microsoft.EntityFrameworkCore;

namespace PublicMessageStorytel.Models
{
    public class PublicMessageContext : DbContext
    {
        public PublicMessageContext(DbContextOptions<PublicMessageContext> options)
            : base(options)
        {
        }

        public DbSet<PublicMessage> PublicMessages { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}