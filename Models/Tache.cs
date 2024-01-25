namespace Tasks_WEB_API;
public class Tache
{
    public string? Matricule { get; set; }
    public string? Titre { get; set; }

    public string? Summary { get; set; }
    public List<Tache>? Taches { get; set; }
    public DateTime Date { get; set; }
}