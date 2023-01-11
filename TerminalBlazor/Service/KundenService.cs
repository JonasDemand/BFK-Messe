using System;
using DbConnection.Entities;
using DbConnection.Repositories;

namespace TerminalBlazor
{
	public static class KundenService
	{
        private static List<Kunde> _kunden = new();

		static KundenService()
		{
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        DbConnection.Connector.ReopenConnection();
                        _kunden.ForEach(x =>
                        {
                            KundenRepository.Insert(x);
                            _kunden.Remove(x);
                        });

                    }
                    catch {}
                    finally
                    {
                        await Task.Delay(TimeSpan.FromSeconds(60));
                    }
                }
            });
        }

        public static void SaveKunde(Kunde kunde)
        {
            _kunden.Add(kunde);
        }
	}
}

