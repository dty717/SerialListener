﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SerialListener
{
    class test
    {
        private bool checkHead(byte[] get)
        {
            //01 03 4C
            if (get[0] != 0x01)
                return false;
            if (get[1] != 0x03)
                return false;
            if (get[2] != 0x4c)
            {
                return false;
            }
            return true;
        }

        int[] rates = { };
        public test()
        {
            string[] lines = System.IO.File.ReadAllLines(@"test.txt");
            var start = false;
            var end = false;
            int line = 0;
            var str = "";
            for (var i = 0; i < lines.Length; i++)
            {
                if (!start)
                    if (lines[i].IndexOf("信息位:") != -1)
                    {
                        start = true;
                        
                    }
                    else
                    {
                        continue;
                    }
                if (!end)
                {
                    if (lines[i].Trim() == "")
                    {
                        end = true;
                        line = i;
                        break;
                    }
                    else
                    {
                        str += lines[i].Trim();
                    }
                }

            }
            // Display the file contents by using a foreach loop.
            infos = str.Substring(str.IndexOf(":") + 1).Split(',').Select(email => email.Trim()).ToArray();
            data = new int[infos.Length];

            start = false;
            end = false;
            str = "";
            for (var i = line; i < lines.Length; i++)
            {
                if (!start)
                    if (lines[i].IndexOf("两位数据:") != -1)
                    {
                        start = true;

                    }
                    else
                    {
                        continue;
                    }
                if (!end)
                {
                    if (lines[i].Trim() == "")
                    {
                        end = true;
                        line = i;
                        break;
                    }
                    else
                    {
                        str += lines[i].Trim();
                    }
                }
            }
            String[]arrange = str.Substring(str.IndexOf(":") + 1).Split(',').Select(email => email.Trim()).ToArray();

            for (var i = 0; i < arrange.Length; i++) {
                Console.WriteLine(arrange[i]);
                if (arrange[i][0] == '[')
                {
                    list.Add(int.Parse(arrange[i].Substring(1)) * 2 - 1);
                }
                else if (arrange[i][0] == '(')
                {
                    list.Add(int.Parse(arrange[i].Substring(1)) * 2);
                }
                else if (arrange[i][arrange[i].Length - 1] == ')')
                {
                    list.Add(int.Parse(arrange[i].Substring(0, arrange[i].Length - 1)) * 2);
                }
                else if (arrange[i][arrange[i].Length - 1] == ']')
                {
                    list.Add(int.Parse(arrange[i].Substring(0, arrange[i].Length - 1)) * 2+1);
                }

            }


            start = false;
            end = false;
            str = "";


            for (var i = line; i < lines.Length; i++)
            {
                if (!start)
                {
                    if (lines[i].IndexOf("赋值方式:") != -1)
                    {
                        start = true;

                    }
                        continue;
                }
                if (!end)
                {
                    if (lines[i].Trim() == "//*")
                    {
                        end = true;
                        line = i;
                        break;
                    }
                    else
                    {
                        str += lines[i].Trim()+"\r\n";
                    }
                }
            }

            start = false;
            end = false;

            for (var i = line; i < lines.Length; i++)
            {
                if (!start)
                {
                    if (lines[i].IndexOf("比率:") != -1)
                    {
                        start = true;
                    }
                    continue;
                }
                if (!end)
                {
                    if (lines[i].Trim() == "//*")
                    {
                        end = true;
                        line = i;
                        break;
                    }
                    else
                    {
                        str += lines[i].Trim() + "\r\n";
                    }
                }
            }


            string sourceCode = @"
                public class ModelClass {
                    " + str+"}";
            Console.WriteLine(sourceCode);
            var compParms = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true
            };
            var csProvider = new CSharpCodeProvider();
            CompilerResults compilerResults =
                csProvider.CompileAssemblyFromSource(compParms, sourceCode);
            typeInstance =
                compilerResults.CompiledAssembly.CreateInstance("ModelClass");
            modelMethod = typeInstance.GetType().GetMethod("Model");
            rateMethod = typeInstance.GetType().GetMethod("Rate");

            start = false;
            end = false;

            str = "";
            for (var i = 0; i < lines.Length; i++)
            {
                if (!start)
                    if (lines[i].IndexOf("数据大小:") != -1)
                    {
                        start = true;
                    }
                    else
                    {
                        continue;
                    }
                if (!end)
                {
                    if (lines[i].Trim() == "")
                    {
                        end = true;
                        line = i;
                        break;
                    }
                    else
                    {
                        str += lines[i].Trim();
                    }
                }

            }
            dataSize = str.Substring(str.IndexOf(":") + 1).Split(',').Select(email => int.Parse(email.Trim())).ToArray();

        }
        public static object typeInstance;
        public static MethodInfo modelMethod;
        public static MethodInfo rateMethod;

        public static String[] infos;
        int[] data;
        List<int> list = new List<int>();
        public bool arrange(int m) {
            for (var i = 0; i < list.Count; i+=2) {
                if (between(list[i + 1],list[i],m)){
                    return true;
                }
            }
            return false;
        }
        public bool between(int a,int b ,int x ) {
            return (2*x - a) * (2*x - b) < 0;
        }



        int[] dataSize;
        public String usfulByte(byte[] bytes)
        {
            String nowTime = DateTime.Now.ToString();

            if (bytes == null || bytes.Length < 80 || !checkHead(bytes))
                return null;
            int p = 0, m = 0;

            for (var i = 3; i < bytes.Length - 2; i += 2)
            {
                //三位数据:[3, 20),[29,47),[55,70)

                if (arrange(i))
                {
                    data[p++] = readTwo(bytes[i], bytes[i + 1]);
                    //Console.WriteLine("名称:"+infos[p-1]+"    数值:"+data[p]+"    信息位:" + (p ) + "   操作位:" + 2+"   数据位:"+(i-3)/2);
                }
                else
                {
                    int[] ints = readOneByOne(bytes[i], bytes[i + 1]);
                    if (m == dataSize.Length)
                        break;
                    for (var ii = 0; ii < dataSize[m]; ii++)
                    {
                        if (m == 8 || m == 10)
                        {
                            data[16 + p++] = ints[ii];

                        }
                        else if (m == 9 || m == 11)
                        {
                            data[-2 + p++] = ints[ii];
                        }
                        else
                            data[p++] = ints[ii];
                        // //Console.WriteLine("名称:" + infos[p - 1] + "   信息位:" + (p ) + "   操作位:" + 1+ "   数据位:" + (i - 3) / 2+"   参数位:" + m+"    "+ dataSize[m]);
                    }
                    m++;
                    //
                }

            }

            //DataTable deviceTable = (DataTable)Static.get("deviceTable");
            //DataTable exceptionTable = (DataTable)Static.get("exceptionTable");

            //for (var i = 0; i < 9; i++)
            //{
            //    exceptionTable.Rows[i].SetField(1, (double)data[i] / rate(i));
            //    exceptionTable.Rows[i].SetField(2, model(i + 14, data[i + 14]));
            //    exceptionTable.Rows[i].SetField(3, model(i + 23, data[i + 23]));
            //    exceptionTable.Rows[i].SetField(4, model(i + 32, data[i + 32]));
            //    exceptionTable.Rows[i].SetField(5, typeof(实验参数).GetField("A" + (i + 1) + "占空比").GetValue(null));
            //}
            //for (var i = 0; i < 9; i++)
            //{
            //    exceptionTable.Rows[i + 9].SetField(1, (double)data[i + 41] / rate(i + 41));
            //    exceptionTable.Rows[i + 9].SetField(2, model(i + 55, data[i + 55]));
            //    exceptionTable.Rows[i + 9].SetField(3, model(i + 64, data[i + 64]));
            //    exceptionTable.Rows[i + 9].SetField(4, model(i + 73, data[i + 73]));
            //    exceptionTable.Rows[i + 9].SetField(5, typeof(实验参数).GetField("B" + (i + 1) + "占空比").GetValue(null));
            //}
            //for (var i = 0; i < data.Length; i++)
            //{
            //    deviceTable.Rows[i].SetField(0, infos[i]);
            //    deviceTable.Rows[i].SetField(1, rate(i));
            //    deviceTable.Rows[i].SetField(2, DateTime.Now.ToString());
            //    deviceTable.Rows[i].SetField(3, (double)data[i] / rate(i));
            //    deviceTable.Rows[i].SetField(4, model(i, data[i]));
            //}
            String res = "";

            for (var i = 0; i < data.Length; i++)
            {
                res += infos[i] + ":" + (double)data[i] /(int)rateMethod.Invoke(typeInstance, new object[] { i }) + "," + (string)modelMethod.Invoke(typeInstance, new object[] { i, data[i]}) + "\r\n";
                Console.WriteLine(data[i]);
            }
            state = data[89];
            sample = data[84];
            tube = data[85];
            return res;
        }
        public static int state;
        public static int sample;
        public static int tube;
        public int rate(int i)
        {
            if (i < 9 || (i > 40 && i < 50))
                return 10;
            return 1;
        }

        public String model(int i, int data)
        {
            if (i < 9 || (i > 40 && i < 50) || (i > 83 && i < 87))
            {
                return "";
            }
            else if (i == 10 || i == 9 || i == 50 || i == 51)
            {
                if (data == 0)
                    return "关";
                else
                    return "开";
            }
            else if ((i > 10 && i < 14) || (i > 51 && i < 55) || (i > 22 && i < 41) || (i > 63 && i < 82))
            {
                if (data == 0)
                {
                    return "正常";
                }
                else
                    return "不正常";
            }
            else if ((i > 13 && i < 23) || (i > 54 && i < 64))
            {
                if (data != 0)
                {
                    return "加热";
                }
                else
                    return "未加热";

            }
            else if (i > 88 && i < 108)
            {
                if (data == 0)
                {
                    return "空白样";
                }
                else
                {
                    return "水样";
                }
            }
            else if (i > 107 && i < 126)
            {
                if (data == 0)
                {
                    return "启用";
                }
                else
                {
                    return "未启用";
                }
            }
            else if (i == 82)
            {
                switch (data)
                {
                    case 0: return "未滴定";
                }

            }
            else if (i == 87)
            {
                switch (data)
                {
                    case 0: return "不搅拌";
                }

            }
            else if (i == 88)
            {
                switch (data)
                {
                    case 0: return "不清除空白样";
                }

            }
            else if (i == 83)
            {
                switch (data)
                {
                    case 0: return "待机";


                }

            }

            return "";
        }

        public int readTwo(byte a, byte b)
        {
            return a * 256 + b;
        }

        public int[] readOneByOne(byte a, byte b)
        {
            int[] val = new int[16];
            for (var i = 0; i < 16; i++)
            {
                val[i] = (a * 256 + b) >> i & 1;
            }
            return val;
        }

    }

}
