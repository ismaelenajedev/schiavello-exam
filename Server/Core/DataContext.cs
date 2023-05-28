using Microsoft.EntityFrameworkCore;

namespace Server.Core;

public class DataContext : DbContext
{
    private readonly string DbPath;
    private readonly IConfiguration _config;

    public DataContext(IConfiguration config)
    {
       _config = config;
       
       DbPath = _config.GetConnectionString("Default");
    }

    public DbSet<Todo> Todos { get; set; }
    public DbSet<Photo> Photos  { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
}