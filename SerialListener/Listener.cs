using SerialListener.TCP;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (_serialPort2 != null && _serialPort2.IsOpen)
                _serialPort2.Close();
            _serialPort2 = null;
            file.Close();
        }

        internal static void serialCheckInit(string port1, string port2, int i)
        {
            check = i;
            com1 = port1;
            com2 = port2;
            //throw new NotImplementedException();
        }

        private static int check;
        private static string com1;
        private static string com2;
        private static TextBox text;
        internal static void serialInit(string text1, string text2)
        {
            com1 = text1;
            com2 = text2;
        }

        public static void start2(TextBox text)
        {

        }

        public static Parity getParity(string parity)
        {
            switch (parity)
            {
                case "None": return Parity.None;
                case "Even": return Parity.Even;
                case "Mark": return Parity.Mark;
                case "Odd": return Parity.Odd;
                case "Space": return Parity.Space;

            }
            throw new Exception("");
        }
        public static StopBits getStopBit(string parity)
        {
            switch (parity)
            {
                case "None": return StopBits.None;
                case "1":
                case "One": return StopBits.One;
                case "1.5":
                case "OnePointFive": return StopBits.OnePointFive;
                case "2":
                case "Two": return StopBits.Two;
            }
            throw new Exception("");
        }

        public static void assgin(string name, string value)
        {
            switch (name)
            {
                case "baud": BaudRate = Convert.ToInt32(value.Trim()); break;
                case "databit": DataBits = Convert.ToInt16(value.Trim()); break;
                case "paritybit": Parity = getParity(value.Trim()); break;
                case "stopbit": StopBits = getStopBit(value.Trim()); break;
                case "ip": serverIp = value.Trim();break;
                case "port": serverPort = Convert.ToInt16(value.Trim()); break;
            }
        }
        static String serverIp;
        static int serverPort;
        static int BaudRate;
        static int DataBits;
        static Parity Parity;
        static StopBits StopBits;
        static StreamWriter file;
        private static bool initReadParm()
        {
            DateTime date = DateTime.Now;
            file = new StreamWriter("testListen" + date.ToString("yyyy-MM-dd HH-mm") + ".txt", false);



            if (!File.Exists(Directory.GetCurrentDirectory() + "\\serial.ini"))
                return false;
            IEnumerable<string> infos = File.ReadLines(Directory.GetCurrentDirectory() + "\\serial.ini");

            for (int i = 0, m = infos.Count(); i < m; i++)
            {
                string[] a = infos.ElementAt(i).Split('=');
                if (a.Length == 2)
                    assgin(a[0], a[1]);
            }
            return true;
        }






        public static void start(TextBox box)
        {


            //   if()    
            text = box;
            // Create a new SerialPort object with default settings.
            if (initReadParm())
            {
                _serialPort1 = new SerialPort();


                // Allow the user to set the appropriate properties.
                _serialPort1.PortName = com1;
                _serialPort1.BaudRate = BaudRate;
                _serialPort1.Parity = Parity;
                _serialPort1.DataBits = DataBits;
                _serialPort1.StopBits = StopBits;

                // Set the read/write timeouts
                _serialPort1.ReadTimeout = 50000;
                _serialPort1.WriteTimeout = 50000;

                _serialPort1.Open();
                _continue1 = true;

                if (check==0||check == 1 || check == 2)
                {
                    // Create a new SerialPort object with default settings.
                    _serialPort2 = new SerialPort();

                    // Allow the user to set the appropriate properties.
                    _serialPort2.PortName = com2;
                    _serialPort2.BaudRate = BaudRate;
                    _serialPort2.Parity = Parity;
                    _serialPort2.DataBits = DataBits;
                    _serialPort2.StopBits = StopBits;

                    // Set the read/write timeouts
                    _serialPort2.ReadTimeout = 50000;
                    _serialPort2.WriteTimeout = 50000;
                }
            }
            else
            {
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
            }

            try
            {
                _serialPort2.Open();
            }
            catch (Exception)
            {
            }
            _continue2 = true;
            if (check == 1)
            {
                Thread thread1 = new Thread(new ThreadStart(getCheckOne));
                Thread thread2 = new Thread(new ThreadStart(getCheckTwo));
                thread1.Start();
                thread2.Start();
                return;
            }
            else if (check == 2)
            {
                Thread thread1 = new Thread(new ThreadStart(post));
                thread1.Start();
            }
            else if (check == 0)
            {
                Thread thread1 = new Thread(new ThreadStart(getReq));
                Thread thread2 = new Thread(new ThreadStart(getRes));
                thread1.Start();
                thread2.Start();
            }
            else if (check == 3)
            {
                Thread thread1 = new Thread(new ThreadStart(checkReq));
                thread1.Start();
            }
            else if (check == 4)
            {
                Thread thread1 = new Thread(new ThreadStart(post_4));
                thread1.Start();
            }else if (check == 5)
            {
                Thread thread1 = new Thread(new ThreadStart(TCPConnect));
                thread1.Start();
            }

        }
        public static void checkReq()
        {
            byte[,] rxs = { {0x02,0x05,0x00,0x00,0x00,0x00,0xCD,0xF9 },
                {0x03,0x05,0x00,0x00,0x00,0x00,0xCC,0x28},
                {0x04,0x05,0x00,0x00,0x00,0x00,0xCD,0x9F},
                {0x05,0x05,0x00,0x00,0x00,0x00,0xCC,0x4E},
                {0x06,0x05,0x00,0x00,0x00,0x00,0xCC,0x7D},
                {0x07,0x05,0x00,0x00,0x00,0x00,0xCD,0xAC},
                {0x08,0x05,0x00,0x00,0x00,0x00,0xCD,0x53},
                {0x09,0x05,0x00,0x00,0x00,0x00,0xCC,0x82}
            };

            for (var k = 2; k < 10; k++)
            {
                if (MasterIsOn)
                {
                    MasterIsOn = false;
                }
                try
                {
                    byte[] rxbytearray = new byte[8];
                    for (int j = 0; j < rxs.GetLength(1); j++)
                        rxbytearray[j] = rxs[k - 2, j];
                    sendRes(rxbytearray);
                    Thread.Sleep(400);
                    int numbytes = _serialPort1.BytesToRead;
                    rxbytearray = new byte[numbytes];

                    for (int i = 0; i < numbytes; i++)
                    {
                        rxbytearray[i] = (byte)_serialPort1.ReadByte();
                    }
                    if (numbytes == 8)
                    {
                        if (rxbytearray[0] == rxs[k - 2, 0] && rxbytearray[3] == 0xFE)
                            text.Invoke((MethodInvoker)delegate
                            {
                                // Running on the UI thread
                                text.Text += "下位机地址:" + rxs[k - 2, 0] + "回复正确\r\n";
                                //text.Text += "下位机对应数据: " + byteToString2(rxbytearray);
                                //file.Write("下位机回复: " + byteToString(rxbytearray) + "下位机对应数据: " + byteToString                            });
                            });
                        else
                        {
                            text.Invoke((MethodInvoker)delegate
                            {
                                var str = "";
                                for (var i = 0; i < numbytes; i++)
                                {
                                    str += rxbytearray[i].ToString("x") + " ";
                                }
                                text.Text += "下位机地址:" + rxs[k - 2, 0] + "回复错误:" + str+ "\r\n";
                            });


                        }
                    }
                    else if (numbytes == 0)
                    {
                        text.Invoke((MethodInvoker)delegate
                        {
                            text.Text += "下位机地址:" + rxs[k - 2, 0] + " 错误:未响应\r\n";
                        });

                    }
                    else {
                        text.Invoke((MethodInvoker)delegate
                        {
                            var str = "";
                            for (var i = 0; i < numbytes; i++)
                            {
                                str += rxbytearray[i].ToString("x") + " ";
                            }
                            text.Text += "下位机地址:" + rxs[k - 2, 0] + "回复错误:" + str+ "\r\n";
                        });
                    }

                }
                catch (TimeoutException)
                {
                    
                    _continue1 = false;
                }
                Thread.Sleep(300);
            }

        }



        static bool MessageReceived(byte[] data)
        {
            text.Invoke((MethodInvoker)delegate
            {
                if (text.Text.Length > 1024)
                    text.Text = "";
                // Running on the UI thread
                text.Text += Encoding.UTF8.GetString(data) + "\r\n";
            });
            return true;
        }

        static bool ServerConnected()
        {
            Console.WriteLine("Server connected");
            return true;
        }

        static bool ServerDisconnected()
        {
            Console.WriteLine("Server disconnected");
            return true;
        }

        public static void TCPConnect()
        {
            WatsonTcpClient client = new WatsonTcpClient(serverIp, serverPort, ServerConnected, ServerDisconnected, MessageReceived, true);

            var _continue = true;
            while (_continue)
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
                        if (rxbytearray[0] == (byte)01 && rxbytearray[1] != (byte)0x03)
                        {
                            client.Send(Encoding.UTF8.GetBytes("data:" + BitConverter.ToString(rxbytearray).Replace("-", " ")));
                            text.Invoke((MethodInvoker)delegate
                            {
                                if (text.Text.Length > 1024)
                                    text.Text = "";
                                // Running on the UI thread
                                text.Text += BitConverter.ToString(rxbytearray).Replace("-", " ") + "\r\n";
                            });
                        }
                    }

                }

                catch (TimeoutException)
                {
                    MessageBox.Show("??");

                    _continue1 = false;
                }
            }

            text.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                text.Text += "A:" + byteToString(null);

            });

        }
        public static void post_4()
        {
            var _continue = true;
            test test = new test();
            while (_continue)
            {
                //Thread.Sleep(1000);
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
                        byte[] m = new byte[rxbytearray.Length-2];
                        for (var i = 0; i < m.Length; i++) {
                            m[i] = rxbytearray[i + 2];
                        }
                        var str = test.AnalysisD(m, m.Length);
                        //MessageBox.Show(str);
                        //sendReq(rxbytearray);
                        text.Invoke((MethodInvoker)delegate
                        {
                            if (text.Text.Length > 1024)
                                text.Text = "";
                            // Running on the UI thread
                            text.Text += str+"\r\n";
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

            text.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                text.Text += "A:" + byteToString(null);

            });

        }

        public static void post()
        {
            var _continue = true;
            test test = new test();
            while (_continue)
            {
                //Thread.Sleep(1000);
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
                        var str = test.usfulByte(rxbytearray);
                        MessageBox.Show(str);
                        //sendReq(rxbytearray);
                        text.Invoke((MethodInvoker)delegate
                        {
                            // Running on the UI thread
                            text.Text = str;

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

            text.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                text.Text += "A:" + byteToString(null);

            });

        }

        public static void getCheckOne()
        {
            bool continues = false;
            while (_continue1)
            {
                //Thread.Sleep(100);
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
                        text.Invoke((MethodInvoker)delegate
                        {
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
            bool continues = false;
            while (_continue1)
            {
                //Thread.Sleep(100);
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
                                continues = true;
                        }
                        Console.Write(rxbytearray[i]);
                    }
                    if (continues)
                    {
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
                        if (check2.Count < check1.Count)
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
                        text.Invoke((MethodInvoker)delegate
                        {
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
        public static List<byte[]> check1 = new List<byte[]>();
        public static List<byte[]> check2 = new List<byte[]>();

        public static string x16(int bit)
        {
            if (bit < 10)
                return bit + "";
            else
                return (char)(bit - 10 + 'A') + "";

        }
        public static string byteToString(byte[] bits)
        {
            string str = "";
            for (var i = 0; i < bits.Length; i++)
            {


                str += x16(bits[i] / 16) + x16(bits[i] % 16) + " ";
            }
            return str + "\r\n";

        }
        public static string byteToString2(byte[] bits)
        {
            string str = "";
            for (var i = 0; i < bits.Length; i++)
            {
                str += bits[i] + " ";
            }
            return str + "\r\n";

        }
        //1 代表res
        //2 代表req
        public static void getReq()
        {
            while (_continue1)
            {
                if (MasterIsOn)
                {
                    //Thread.Sleep(300);
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
                        text.Invoke((MethodInvoker)delegate
                        {
                            // Running on the UI thread
                            text.Text += "下位机回复: " + byteToString(rxbytearray);
                            text.Text += "下位机对应数据: " + byteToString2(rxbytearray);
                            file.Write("下位机回复: " + byteToString(rxbytearray) + "下位机对应数据: " + byteToString(rxbytearray));
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
                        //Thread.Sleep(10);
                        sendRes(rxbytearray);
                        text.Invoke((MethodInvoker)delegate
                        {
                            // Running on the UI thread
                            if (text.Text.Length > 10240)
                                text.Text = "";
                            text.Text += "上位机指令:" + byteToString(rxbytearray);
                            file.Write("上位机指令:" + byteToString(rxbytearray));
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
