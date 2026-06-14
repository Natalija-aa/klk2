using System.Windows;
using klk2.Data;
using klk2.Models;

namespace klk2;

public partial class LetDialog : Window
{
    private readonly int? _letId;   // izmjena, dodavanje

    public int? SavedLetId { get; private set; }    // id leta koji je forma sacuvala

    public LetDialog(int? letId = null)
    {
        InitializeComponent();  // izgled forme
        _letId = letId;
        UcitajAviokompanije();  // padajuca lista / aviokompanije

        if (letId.HasValue) // da li ima vrijednost - iymjena
        {
            tbNaslov.Text = "Izmeni Let";
            PopuniFormu(letId.Value);
        }
    }

    // padajuca lista
    private void UcitajAviokompanije()
    {
        try
        {
            using var ctx = new AvioContext();
            cbAviokompanija.ItemsSource = ctx.Aviokompanije.ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Greška pri učitavanju aviokompanija:\n{ex.Message}", "Greška",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // ya iymjenu
    private void PopuniFormu(int id)
    {
        try
        {
            using var ctx = new AvioContext();
            var let = ctx.Letovi.Find(id);  // let po id
            if (let == null) return;

            // popunjavanje vrijednosti
            cbAviokompanija.SelectedValue = let.AviokompanijaId;
            tbBrojLeta.Text = let.BrojLeta;
            tbRelacija.Text = let.Relacija;
            dpDatum.SelectedDate = let.DatumVreme.Date;
            tpVreme.SelectedTime = DateTime.Today.Add(let.DatumVreme.TimeOfDay);
            tbCena.Text = let.CenaKarte.ToString("N2");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Greška pri učitavanju leta:\n{ex.Message}", "Greška",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // da li je unos tacan i dobar
    private bool Validiraj()
    {
        if (cbAviokompanija.SelectedItem == null)
        {
            MessageBox.Show("Molimo izaberite aviokompaniju.", "Validacija",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
        if (string.IsNullOrWhiteSpace(tbBrojLeta.Text))
        {
            MessageBox.Show("Broj leta ne sme biti prazan.", "Validacija",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            tbBrojLeta.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(tbRelacija.Text))
        {
            MessageBox.Show("Relacija ne sme biti prazna.", "Validacija",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            tbRelacija.Focus();
            return false;
        }
        if (dpDatum.SelectedDate == null)
        {
            MessageBox.Show("Molimo izaberite datum leta.", "Validacija",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
        if (tpVreme.SelectedTime == null)
        {
            MessageBox.Show("Molimo unesite vreme leta.", "Validacija",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
        if (!decimal.TryParse(tbCena.Text.Replace(',', '.'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal cena) || cena <= 0)
        {
            MessageBox.Show("Cena mora biti pozitivan broj.", "Validacija",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            tbCena.Focus();
            return false;
        }
        return true;
    }

    // Sacuvaj
    private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
    {
        if (!Validiraj()) return;

        // cijena
        decimal.TryParse(tbCena.Text.Replace(',', '.'),
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture,
            out decimal cena);

        var datumVreme = dpDatum.SelectedDate!.Value.Date.Add(tpVreme.SelectedTime!.Value.TimeOfDay);

        try
        {
            using var ctx = new AvioContext();

            // izmjena i dodavanje
            if (_letId.HasValue)
            {
                var let = ctx.Letovi.Find(_letId.Value);
                if (let != null)
                {
                    let.AviokompanijaId = (int)cbAviokompanija.SelectedValue;
                    let.BrojLeta = tbBrojLeta.Text.Trim();
                    let.Relacija = tbRelacija.Text.Trim();
                    let.DatumVreme = datumVreme;
                    let.CenaKarte = cena;
                }
                ctx.SaveChanges();
                SavedLetId = _letId.Value;
            }
            else
            {
                var let = new Let
                {
                    AviokompanijaId = (int)cbAviokompanija.SelectedValue,
                    BrojLeta = tbBrojLeta.Text.Trim(),
                    Relacija = tbRelacija.Text.Trim(),
                    DatumVreme = datumVreme,
                    CenaKarte = cena
                };
                ctx.Letovi.Add(let);
                ctx.SaveChanges();
                SavedLetId = let.Id;
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
