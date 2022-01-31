using Microsoft.EntityFrameworkCore;
using Queue.Models;

namespace Queue.Data; 
public class QueueDbContext : DbContext {
    public QueueDbContext (DbContextOptions<QueueDbContext> options) : base (options) { }
    public DbSet<QueueModel> queues { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<QueueModel>().HasIndex(i => i.Phone).IsUnique();
    }

}
