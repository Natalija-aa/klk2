namespace klk2.Models;

public class Aviokompanija
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
    public string Drzava { get; set; } = string.Empty;
    public List<Let> Letovi { get; set; } = new();  // svi letovi aviokompanije
}
