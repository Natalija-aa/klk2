using System.Collections.ObjectModel;
using System.Windows;
using klk2.Data;
using klk2.Models;
using Microsoft.EntityFrameworkCore;

namespace klk2;

public partial class MainWindow : Window
{
    public ObservableCollection<Let> Letovi { get; set; } = new();

    public MainWindow()
    {
        InitializeComponent();
        dgLetovi.ItemsSource = Letovi;
        UcitajLetove();
    }

    private void UcitajLetove()
    {
        try
        {
            using var ctx = new AvioContext();
            var letovi = ctx.Letovi.Include(l => l.Aviokompanija).ToList();
            Letovi.Clear();
            foreach (var let in letovi)
                Letovi.Add(let);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Greška pri učitavanju podataka:\n{ex.Message}", "Greška",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnDodaj_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new LetDialog { Owner = this };
        if (dialog.ShowDialog() == true && dialog.SavedLetId.HasValue)
        {
            try
            {
                using var ctx = new AvioContext();
                var noviLet = ctx.Letovi.Include(l => l.Aviokompanija)
                    .First(l => l.Id == dialog.SavedLetId.Value);
                Letovi.Add(noviLet);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri osvežavanju liste:\n{ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void btnIzmeni_Click(object sender, RoutedEventArgs e)
    {
        if (dgLetovi.SelectedItem is not Let selektovani)
        {
            MessageBox.Show("Molimo izaberite let za izmenu.", "Upozorenje",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var dialog = new LetDialog(selektovani.Id) { Owner = this };
        if (dialog.ShowDialog() == true)
        {
            try
            {
                using var ctx = new AvioContext();
                var azurirani = ctx.Letovi.Include(l => l.Aviokompanija)
                    .First(l => l.Id == selektovani.Id);

                var item = Letovi.FirstOrDefault(l => l.Id == selektovani.Id);
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

        if (potvrda != MessageBoxResult.Yes) return;

        try
        {
            using var ctx = new AvioContext();
            var let = ctx.Letovi.Find(selektovani.Id);
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

    private void btnAviokompanije_Click(object sender, RoutedEventArgs e)
    {
        var prozor = new AviokompanijaWindow { Owner = this };
        prozor.ShowDialog();
    }
}
