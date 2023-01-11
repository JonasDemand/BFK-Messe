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
                try
                {
                    DbConnection.Connector.ReopenConnection();

                } catch
                {
                    return;
                }

                _kunden.ForEach(x =>
                {
                    KundenRepository.Insert(x);
                    _kunden.Remove(x);
                });

                await Task.Delay(TimeSpan.FromSeconds(60));
            });
        }

        public static void SaveKunde(Kunde kunde)
        {
            _kunden.Add(kunde);
        }
	}
}

