using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS.Sensor.AccelGyro
{
    public class DataSensor
    {
        private float _X;
        private float _Y;
        private float _Z;
        private DateTime _TimeData;
        public DataSensor()
        {
            _X = 0;
            _Y = 0;
            _Z = 0;
            _TimeData = DateTime.Now;
        }
        public DataSensor(float x, float y, float z)
        {
            _X = x;
            _Y = y;
            _Z = z;
            _TimeData = DateTime.Now;
        }
        public DataSensor(float[] data)
        {
            _X = data[0];
            _Y = data[1];
            _Z = data[2];
            _TimeData = DateTime.Now;
        }
        public DataSensor(DateTime TimeCollect, float[] data)
        {
            _X = data[0];
            _Y = data[1];
            _Z = data[2];
            _TimeData = TimeCollect;
        }
        public DateTime TimeData
        {
            get { return _TimeData; }
        }
         public float X
        {
            set { _X = value; }
            get { return _X; }
        }
        public float Y
        {
            set { _Y = value; }
            get { return _Y; }
        }
        public float Z
        {
            set { _Z = value; }
            get { return _Z; }
        }
        public float Roll
        {
            set { _X = value; }
            get { return _X; }
        }
        public float Pitch
        {
            set{ _Y = value;}
            get { return _Y; }
        }
        public float Yaw
        {
            set { _Z = value; }
            get { return _Z; }
        }
        public void ComputerPRY()
        {
            double zPitch = -(Math.Atan2(_X, Math.Sqrt(_Y * _Y + _Z * _Z)) * 180.0) / Math.PI;
            double zRoll = (Math.Atan2(_Y, _Z) * 180.0) / Math.PI;
        }
         
    }
}
