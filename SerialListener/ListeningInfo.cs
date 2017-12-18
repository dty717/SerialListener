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
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Listener.serialInit(textBox1.Text, textBox2.Text);
            this.Close();
        }


        
    }
}
