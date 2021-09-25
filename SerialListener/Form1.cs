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
using NationalInstruments.DAQmx;
using NationalInstruments;

namespace SerialListener
{
    public partial class Form1 : Form
    {   
        
        private AnalogMultiChannelReader analogInReader;
        private NationalInstruments.DAQmx.Task myTask;
        private NationalInstruments.DAQmx.Task runningTask;
        private AsyncCallback analogCallback;
        private AnalogWaveform<double>[] data;

        public Form1()
        {
            InitializeComponent();
        }


        RemoteDataBase dB;// = RemoteDataBase.Instance();
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //dB.DatabaseName = "事务管理";
            if (Listener.check == 6)
            {
                Listener.initReadParm();
                showData(stateCode);
            }
            else {
                Listener.start(textBox1);
            }
            
        }
        MySqlCommand cmd;

        private void insertData(AnalogWaveform<double>[] sourceArray)
        {
            dB.IsConnect(Listener.serverIp);
            {
                cmd = dB.Connection.CreateCommand();
                string cmdText = "";
                string cmdTextBefore="";
                var waveform = sourceArray[0];
                DateTime time = DateTime.Now;              
                string format = "yyyy-MM-dd HH:mm:ss";   
                var mytime = time.ToString(format);
                cmdTextBefore="insert into info (Data ,time) values ("+ waveform.Samples[1].Value+",\""+mytime+"\");";

                if (waveform.SampleCount >= 2) {
                    cmdText = "(" + waveform.Samples[1].Value + ")";
                }
                for (int i=2;i<waveform.SampleCount;i++){
                    cmdText += ",(" + waveform.Samples[i].Value + ")";
                }
                
                cmd.CommandText = cmdTextBefore+"insert into info (Data) values "+cmdText;
                //dB.Connection.Close();
                cmd.ExecuteNonQuery();
                //dB.Connection.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            return;
        }
        int stateCode;
        int again;
        private void showData(int state)
        {
            //dataTable = new DataTable();
            if (runningTask == null)
            {
                try
                {
                    // Create a new task
                    myTask = new NationalInstruments.DAQmx.Task();
                    //myTask.AIChannels.CreateV

                    // myTask.Stream.Buffer.InputBufferSize = 3000;

                    // Create a virtual channel
                    myTask.AIChannels.CreateVoltageChannel(Listener.dev, String.Empty,
                        AITerminalConfiguration.Rse, -10,
                        10, AIVoltageUnits.Volts);

                    // Configure the timing parameters
                    myTask.Timing.ConfigureSampleClock(String.Empty, 100,
                        SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 100);

                    // Verify the Task
                    myTask.Control(TaskAction.Verify);

                    // Prepare the table for Data
                    //InitializeDataTable(myTask.AIChannels, ref dataTable);
                    //acquisitionDataGrid.DataSource = dataTable;

                    runningTask = myTask;
                    analogInReader = new AnalogMultiChannelReader(myTask.Stream);

                    analogCallback = new AsyncCallback(AnalogInCallback);

                    // Use SynchronizeCallbacks to specify that the object 
                    analogInReader.SynchronizeCallbacks = true;
                    analogInReader.BeginReadWaveform(100,
                        analogCallback, myTask);
                }
                catch (DaqException)
                {
                    // Display Errors
                    //MessageBox.Show(exception.Message);
                    //MessageBox.Show("请重新点开始");
                    runningTask = null;
                    myTask.Dispose();
                    if (again++ > 10)
                    {
                        MessageBox.Show("usb设备连接错误,请重插后,点开始启动");
                        return;
                    }
                    System.Threading.Thread.Sleep(600);
                    showData(stateCode);
                }
            }
        }
        private void AnalogInCallback(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the available data from the channels
                    data = analogInReader.EndReadWaveform(ar);

                    // Plot your data here
                    insertData(data);

                    analogInReader.BeginMemoryOptimizedReadWaveform(100,
                        analogCallback, myTask, data);

                }
            }
            catch (DaqException)
            {
                // Display Errors
                //MessageBox.Show(exception.Message);
                runningTask = null;
                myTask.Dispose();
                if (again++ > 10)
                {
                    MessageBox.Show("usb设备连接错误,请重插后,点开始启动");
                    return;
                }
                System.Threading.Thread.Sleep(600);
                //await: 100;
                showData(stateCode);

            }
        }
        
    }
}
