using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialListener.data
{
    public class RemoteDataBase
    {
        private RemoteDataBase() {

        }
        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static RemoteDataBase _instance = null;
        public static RemoteDataBase Instance()
        {
            if (_instance == null)
                _instance = new RemoteDataBase();
            return _instance;
        }

        public bool IsConnect(string server)
        {
            if (Connection == null)
            {
                if (string.IsNullOrEmpty(databaseName))
                    return false;
                string connstring = string.Format("Server={0};PORT=3306 ;database={1}; UID=root; password=root", server, databaseName);
                connection = new MySqlConnection(connstring);
                connection.Open();
            }
            return true;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
