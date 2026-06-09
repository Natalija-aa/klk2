using System.Collections.ObjectModel;
using System.Windows;
using klk2.Data;
using klk2.Models;

namespace klk2;

public partial class AviokompanijaWindow : Window
{
    private ObservableCollection<Aviokompanija> Aviokompanije { get; set; } = new();

    public AviokompanijaWindow()
    {
        InitializeComponent();
        dgAviokompanije.ItemsSource = Aviokompanije;
        UcitajAviokompanije();
    }

    private void UcitajAviokompanije()
    {
        try
        {
            using var ctx = new AvioContext();
            var lista = ctx.Aviokompanije.ToList();
            Aviokompanije.Clear();
            foreach (var a in lista)
                Aviokompanije.Add(a);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Greška pri učitavanju podataka:\n{ex.Message}", "Greška",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnDodaj_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AviokompanijaDialog { Owner = this };
        if (dialog.ShowDialog() == true)
            UcitajAviokompanije();
    }

    private void btnIzmeni_Click(object sender, RoutedEventArgs e)
    {
        if (dgAviokompanije.SelectedItem is not Aviokompanija selektovana)
        {
            MessageBox.Show("Molimo izaberite aviokompaniju za izmenu.", "Upozorenje",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var dialog = new AviokompanijaDialog(selektovana.Id) { Owner = this };
        if (dialog.ShowDialog() == true)
            UcitajAviokompanije();
    }

    private void btnObrisi_Click(object sender, RoutedEventArgs e)
    {
        if (dgAviokompanije.SelectedItem is not Aviokompanija selektovana)
        {
            MessageBox.Show("Molimo izaberite aviokompaniju za brisanje.", "Upozorenje",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var potvrda = MessageBox.Show(
            $"Da li ste sigurni da želite da obrišete aviokompaniju?\n\n{selektovana.Naziv}",
            "Potvrda brisanja",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (potvrda != MessageBoxResult.Yes) return;

        try
        {
            using var ctx = new AvioContext();
            var a = ctx.Aviokompanije.Find(selektovana.Id);
            if (a != null)
            {
                ctx.Aviokompanije.Remove(a);
                ctx.SaveChanges();
            }
            Aviokompanije.Remove(selektovana);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Greška pri brisanju:\n{ex.Message}\n\nProvjerite da li ova aviokompanija ima povezane letove.", "Greška",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
