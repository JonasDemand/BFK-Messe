using System;
using System.Data;
using DbConnection.Entities;

namespace DbConnection.Repositories
{
	public static class UnternehmenRepository
	{
		public static int GetOrCreate(string name)
		{
            var dt = Connector.Current.ExecuteTable($"SELECT * FROM Unternehmen WHERE Name = '{name}'");
            var unternehmen = dt.AsEnumerable().Select(row =>
                new Unternehmen
                {
                    Id = row.Field<int>("Id"),
                    Name = row.Field<string>("Name")
                }).FirstOrDefault();
            if (unternehmen != null) return unternehmen.Id;
            return Connector.Current.ExecuteNonQuery($"INSERT INTO Unternehmen(Name) VALUES('{name}')");
        }
	}
}

