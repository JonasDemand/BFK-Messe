using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace DbConnection
{
    public class Connector
    {
        private static Connector _current;
        private const string DefaultConnection = "SERVER=localhost;DATABASE=BFKMesse;UID=root;PWD=;";
        public static Connector Current
        {
            get
            {
                if (_current == null)
                {
                    ReopenConnection();
                }
                return _current;
            }
        }

        private MySqlConnection connection;
        private Connector(string connectionString)
        {
            connection = new MySqlConnection(connectionString);
        }
        private void Open()
        {
            connection.Open();
        }
        private void Close()
        {
            connection.Close();
        }
        private MySqlDataReader Execute(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);
            return cmd.ExecuteReader();
        }

        public DataTable ExecuteTable(string query)
        {
            DataTable dt = new DataTable();
            dt.Load(Execute(query));
            return dt;
        }

        public int ExecuteNonQuery(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            return Convert.ToInt32(cmd.LastInsertedId);
        }

        public static void ReopenConnection()
        {
            _current = new Connector(DefaultConnection);
            _current.Open();
            AppDomain.CurrentDomain.ProcessExit +=
                new EventHandler((object sender, EventArgs e) => _current.Close());
        }
    }
}
