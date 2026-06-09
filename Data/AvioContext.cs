using klk2.Models;
using Microsoft.EntityFrameworkCore;

namespace klk2.Data;

public class AvioContext : DbContext
{
    public DbSet<Aviokompanija> Aviokompanije { get; set; }
    public DbSet<Let> Letovi { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=aviokompanija.db");
    }
}
