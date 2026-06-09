using System.Windows;
using System.Windows.Media;
using klk2.Data;
using klk2.Models;
using MaterialDesignThemes.Wpf;

namespace klk2;

public partial class App : Application
{
    protected void OnStartup(object sender, StartupEventArgs e)
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        theme.SetPrimaryColor(Color.FromRgb(0x1A, 0x23, 0x7E));
        theme.SetSecondaryColor(Color.FromRgb(0x15, 0x65, 0xC0));
        paletteHelper.SetTheme(theme);

        using var context = new AvioContext();
        context.Database.EnsureCreated();

        if (!context.Aviokompanije.Any())
        {
            var air = new Aviokompanija { Naziv = "Air Serbia", Drzava = "Srbija" };
            var luf = new Aviokompanija { Naziv = "Lufthansa", Drzava = "Nemacka" };
            context.Aviokompanije.AddRange(air, luf);
            context.Letovi.AddRange(
                new Let { BrojLeta = "JU 101", Relacija = "Beograd - London", DatumVreme = DateTime.Now.AddDays(5), CenaKarte = 250m, Aviokompanija = air },
                new Let { BrojLeta = "LH 402", Relacija = "Frankfurt - Beograd", DatumVreme = DateTime.Now.AddDays(3), CenaKarte = 180m, Aviokompanija = luf }
            );
            context.SaveChanges();
        }

        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
}
