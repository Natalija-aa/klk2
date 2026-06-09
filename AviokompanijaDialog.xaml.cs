using System.Windows;
using klk2.Data;
using klk2.Models;

namespace klk2;

public partial class AviokompanijaDialog : Window
{
    private readonly int? _aviokompanijaId;

    public int? SavedAviokompanijaId { get; private set; }

    public AviokompanijaDialog(int? aviokompanijaId = null)
    {
        InitializeComponent();
        _aviokompanijaId = aviokompanijaId;

        if (aviokompanijaId.HasValue)
        {
            tbNaslov.Text = "Izmeni Aviokompaniju";
            PopuniFormu(aviokompanijaId.Value);
        }
    }

    private void PopuniFormu(int id)
    {
        try
        {
            using var ctx = new AvioContext();
            var a = ctx.Aviokompanije.Find(id);
            if (a == null) return;

            tbNaziv.Text = a.Naziv;
            tbDrzava.Text = a.Drzava;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Greška pri učitavanju podataka:\n{ex.Message}", "Greška",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool Validiraj()
    {
        if (string.IsNullOrWhiteSpace(tbNaziv.Text))
        {
            MessageBox.Show("Naziv ne sme biti prazan.", "Validacija",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            tbNaziv.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(tbDrzava.Text))
        {
            MessageBox.Show("Država ne sme biti prazna.", "Validacija",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            tbDrzava.Focus();
            return false;
        }
        return true;
    }

    private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
    {
        if (!Validiraj()) return;

        try
        {
            using var ctx = new AvioContext();

            if (_aviokompanijaId.HasValue)
            {
                var a = ctx.Aviokompanije.Find(_aviokompanijaId.Value);
                if (a != null)
                {
                    a.Naziv = tbNaziv.Text.Trim();
                    a.Drzava = tbDrzava.Text.Trim();
                }
                ctx.SaveChanges();
                SavedAviokompanijaId = _aviokompanijaId.Value;
            }
            else
            {
                var a = new Aviokompanija
                {
                    Naziv = tbNaziv.Text.Trim(),
                    Drzava = tbDrzava.Text.Trim()
                };
                ctx.Aviokompanije.Add(a);
                ctx.SaveChanges();
                SavedAviokompanijaId = a.Id;
            }

            DialogResult = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Greška pri čuvanju:\n{ex.Message}", "Greška",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnOtkazi_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
