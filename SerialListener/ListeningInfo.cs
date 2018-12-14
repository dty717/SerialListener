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
    public partial class ListeningInfo : Form
    {
        public ListeningInfo()
        {
            InitializeComponent();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(s);
                comboBox2.Items.Add(s);
            }

            var count = comboBox1.Items.Count;
            if (count != 0)
            {
                comboBox1.SelectedIndex = 0;
                if (count > 1) {
                    comboBox2.SelectedIndex =1;
                }
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Listener.serialInit(comboBox1.Text, comboBox2.Text);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Listener.serialCheckInit(comboBox1.Text, comboBox2.Text,1);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Listener.serialCheckInit(comboBox1.Text, comboBox2.Text, 2);
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Listener.serialCheckInit(comboBox1.Text, comboBox2.Text, 3);
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Listener.serialCheckInit(comboBox1.Text, comboBox2.Text, 4);
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Listener.serialCheckInit(comboBox1.Text, comboBox2.Text, 5);
            this.Close();
        }
    }
}
