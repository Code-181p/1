using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS.Sensor.AccelGyro
{
    public class Series
    {
        public string Title { get; set; }
        public string ChartColor { get; set; }
        public string ChartType { get; set; }
        public string X_ValueType { get; set; }
        public List<float> Points_Y { get; private set; }
        public List<float> Points_X { get; private set; }
        public List<DateTime> Points_X_Object { get; private set; }
        public Pen Pen { get { return new Pen(ColorTranslator.FromHtml(ChartColor)); } }
        public SolidBrush Brush { get { return new SolidBrush(ColorTranslator.FromHtml(ChartColor)); } }
        public Series(string title)
        {
            Points_X = new List<float>();
            Points_X_Object = new List<DateTime>();
            Points_Y = new List<float>();
            ChartColor = "#f9ff5b";
            ChartType = "Line";
            Title = title;
        }
        public void Points_AddY(float Y)
        {
            Points_X.Add(Points_X[Points_X.Count - 1] + 1);
            Points_Y.Add(Y);
        }
        public void Points_AddXY(float X, float Y)
        {
            Points_X.Add(X);
            Points_Y.Add(Y);
        }
        public void Points_AddXY(DateTime X, float Y)
        {
            Points_X_Object.Add(X);
            Points_Y.Add(Y);
        }
        public void Points_NewArray(List<float> Y)
        {
            Points_RemoveAll();
            for (int i = 0; i < Y.Count; i++)
            {
                Points_X.Add(i + 1);
                Points_Y.Add(Y[i]);
            }
        }
        public int Points_Count
        {
            get { return Points_X.Count; }
        }
        public void Points_RemoveAt(int Index)
        {
            Points_X.RemoveAt(Index);
            Points_Y.RemoveAt(Index);
            if (Points_X_Object.Count > Index)
                Points_X_Object.RemoveAt(Index);
        }
        public void Points_RemoveAll()
        {
            Points_X = new List<float>();
            Points_Y = new List<float>();
            Points_X_Object = new List<DateTime>();
        }
        public void Points_Limit(int Amount)
        {
            int zCount = 0;
            int n = Points_X.Count;
            if (n >= Amount)
            {
                zCount = n - Amount;
                Points_X.RemoveRange(0, zCount);
                Points_Y.RemoveRange(0, zCount);
                if (Points_X_Object.Count > zCount)
                    Points_X_Object.RemoveRange(0, zCount);

            }

        }

    }
}
