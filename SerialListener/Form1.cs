using MySql.Data.MySqlClient;
using SerialListener.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
        
namespace SerialListener
{
    public partial class Form1 : Form
    {   

        public Form1()
        {
            InitializeComponent();
        }

        RemoteDataBase dB = RemoteDataBase.Instance();

        private void Form1_Load(object sender, EventArgs e)
        {
            Listener.start(textBox1);
            dB.DatabaseName = "事务管理";
        }
        MySqlCommand cmd;


        private void timer1_Tick(object sender, EventArgs e)
        {
            return;

            dB.IsConnect("192.168.1.166");
            {
                cmd = dB.Connection.CreateCommand();
                cmd.CommandText = "insert into test (A) values (" + test.state + ")";

                //dB.Connection.Close();
                cmd.ExecuteNonQuery();
                //dB.Connection.Close();
            }


        }
    }   
}
