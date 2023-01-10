namespace DbConnection.Entities;

public class Kunde
{  
    public int Id { get; set; }
    public string Nachname { get; set; }
    public string Vorname { get; set; }
    public string Adress1 { get; set; }
    public string Adress2 { get; set; }
    public string Bild { get; set; }
    public Unternehmen Unternehmen { get; set; }
    public List<Produktgruppe> Produktgruppen { get; set; }
}