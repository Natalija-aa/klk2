using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace klk2.Models;

public class Let : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public int Id { get; set; }

    private string _brojLeta = string.Empty;
    public string BrojLeta
    {
        get => _brojLeta;
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

    private int _aviokompanijaId;
    public int AviokompanijaId
    {
        get => _aviokompanijaId;
        set { if (_aviokompanijaId != value) { _aviokompanijaId = value; OnPropertyChanged(); } }
    }

    private Aviokompanija _aviokompanija = null!;
    public Aviokompanija Aviokompanija
    {
        get => _aviokompanija;
        set { if (_aviokompanija != value) { _aviokompanija = value; OnPropertyChanged(); } }
    }
}
