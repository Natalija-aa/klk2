using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace klk2.Models;

public class Let : INotifyPropertyChanged
{
    // dogadjaj
    public event PropertyChangedEventHandler? PropertyChanged;

    // metoda
    // Invoke salje samo kada ima neko preplacen
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public int Id { get; set; } // isti kao broj leta

    private string _brojLeta = string.Empty;
    public string BrojLeta
    {
        get => _brojLeta;
        // promjeni vrijednost ako se razlikuje od stare
        set { if (_brojLeta != value) { _brojLeta = value; OnPropertyChanged(); } }
    }

    private string _relacija = string.Empty;
    public string Relacija
    {
        get => _relacija;
        set { if (_relacija != value) { _relacija = value; OnPropertyChanged(); } }
    }

    private DateTime _datumVreme;
    public DateTime DatumVreme
    {
        get => _datumVreme;
        set { if (_datumVreme != value) { _datumVreme = value; OnPropertyChanged(); } }
    }

    private decimal _cenaKarte;
    public decimal CenaKarte
    {
        get => _cenaKarte;
        set { if (_cenaKarte != value) { _cenaKarte = value; OnPropertyChanged(); } }
    }

    // kojoj aviokompaniji pripada let / kljuc
    private int _aviokompanijaId;
    public int AviokompanijaId
    {
        get => _aviokompanijaId;
        set { if (_aviokompanijaId != value) { _aviokompanijaId = value; OnPropertyChanged(); } }
    }

    // referenca
    private Aviokompanija _aviokompanija = null!;
    public Aviokompanija Aviokompanija
    {
        get => _aviokompanija;
        set { if (_aviokompanija != value) { _aviokompanija = value; OnPropertyChanged(); } }
    }
}
