using DbConnection.Entities;
using System.Data;

namespace DbConnection.Repositories;

public static class KundenRepository
{
    public static List<Kunde> GetAll()
    {
        var dt = Connector.Current.ExecuteTable(@"
            SELECT
	            k.Id,
	            k.Nachname,
	            k.Vorname,
	            k.Adress1,
	            k.Adress2,
	            k.Bild,
	            k.UnternehmenId,
	            u.Name AS UnternehmenName,
	            p.Id AS ProduktgruppenId,
	            p.Name AS ProduktgruppenName
            FROM
	            Kunden k
            LEFT JOIN Unternehmen u ON
	            k.UnternehmenId = u.Id
            LEFT JOIN KundenProduktgruppen kp ON
	            k.Id = kp.KundenId
            INNER JOIN Produktgruppen p ON
	            kp.ProduktgruppenId = p.Id
        ");
        var kunden = dt.AsEnumerable().Select(row =>
            new Kunde
            {
                Id = row.Field<int>("Id"),
                Vorname = row.Field<string>("Vorname"),
                Nachname = row.Field<string>("Nachname"),
                Adress1 = row.Field<string>("Adress1"),
                Adress2 = row.Field<string>("Adress2"),
                Bild = row.Field<string>("Bild"),
                Unternehmen = new Unternehmen()
                {
                    Id = row.Field<int?>("UnternehmenId") ?? -1,
                    Name = row.Field<string>("UnternehmenName")
                },
                Produktgruppen = new List<Produktgruppe>()
            }).ToList();

        kunden.ForEach(x =>
        {
            var dt = Connector.Current.ExecuteTable($@"
                SELECT
	                p.Id,
	                p.Name
                FROM
	                Kunden k
                LEFT JOIN KundenProduktgruppen kp ON
	                k.Id = kp.KundenId
                INNER JOIN Produktgruppen p ON
	                kp.ProduktgruppenId = p.Id
                WHERE
	                k.Id = {x.Id}
            ");
            x.Produktgruppen.AddRange(dt.AsEnumerable().Select(row =>
            new Produktgruppe
            {
                Id = row.Field<int>("Id"),
                Name = row.Field<string>("Name")
            }));
        });
        return kunden;
    }
    
    public static int Insert(Kunde kunde)
    {
        int? unternehmenId = string.IsNullOrEmpty(kunde.Unternehmen?.Name) ? null : UnternehmenRepository.GetOrCreate(kunde.Unternehmen.Name);

        var kundenId = Connector.Current.ExecuteNonQuery($@"
            INSERT
	            INTO
	            Kunden(Nachname,
	            Vorname,
	            Adress1,
	            Adress2,
	            UnternehmenId,
	            Bild)
            VALUES('{kunde.Nachname}',
            '{kunde.Vorname}',
            '{kunde.Adress1}',
            '{kunde.Adress2}',
            {unternehmenId?.ToString() ?? "NULL"},
            '{kunde.Bild}')
        ");

        kunde.Produktgruppen.ForEach(x => Connector.Current.ExecuteNonQuery($@"
            INSERT
	            INTO
	            BFKMesse.KundenProduktgruppen(KundenId,
	            ProduktgruppenId)
            VALUES({kundenId},
            {x.Id})
        "));

        return kundenId;
    }
}