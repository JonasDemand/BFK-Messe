using DbConnection.Entities;

namespace DbConnection.Repositories;

public static class KundenRepository
{
    public static List<Kunde> GetAll()
    {
        return new List<Kunde>();
    }
    
    public static Kunde Insert(Kunde kunde)
    {
        return kunde;
    }
}