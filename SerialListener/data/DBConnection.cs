using MySql.Data;
using MySql.Data.MySqlClient;


namespace SerialListener.data
{
        
         public class DBConnection
        {
            private DBConnection()
            {
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

            private static DBConnection _instance = null;
            public static DBConnection Instance()
            {
                if (_instance == null)
                    _instance = new DBConnection();
                return _instance;
            }

            public bool IsConnect(string server)
            {
                if (Connection == null)
                {
                    if (string.IsNullOrEmpty(databaseName))
                        return false;
                    string connstring = string.Format("Server={0}; database={1}; UID=root; password=root",server, databaseName);
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

