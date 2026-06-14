using klk2.Models;
using Microsoft.EntityFrameworkCore;    // da ne bih pisala upite rucno

namespace klk2.Data;

public class AvioContext : DbContext
{
    // dvije tabele
    public DbSet<Aviokompanija> Aviokompanije { get; set; }
    public DbSet<Let> Letovi { get; set; }

    // veza sa bazom
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=aviokompanija.db");
    }
}
