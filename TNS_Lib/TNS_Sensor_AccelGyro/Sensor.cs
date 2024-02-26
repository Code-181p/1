using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TNS.Sensor.AccelGyro
{
    public class Sensor
    {
        private DateTime _Receive_Time;
        private DateTime _Receive_Time_Previous;
        private List<string> Packet_Buffer;
        private List<string> Log_Buffer;
        private int _Buffer_Max = 1000;
        private Timer TimerProcess;

        private Chart _Chart_Ax;
        private Chart _Chart_Ay;
        private Chart _Chart_Az;
        private Chart _Chart_Gx;
        private Chart _Chart_Gy;
        private Chart _Chart_Gz;
        private int _Chart_MaxItem = 300;

        public Sensor()
        {
            Accelerometer = new List<DataSensor>();
            Gyroscope = new List<DataSensor>();
            Magnetometer = new List<DataSensor>();
            Rotation = new List<DataSensor>();
            Chart_Init();
            Log_Buffer = new List<string>();
            TimerProcess = new Timer();
            TimerProcess.Interval = 1;

            TimerProcess.Tick += TimerProcess_Tick;
            TimerProcess.Start();
        }
        private void Chart_Init()
        {
            _Chart_Ax = new Chart(500, 200);
            _Chart_Ax.Title = "Accelerometer X";
            _Chart_Ax.AxisY_Maximum = 10;
            _Chart_Ax.AxisY_Minimum = -10;
            _Chart_Ax.AxisX_Maximum = 300;
            Series zTNS_Series = new Series("X value");
            zTNS_Series.ChartColor = "#ffa800";
            _Chart_Ax.Series.Add(zTNS_Series);
            _Chart_Ax.SetConfig();
            _Chart_Ax.View = "Box";

            _Chart_Ay = new Chart(500, 200);
            _Chart_Ay.Title = "Accelerometer Y";
            _Chart_Ay.AxisY_Maximum = 10;
            _Chart_Ay.AxisY_Minimum = -10;
            _Chart_Ay.AxisX_Maximum = 300;
            zTNS_Series = new Series("Y value");
            zTNS_Series.ChartColor = "#ffa800";
            zTNS_Series.Points_AddXY(0, 0);
            _Chart_Ay.Series.Add(zTNS_Series);
            _Chart_Ay.SetConfig();
            _Chart_Ay.View = "Box";

            _Chart_Az = new Chart(500, 200);
            _Chart_Az.Title = "Accelerometer Z";
            _Chart_Az.AxisY_Maximum = 20;
            _Chart_Az.AxisY_Minimum = 0;
            _Chart_Az.AxisX_Maximum = 300;
            zTNS_Series = new Series("Z value");
            zTNS_Series.ChartColor = "#ffa800";
            zTNS_Series.Points_AddXY(0, 0);
            _Chart_Az.Series.Add(zTNS_Series);
            _Chart_Az.SetConfig();
            _Chart_Az.View = "Box";


            _Chart_Gx = new Chart(500, 200);
            _Chart_Gx.Title = "Gyroscope X";
            _Chart_Gx.AxisY_Maximum = 100;
            _Chart_Gx.AxisY_Minimum = -100;
            _Chart_Gx.AxisX_Maximum = 300;
            zTNS_Series = new Series("X value");
            zTNS_Series.ChartColor = "#ffa800";
            zTNS_Series.Points_AddXY(0, 0);
            _Chart_Gx.Series.Add(zTNS_Series);
            _Chart_Gx.SetConfig();
            _Chart_Gx.View = "Box";

            _Chart_Gy = new Chart(500, 200);
            _Chart_Gy.Title = "Gyroscope Y";
            _Chart_Gy.AxisY_Maximum = 50;
            _Chart_Gy.AxisY_Minimum = -50;
            _Chart_Gy.AxisX_Maximum = 300;
            zTNS_Series = new Series("Y value");
            zTNS_Series.ChartColor = "#ffa800";
            zTNS_Series.Points_AddXY(0, 0);
            _Chart_Gy.Series.Add(zTNS_Series);
            _Chart_Gy.SetConfig();
            _Chart_Gy.View = "Box";

            _Chart_Gz = new Chart(500, 200);
            _Chart_Gz.Title = "Gyroscope Z";
            _Chart_Gz.AxisY_Maximum = 50;
            _Chart_Gz.AxisY_Minimum = -50;
            _Chart_Gz.AxisX_Maximum = 300;
            zTNS_Series = new Series("Z value");
            zTNS_Series.ChartColor = "#ffa800";
            zTNS_Series.Points_AddXY(0, 0);
            _Chart_Gz.Series.Add(zTNS_Series);
            _Chart_Gz.SetConfig();
            _Chart_Gz.View = "Box";
        }

        private void TimerProcess_Tick(object sender, EventArgs e)
        {
            TimerProcess.Stop();

            int n = Accelerometer.Count;
            int zBegin = 0;
            if (n > _Buffer_Max)
            {
                zBegin = n - _Chart_MaxItem;
                string zData = "";
                for (int i = 0; i < zBegin; i++)
                {
                    zData += Accelerometer[i].TimeData.ToString("yyMMddHHmmssfff")
                        + "|" + Accelerometer[i].X.ToString()
                        + ":" + Accelerometer[i].Y.ToString()
                        + ":" + Accelerometer[i].Z.ToString()
                        + ";";
                }
                SaveData(zData);
                Accelerometer.RemoveRange(0, zBegin);
                Gyroscope.RemoveRange(0, zBegin);
            }
           
            TimerProcess.Start();
        }
        private void SaveData(string Data)
        {
            FileStream zStream;
            try
            {

                StringBuilder zContent = new StringBuilder();
                zStream = new FileStream(@"Data/MPU6050/" + DateTime.Now.ToString("yyMMddHHmmss") + ".txt", FileMode.Create);
                StreamWriter zWrite = new StreamWriter(zStream, Encoding.UTF8);
                zWrite.Write(Data);
                zWrite.Flush();
                zWrite.Close();

                Message = "OK";

            }
            catch (Exception ex)
            {
                Message = "ER" + ex.ToString();
            }
        }
        public void PacketComming(List<byte> PackData)
        {
            /*string zData = "";
            foreach (byte b in PackData)
            {
                zData += Convert.ToChar(b).ToString();
            }
            Packet_Buffer.Add("->" + zData);
            */
            if (PackData.Count >= 4)
            {
                int k = 0;
                string strValue = "";
                //Header
                char zAddressDevice = (char)PackData[0];
                char zHeader = (char)PackData[1];
                if (zHeader == 'I')
                {
                    string zStrValue = zAddressDevice.ToString() + ":";
                    int n = PackData.Count;
                    for (int i = 2; i < n; i++)
                    {
                        zStrValue += (char)PackData[i];
                    }
                    Log_Buffer.Add("[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] " + zStrValue);
                }
                if (zHeader == 'D')
                {
                    k = 2;
                    float[] zValues = new float[6];
                    if (PackData.Count >= 26)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            byte[] zResult = new byte[4];
                            zResult[0] = PackData[k];
                            zResult[1] = PackData[k + 1];
                            zResult[2] = PackData[k + 2];
                            zResult[3] = PackData[k + 3];
                            zValues[i] = BitConverter.ToSingle(zResult, 0);
                            k = k + 4;

                            strValue += zValues[i].ToString() + ";";
                        }
                        float[] zAccelerometer = new float[3];
                        zAccelerometer[0] = zValues[0];
                        zAccelerometer[1] = zValues[1];
                        zAccelerometer[2] = zValues[2];

                        TNS.Sensor.AccelGyro.DataSensor zDataItem = new TNS.Sensor.AccelGyro.DataSensor(zAccelerometer);
                        Accelerometer.Add(zDataItem);

                        float[] zGyroscope = new float[3];
                        zGyroscope[0] = zValues[3];
                        zGyroscope[1] = zValues[4];
                        zGyroscope[2] = zValues[5];
                        zDataItem = new TNS.Sensor.AccelGyro.DataSensor(zGyroscope);
                        Gyroscope.Add(zDataItem);

                        Log_Buffer.Add("A[" + zAccelerometer[0].ToString("#,##0.00") + " : " + zAccelerometer[1].ToString("#,##0.00") + " : " + zAccelerometer[2].ToString("#,##0.00") + "]");
                        Log_Buffer.Add("G[" + zGyroscope[0].ToString("#,##0.00") + " : " + zGyroscope[1].ToString("#,##0.00") + " : " + zGyroscope[2].ToString("#,##0.00") + "]");

                    }
                }
                if (zHeader == 'T')
                {
                    string zStrValue = zAddressDevice.ToString() + ":";
                    int n = PackData.Count;
                    for (int i = 2; i < n; i++)
                    {
                        zStrValue += (char)PackData[i];
                    }
                    string[] zValues = zStrValue.Split(';');
                    float[] zAccelerometer = new float[3];
                    zAccelerometer[0] = float.Parse(zValues[1]);
                    zAccelerometer[1] = float.Parse(zValues[1]);
                    zAccelerometer[2] = float.Parse(zValues[2]);

                    TNS.Sensor.AccelGyro.DataSensor zItem = new TNS.Sensor.AccelGyro.DataSensor(DateTime.Now, zAccelerometer);
                    Accelerometer.Add(zItem);

                    float[] zGyroscope = new float[3];
                    zGyroscope[0] = float.Parse(zValues[3]);
                    zGyroscope[1] = float.Parse(zValues[4]);
                    zGyroscope[2] = float.Parse(zValues[5]);
                    zItem = new TNS.Sensor.AccelGyro.DataSensor(DateTime.Now, zGyroscope);
                    Gyroscope.Add(zItem);

                    // _Log_Buffer.Add("[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] " + zStrValue);
                }
            }

        }
       
        public void Chart_UpdateData()
        {
            int n = Accelerometer.Count;
            if (n > 0)
            {
                DataSensor zItem;
                int zBegin = 0;
                if (n > _Chart_MaxItem)
                    zBegin = n - _Chart_MaxItem;

                _Chart_Ax.Series[0].Points_RemoveAll();
                _Chart_Ay.Series[0].Points_RemoveAll();
                _Chart_Az.Series[0].Points_RemoveAll();
                _Chart_Gx.Series[0].Points_RemoveAll();
                _Chart_Gy.Series[0].Points_RemoveAll();
                _Chart_Gz.Series[0].Points_RemoveAll();

                zItem = Accelerometer[zBegin];
                _Chart_Ax.Series[0].Points_AddXY(0, (float)zItem.X);
                _Chart_Ay.Series[0].Points_AddXY(0, (float)zItem.Y);
                _Chart_Az.Series[0].Points_AddXY(0, (float)zItem.Z);

                zItem = Gyroscope[zBegin];
                _Chart_Gx.Series[0].Points_AddXY(0, (float)zItem.X);
                _Chart_Gy.Series[0].Points_AddXY(0, (float)zItem.Y);
                _Chart_Gz.Series[0].Points_AddXY(0, (float)zItem.Z);

                for (int i = zBegin + 1; i < n; i++)
                {
                    zItem = Accelerometer[i];
                    _Chart_Ax.Series[0].Points_AddY((float)zItem.X);
                    _Chart_Ay.Series[0].Points_AddY((float)zItem.Y);
                    _Chart_Az.Series[0].Points_AddY((float)zItem.Z);

                    zItem = Gyroscope[i];
                    _Chart_Gx.Series[0].Points_AddY((float)zItem.X);
                    _Chart_Gy.Series[0].Points_AddY((float)zItem.Y);
                    _Chart_Gz.Series[0].Points_AddY((float)zItem.Y);
                }
            }
        }
        
        public Image Chart_Ax(string StyleView)
        {
            _Chart_Ax.View = StyleView;
            return _Chart_Ax.Display();
        }
        public Image Chart_Ay(string StyleView)
        {
            _Chart_Ay.View = StyleView;
            return _Chart_Ay.Display();
        }
        public Image Chart_Az(string StyleView)
        {
            _Chart_Az.View = StyleView;
            return _Chart_Az.Display();
        }
        public Image Chart_Gx(string StyleView)
        {
            _Chart_Gx.View = StyleView;
            return _Chart_Gx.Display();
        }
        public Image Chart_Gy(string StyleView)
        {
            _Chart_Ay.View = StyleView;
            return _Chart_Gy.Display();
        }
        public Image Chart_Gz(string StyleView)
        {
            _Chart_Az.View = StyleView;
            return _Chart_Gz.Display();
        }

        #region [ Properities ] 
        public int Buffer_Max { set { _Buffer_Max = value; } }
        public List<DataSensor> Accelerometer { get; set; }
        public List<DataSensor> Gyroscope { get; set; }
        public List<DataSensor> Magnetometer { get; set; }
        public List<DataSensor> Rotation { get; set; }

        public string Message { get; set; }

        #endregion

    }
}
