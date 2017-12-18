using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialListener
{
    public class Listener
    {
        //private 
        static SerialPort _serialPort1;
        private static bool _continue1;
        static SerialPort _serialPort2;
        private static bool _continue2;
        public static void close()
        {
            _continue1 = false;
            _serialPort1.Close();
            _serialPort1 = null;
            _continue2 = false;
            _serialPort2.Close();
            _serialPort2 = null;
        }
        private static string com1;
        private static string com2;
        private static TextBox text;
        internal static void serialInit(string text1, string text2)
        {
            com1 = text1;
            com2 = text2;
        }
        
        public static void start(TextBox box){

            text = box;
        // Create a new SerialPort object with default settings.
            _serialPort1 = new SerialPort();

            // Allow the user to set the appropriate properties.
            _serialPort1.PortName = com1;
            _serialPort1.BaudRate = 115200;
            _serialPort1.Parity = Parity.None;
            _serialPort1.DataBits = 8;
            _serialPort1.StopBits = StopBits.One;

            // Set the read/write timeouts
            _serialPort1.ReadTimeout = 500;
            _serialPort1.WriteTimeout = 500;

            _serialPort1.Open();
            _continue1 = true;
            
            // Create a new SerialPort object with default settings.
            _serialPort2 = new SerialPort();

            // Allow the user to set the appropriate properties.
            _serialPort2.PortName = com2;
            _serialPort2.BaudRate = 115200;
            _serialPort2.Parity = Parity.None;
            _serialPort2.DataBits = 8;
            _serialPort2.StopBits = StopBits.One;

            // Set the read/write timeouts
            _serialPort2.ReadTimeout = 500;
            _serialPort2.WriteTimeout = 500;

            _serialPort2.Open();
            _continue2 = true;
            Thread thread1 = new Thread(new ThreadStart(getReq));
            Thread thread2 = new Thread(new ThreadStart(getRes));
            thread1.Start();
            thread2.Start();

        }
        public static string x16(int bit) {
            if (bit < 10)
                return bit + "";
            else
                return (char)(bit - 10 + 'A')+"";

        }
        public  static  string byteToString(byte[]bits)
        {
            string str="";
            for(var i = 0; i < bits.Length; i++)
            {
               

                str += x16(bits[i] / 16) + x16(bits[i]%16) + " ";
            }
            return str+ "\r\n";

        }
        //1 代表res
        //2 代表req
        public static void getReq()
        {
            while (_continue1)
            {
                try
                {
                    int numbytes = _serialPort1.BytesToRead;
                    byte[] rxbytearray = new byte[numbytes];

                    for (int i = 0; i < numbytes; i++)
                    {
                        rxbytearray[i] = (byte)_serialPort1.ReadByte();
                    }
                    if (rxbytearray.Length != 0)
                    {
                        sendReq(rxbytearray);
                        text.Invoke((MethodInvoker)delegate {
                            // Running on the UI thread
                            text.Text += "from master: "+byteToString(rxbytearray);
                        });
                    }


                    //
                    // MessageBox.Show("??");

                }

                catch (TimeoutException)
                {
                    MessageBox.Show("??");

                    _continue1 = false;
                }
            }
        }
        public static void getRes()
        {
            while (_continue2)
            {
                try
                {
                    int numbytes = _serialPort2.BytesToRead;
                    byte[] rxbytearray = new byte[numbytes];

                    for (int i = 0; i < numbytes; i++)
                    {
                        rxbytearray[i] = (byte)_serialPort2.ReadByte();
                    }
                    if (rxbytearray.Length != 0)
                    {
                        sendRes(rxbytearray);
                        text.Invoke((MethodInvoker)delegate {
                            // Running on the UI thread
                            text.Text+= "from servant:"+ byteToString(rxbytearray);
                        });
                    }

                }
                catch (TimeoutException)
                {
                    _continue2 = false;
                }
            }
        }
        public static void sendReq(byte[] bits)
        {
            _serialPort2.Write(bits, 0, bits.Length);
        }
        public static void sendRes(byte[] bits)
        {
            _serialPort1.Write(bits, 0, bits.Length);
        }
    }
}
