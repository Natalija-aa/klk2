using System.Windows;
using klk2.Data;
using klk2.Models;

namespace klk2;

public partial class LetDialog : Window
{
    private readonly int? _letId;

    public int? SavedLetId { get; private set; }

    public LetDialog(int? letId = null)
    {
        InitializeComponent();
        _letId = letId;
        UcitajAviokompanije();

        if (letId.HasValue)
        {
            tbNaslov.Text = "Izmeni Let";
            PopuniFormu(letId.Value);
        }
    }

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

    private void PopuniFormu(int id)
    {
        try
        {
            using var ctx = new AvioContext();
            var let = ctx.Letovi.Find(id);
            if (let == null) return;

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

    private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
    {
        if (!Validiraj()) return;

        decimal.TryParse(tbCena.Text.Replace(',', '.'),
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture,
            out decimal cena);

        var datumVreme = dpDatum.SelectedDate!.Value.Date.Add(tpVreme.SelectedTime!.Value.TimeOfDay);

        try
        {
            using var ctx = new AvioContext();

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
