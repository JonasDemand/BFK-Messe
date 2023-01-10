using DbConnection.Entities;
using System.Data;

namespace DbConnection.Repositories;

public static class KundenRepository
{
    public static List<Kunde> GetAll()
    {
        //TODO Add Fk
        var dt = Connector.Current.ExecuteTable("SELECT * FROM Kunden");
        var kunden = dt.AsEnumerable().Select(row =>
            new Kunde
            {
                Id = row.Field<int>("Id"),
                Vorname = row.Field<string>("Vorname"),
                Nachname = row.Field<string>("Nachname"),
                Adress1 = row.Field<string>("Adress1"),
                Adress2 = row.Field<string>("Adress2"),
                Bild = row.Field<string>("Bild")
            }).ToList();
        return kunden;
    }
    
    public static int Insert(Kunde kunde)
    {
        int? unternehmenId = string.IsNullOrEmpty(kunde.Unternehmen?.Name) ? null : UnternehmenRepository.GetOrCreate(kunde.Unternehmen.Name);

        var kundenId = Connector.Current.ExecuteNonQuery($"INSERT INTO Kunden(Nachname, Vorname, Adress1, Adress2, UnternehmenId, Bild) VALUES('{kunde.Nachname}', '{kunde.Vorname}', '{kunde.Adress1}', '{kunde.Adress2}', {unternehmenId?.ToString() ?? "NULL"}, '{kunde.Bild}')");

        kunde.Produktgruppen.ForEach(x => Connector.Current.ExecuteNonQuery($"INSERT INTO BFKMesse.KundenProduktgruppen(KundenId, ProduktgruppenId) VALUES({kundenId}, {x.Id})"));

        return kundenId;
    }
}