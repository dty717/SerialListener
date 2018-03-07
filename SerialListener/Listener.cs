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

        internal static void serialCheckInit(string port1, string port2)
        {
            check = true;
            com1 = port1;
            com2 = port2;
            //throw new NotImplementedException();
        }

        private static bool check;
        private static string com1;
        private static string com2;
        private static TextBox text;
        internal static void serialInit(string text1, string text2)
        {
            com1 = text1;
            com2 = text2;
        }

        public static void start2(TextBox  text) {

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
            _serialPort1.ReadTimeout = 50000;
            _serialPort1.WriteTimeout = 50000;

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
            _serialPort2.ReadTimeout = 50000;
            _serialPort2.WriteTimeout = 50000;

            _serialPort2.Open();
            _continue2 = true;
            if (check)
            {
                Thread thread1 = new Thread(new ThreadStart(getCheckOne));
                Thread thread2 = new Thread(new ThreadStart(getCheckTwo));
                thread1.Start();
                thread2.Start();
                return;
            }
            else
            {
                Thread thread1 = new Thread(new ThreadStart(getReq));
                Thread thread2 = new Thread(new ThreadStart(getRes));
                thread1.Start();
                thread2.Start();
            }
            

        }

        public static void getCheckOne() {
            bool continues=false;
            while (_continue1)
            {
                Thread.Sleep(100);
                try
                {
                    int numbytes = _serialPort1.BytesToRead;

                    if (numbytes < 8)
                    {
                        continue;
                    }

                    byte[] rxbytearray = new byte[8];

                    for (int i = 0; i < 8; i++)
                    {
                        
                        rxbytearray[i] = (byte)_serialPort1.ReadByte();
                        if (i == 1)
                        {
                            if (rxbytearray[i] == 03)
                                continues = true;
                        }
                    }

                    if (continues)
                    {
                        continues = false;
                        rxbytearray = new byte[8];
                        if (numbytes >= 16)
                        {
                            for (int i = 0; i < 8; i++)
                            {

                                rxbytearray[i] = (byte)_serialPort1.ReadByte();
                                //if (i == 1)
                                //{
                                //    if (rxbytearray[i] == 03)
                                //        continues = true;
                                //}
                            }
                        }
                        //if (continues) {
                        //    continues = false;
                        //    continue;
                        //}
                    }
                    if (rxbytearray[0] == 0 && rxbytearray[1] == 0)
                        continue;
                    if (rxbytearray.Length != 0)
                    {
                        if (check1.Count < check2.Count)
                        {
                            byte[] checks = check2[check1.Count];
                            if (checks.Length != rxbytearray.Length)
                                MessageBox.Show(checks[0] + "" + checks[1] + "错误长度");
                            if (checks[0] != rxbytearray[0] || checks[1] != rxbytearray[1])
                            {
                                MessageBox.Show(checks[0] + "" + checks[1] + "错误头");
                            }
                            for (var i = 3; i < checks.Length; i++)
                            {
                                if (checks[i] != rxbytearray[i])
                                {
                                    MessageBox.Show(checks[0] + "" + checks[1] + "错误内容");
                                    break;
                                }
                            }
                        }
                        text.Invoke((MethodInvoker)delegate {
                            // Running on the UI thread
                            text.Text += "A:" + byteToString(rxbytearray);

                        });
                        check1.Add(rxbytearray);
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
        public static int big;

        public static void getCheckTwo()
        {
            bool continues = false ;
            while (_continue1)
            {
                Thread.Sleep(100);
                try
                {
                    int numbytes = _serialPort2.BytesToRead;
                    if (numbytes < 8)
                    {
                        continue;
                    }
                    byte[] rxbytearray = new byte[8];


                    for (int i = 0; i < numbytes; i++)
                    {
                        
                        rxbytearray[i] = (byte)_serialPort2.ReadByte();
                        if (i == 1)
                        {
                            if (rxbytearray[i] == 03)
                                continues=true;
                        }
                        Console.Write(rxbytearray[i]);
                    }
                    if (continues) {
                        continues = false;
                        rxbytearray = new byte[8];
                        if (numbytes >= 16)
                        {
                            for (int i = 0; i < 8; i++)
                            {

                                rxbytearray[i] = (byte)_serialPort2.ReadByte();
                                //if (i == 1)
                                //{
                                //    if (rxbytearray[i] == 03)
                                //        continues = true;
                                //}
                            }
                        }
                    }
                    if (rxbytearray[0] == 0 && rxbytearray[1] == 0)
                        continue;
                    Console.WriteLine();
                    if (rxbytearray.Length != 0)
                    {
                        if (check2.Count <check1.Count)
                        {
                            byte[] checks = check1[check2.Count];
                            if (checks.Length != rxbytearray.Length)
                                MessageBox.Show(checks[0] + "" + checks[1] + "错误长度");
                            if (checks[0] != rxbytearray[0] || checks[1] != rxbytearray[1])
                            {
                                MessageBox.Show(checks[0] + "" + checks[1] + "错误头");
                            }
                            for (var i = 3; i < checks.Length; i++)
                            {
                                if (checks[i] != rxbytearray[i])
                                {
                                    MessageBox.Show(checks[0] + "" + checks[1] + "错误内容");
                                    break;
                                }
                            }
                        }
                        text.Invoke((MethodInvoker)delegate {
                            // Running on the UI thread
                            text.Text += "B:" + byteToString(rxbytearray);

                        });
                        check2.Add(rxbytearray);
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
        public static List<byte[]> check1=new List<byte[]>();
        public static List<byte[]> check2= new List<byte[]>();

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
        public static string byteToString2(byte[] bits)
        {
            string str = "";
            for (var i = 0; i < bits.Length; i++)
            {
                str += bits[i]+" ";
            }
            return str + "\r\n";

        }
        //1 代表res
        //2 代表req
        public static void getReq()
        {
            while (_continue1)
            {
                if (MasterIsOn) {
                    Thread.Sleep(300);
                    MasterIsOn = false;
                }
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
                            text.Text += "下位机回复: " + byteToString(rxbytearray);
                            text.Text += "下位机对应数据: " + byteToString2(rxbytearray);

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
        static bool MasterIsOn;
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
                       
                        MasterIsOn = true;
                        Thread.Sleep(10);
                        sendRes(rxbytearray);
                        text.Invoke((MethodInvoker)delegate {
                            // Running on the UI thread
                            if (text.Text.Length > 10240)
                                text.Text = "";
                            text.Text+= "上位机指令:" + byteToString(rxbytearray);
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
