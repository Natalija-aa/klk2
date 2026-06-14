using System.Windows;
using klk2.Data;
using Microsoft.EntityFrameworkCore;

namespace klk2;

public partial class DetaljiWindow : Window
{
    public DetaljiWindow(int letId)
    {
        InitializeComponent();  // izgled
        UcitajDetalje(letId);   // podaci
    }

    private void UcitajDetalje(int letId)
    {
        try
        {
            using var ctx = new AvioContext();
            var let = ctx.Letovi.Include(l => l.Aviokompanija).FirstOrDefault(l => l.Id == letId);
            if (let == null) return;

            tbAviokompanija.Text = let.Aviokompanija.Naziv;
            tbDrzava.Text = let.Aviokompanija.Drzava;
            tbBrojLeta.Text = let.BrojLeta;
            tbRelacija.Text = let.Relacija;
            tbDatumVreme.Text = let.DatumVreme.ToString("dd.MM.yyyy HH:mm");
            tbCenaKarte.Text = $"{let.CenaKarte:N2} EUR";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Greška pri učitavanju detalja:\n{ex.Message}", "Greška",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnZatvori_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
