using System.Data;
using DbConnection.Entities;

namespace DbConnection.Repositories;

public static class ProduktgruppenRepository
{
    public static List<Produktgruppe> GetAll()
    {
        var dt = Connector.Current.ExecuteTable("SELECT * FROM Produktgruppen");
        return dt.AsEnumerable().Select(row =>
            new Produktgruppe
            {
                Id = row.Field<int>("Id"),
                Name = row.Field<string>("Name"),
            }).ToList();
    }
}