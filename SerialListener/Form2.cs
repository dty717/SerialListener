using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialListener
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Listener.startNew(textBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tem = richTextBox1.Text.Trim().Split(' ');
            byte[] data = new byte[tem.Length];
            for(int i=0;i<data.Length;i++){
                if (tem[i].StartsWith("0x")||tem[i].StartsWith("0X")) {
                    data[i] = byte.Parse(tem[i].Substring(2),NumberStyles.AllowHexSpecifier);
                }
                else
                    data[i]=byte.Parse(tem[i],NumberStyles.AllowHexSpecifier);
            }
            Listener.sendRes(data);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var tem = richTextBox2.Text.Trim().Split(' ');
            byte[] data = new byte[tem.Length];
            for(int i=0;i<data.Length;i++){
                if (tem[i].StartsWith("0x")||tem[i].StartsWith("0X")) {
                    data[i] = byte.Parse(tem[i].Substring(2),NumberStyles.AllowHexSpecifier);
                }
                else
                    data[i]=byte.Parse(tem[i],NumberStyles.AllowHexSpecifier);
            }
            Listener.sendReq(data);
        }
    }
}
