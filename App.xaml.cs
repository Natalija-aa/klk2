using System.Windows;
using System.Windows.Media;
using klk2.Data;    // AvioContext - most ka bayi
using klk2.Models;  // Let i Aviokompanija
using MaterialDesignThemes.Wpf;

namespace klk2;

public partial class App : Application
{
    // pokrece se prije prozora
    protected void OnStartup(object sender, StartupEventArgs e)
    {
        // boje
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        theme.SetPrimaryColor(Color.FromRgb(0x1A, 0x23, 0x7E));
        theme.SetSecondaryColor(Color.FromRgb(0x15, 0x65, 0xC0));
        paletteHelper.SetTheme(theme);

        using var context = new AvioContext();  // sama se zatvori kad se zavrsi metoda
        context.Database.EnsureCreated();   // ako ne postoji napravi bazu


        if (!context.Aviokompanije.Any())   // ako je baza prazna postavi pocetne podatke
        {
            var air = new Aviokompanija { Naziv = "Air Serbia",       Drzava = "Srbija"  };
            var luf = new Aviokompanija { Naziv = "Lufthansa",        Drzava = "Nemacka" };
            var thy = new Aviokompanija { Naziv = "Turkish Airlines", Drzava = "Turska"  };
            var qat = new Aviokompanija { Naziv = "Qatar Airways",    Drzava = "Katar"   };
            var emi = new Aviokompanija { Naziv = "Emirates",         Drzava = "UAE"     };
            context.Aviokompanije.AddRange(air, luf, thy, qat, emi);
            context.Letovi.AddRange(
                new Let { BrojLeta = "JU 410",  Relacija = "Beograd - Pariz",   DatumVreme = DateTime.Now.AddDays(7),  CenaKarte = 210m, Aviokompanija = air },
                new Let { BrojLeta = "LH 1411", Relacija = "Frankfurt - Beograd",DatumVreme = DateTime.Now.AddDays(3),  CenaKarte = 175m, Aviokompanija = luf },
                new Let { BrojLeta = "TK 1082", Relacija = "Istanbul - Beograd", DatumVreme = DateTime.Now.AddDays(2),  CenaKarte = 130m, Aviokompanija = thy },
                new Let { BrojLeta = "QR 231",  Relacija = "Doha - Beograd",     DatumVreme = DateTime.Now.AddDays(10), CenaKarte = 480m, Aviokompanija = qat },
                new Let { BrojLeta = "EK 209",  Relacija = "Dubai - Beograd",    DatumVreme = DateTime.Now.AddDays(5),  CenaKarte = 520m, Aviokompanija = emi }
            );
            context.SaveChanges();
        }

        var mainWindow = new MainWindow();  // glavni prozor
        mainWindow.Show();
    }
}
