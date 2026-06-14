using System.Collections.ObjectModel;
using System.Windows;
using klk2.Data;
using klk2.Models;
using Microsoft.EntityFrameworkCore;

namespace klk2;

public partial class MainWindow : Window    // nasljedjuje WPF prozor
{
    public ObservableCollection<Let> Letovi { get; set; } = new();  // osvjezavanje u realnm vremenu

    // konstruktor - da pravi novi objekat kada se poyove new
    public MainWindow()
    {
        InitializeComponent();  // izgled XAML
        dgLetovi.ItemsSource = Letovi;  // tabela i kolekcija su povezane
        UcitajLetove(); // da se prikaze
    }

    private void UcitajLetove()
    {
        try
        {
            using var ctx = new AvioContext();
            var letovi = ctx.Letovi.Include(l => l.Aviokompanija).ToList(); // letovi iz baze
            Letovi.Clear();
            foreach (var let in letovi) // prolazak kroz sve letove
                Letovi.Add(let);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Greška pri učitavanju podataka:\n{ex.Message}", "Greška",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // dodavanje novog leta
    // event handler
    private void btnDodaj_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new LetDialog { Owner = this };    // novi prozor centriran 
        
        // korisnik pritisnuo sacuvaj i da li je sacuvan ID leta
        if (dialog.ShowDialog() == true && dialog.SavedLetId.HasValue)
        {
            try
            {
                using var ctx = new AvioContext();  // otvori bazu
                var noviLet = ctx.Letovi.Include(l => l.Aviokompanija)  
                    .First(l => l.Id == dialog.SavedLetId.Value);   // let iz baze sa odgovarajucim IDom
                Letovi.Add(noviLet);    // dodaj novi let
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri osvežavanju liste:\n{ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Izmjeni
    private void btnIzmeni_Click(object sender, RoutedEventArgs e)
    {
        // koji red je selektovan
        if (dgLetovi.SelectedItem is not Let selektovani)
        {
            MessageBox.Show("Molimo izaberite let za izmenu.", "Upozorenje",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        //  otvara formu odovarajuceg ID
        var dialog = new LetDialog(selektovani.Id) { Owner = this };
        if (dialog.ShowDialog() == true)
        {
            try
            {
                using var ctx = new AvioContext();
                var azurirani = ctx.Letovi.Include(l => l.Aviokompanija)
                    .First(l => l.Id == selektovani.Id);

                var item = Letovi.FirstOrDefault(l => l.Id == selektovani.Id);
                // zamjeni sa novim vrijednostima
                if (item != null)
                {
                    item.BrojLeta = azurirani.BrojLeta;
                    item.Relacija = azurirani.Relacija;
                    item.DatumVreme = azurirani.DatumVreme;
                    item.CenaKarte = azurirani.CenaKarte;
                    item.AviokompanijaId = azurirani.AviokompanijaId;
                    item.Aviokompanija = azurirani.Aviokompanija;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri osvežavanju podataka:\n{ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // brisanje selektovanog leta
    private void btnObrisi_Click(object sender, RoutedEventArgs e)
    {
        if (dgLetovi.SelectedItem is not Let selektovani)
        {
            MessageBox.Show("Molimo izaberite let za brisanje.", "Upozorenje",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var potvrda = MessageBox.Show(
            $"Da li ste sigurni da želite da obrišete ovaj let?\n\n{selektovani.BrojLeta} — {selektovani.Relacija}",
            "Potvrda brisanja",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (potvrda != MessageBoxResult.Yes) return;    // ako nije da pritisnuto prekini sve

        // stvarno brisanje
        try
        {
            using var ctx = new AvioContext();
            var let = ctx.Letovi.Find(selektovani.Id);  // naci let sa zadatim id
            // pronadjen je let
            if (let != null)
            {
                ctx.Letovi.Remove(let);
                ctx.SaveChanges();
            }
            Letovi.Remove(selektovani);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Greška pri brisanju:\n{ex.Message}", "Greška",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // detalji o selektovanom letu
    private void btnDetalji_Click(object sender, RoutedEventArgs e)
    {
        if (dgLetovi.SelectedItem is not Let selektovani)
        {
            MessageBox.Show("Molimo izaberite let za prikaz detalja.", "Upozorenje",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var detalji = new DetaljiWindow(selektovani.Id) { Owner = this };
        detalji.ShowDialog();
    }

    // prozor sa aviokompanijama
    private void btnAviokompanije_Click(object sender, RoutedEventArgs e)
    {
        var prozor = new AviokompanijaWindow { Owner = this };
        prozor.ShowDialog();
        UcitajLetove();
    }
}
