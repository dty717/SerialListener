using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialListener
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            data.DBConnection dB=data.DBConnection.Instance();

            //
            dB.DatabaseName = "事务管理";
            dB.IsConnect();
            MySqlConnection conn =dB.Connection;

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from signon";
            //cmd.Parameters.Add("?person", MySqlDbType.VarChar).Value = "myname";
            //cmd.Parameters.Add("?address", MySqlDbType.VarChar).Value = "myaddress";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string someStringFromColumnZero = reader.GetString(0);
                string someStringFromColumnOne = reader.GetString(1);
                Console.WriteLine(someStringFromColumnZero + "," + someStringFromColumnOne);
            }
            conn.Close();
            Console.WriteLine();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ListeningInfo());
            Application.Run(new Form1());
            Listener.close();
        }
    }
}
