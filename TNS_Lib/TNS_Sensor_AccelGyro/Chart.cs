using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS.Sensor.AccelGyro
{
    public class Chart
    {
        public string Title { get; set; }
        public float AxisY_Maximum { get; set; }
        public float AxisY_Minimum { get; set; }
        public float AxisX_Maximum { get; set; }
        public float AxisX_Minimum { get; set; }

        public int Chart_Margin_Left { get; set; }
        public int Chart_Margin_Top { get; set; }

        private int _Width;
        private int _Height;

        private Point _Title_Point;
        private int _Chart_Width;
        private float _Chart_Width_Unit;
        private int _Chart_Height;
        private float _Chart_Height_Unit;
        private PointF Oxy;
        private PointF Oy;
        private PointF Ox;
        public List<Series> Series { get; set; }
        public string View { set { _View = value; } }
        private string _View = "Coordinate";
        public Chart()
        {
            AxisY_Maximum = 10;
            AxisY_Minimum = 0;
            Series = new List<Series>();
        }
        public Chart(int width, int height)
        {
            _Width = width;
            _Height = height;

            AxisY_Maximum = 10;
            AxisY_Minimum = -10;

            AxisX_Maximum = 100;
            AxisX_Minimum = 0;

            Series = new List<Series>();

            _Title_Point = new Point(_Width / 2, 10);
            Chart_Margin_Left = 150;
            Chart_Margin_Top = _Title_Point.Y + 20;

            _Chart_Height = _Height - Chart_Margin_Top - 30;
            _Chart_Height_Unit = (float)_Chart_Height / (AxisY_Maximum - AxisY_Minimum);
            _Chart_Width = _Width - Chart_Margin_Left - 20;
            _Chart_Width_Unit = (float)_Chart_Width / (AxisX_Maximum - AxisX_Minimum);

            Oxy = new PointF(Chart_Margin_Left, (_Chart_Height_Unit * AxisY_Maximum) + Chart_Margin_Top);
            Oy = new PointF(Oxy.X, Oxy.Y - (_Chart_Height_Unit * AxisY_Maximum));
            Ox = new PointF(Oxy.X + _Chart_Width, Oxy.Y);
        }
        public void SetConfig()
        {
            _Title_Point = new Point(_Width / 2, 10);
            Chart_Margin_Left = 50;

            _Chart_Height = _Height - Chart_Margin_Top - 30;
            _Chart_Height_Unit = (float)_Chart_Height / (AxisY_Maximum - AxisY_Minimum);
            _Chart_Width = _Width - Chart_Margin_Left - 20;
            _Chart_Width_Unit = (float)_Chart_Width / (AxisX_Maximum - AxisX_Minimum);

            Oxy = new PointF(Chart_Margin_Left, (_Chart_Height_Unit * AxisY_Maximum) + Chart_Margin_Top);
            Oy = new PointF(Oxy.X, Oxy.Y - (_Chart_Height_Unit * AxisY_Maximum));
            Ox = new PointF(Oxy.X + _Chart_Width, Oxy.Y);
        }
        public Image Display()
        {
            Graphics g;
            Bitmap zDrawing = null;
            zDrawing = new Bitmap(_Width, _Height);
            g = Graphics.FromImage(zDrawing);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            //================================================
            //g.DrawRectangle(Pens.White, 0, 0, zDrawing.Width - 1, zDrawing.Height - 1);

            PointF zPoint_Value, zPoint_Draw = new PointF(0, 0);
            float zUnitBlocl_Y = (AxisY_Maximum - AxisY_Minimum) / 10;
            float zUnitBlocl_X = (AxisX_Maximum - AxisX_Minimum) / 10;
            Font Y_Value_Font = new System.Drawing.Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Pixel);

            #region [ Title ]
            Font zTitle_Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            StringFormat zFormat = new StringFormat();
            zFormat.Alignment = StringAlignment.Center;
            g.DrawString(Title, zTitle_Font, Brushes.White, _Title_Point, zFormat);
            #endregion

            #region [ Axis XY ]
            if (_View == "Coordinate")
            {
                Font zAxis_Value_Font = new System.Drawing.Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Pixel);
                Pen Pen_Oxy = new Pen(Color.FromArgb(200, 212, 212, 212), 1.0F);
                string zLableY_Format = "#,##0";
                if (AxisY_Maximum <= 1)
                    zLableY_Format = "##0.0";

                g.FillEllipse(Brushes.White, Oxy.X - 2, Oxy.Y - 2, 4, 4);
                //g.FillEllipse(Brushes.White, Oy.X - 2, Oy.Y - 2, 4, 4);
                //g.FillEllipse(Brushes.White, Ox.X - 2, Ox.Y - 2, 4, 4);
                g.DrawLine(Pen_Oxy, Oy.X, Oy.Y, Oy.X, Oy.Y + _Chart_Height);
                g.DrawLine(Pen_Oxy, Oxy, Ox);

                zFormat.Alignment = StringAlignment.Far;
                g.DrawLine(Pen_Oxy, Oy.X - 3, Oy.Y, Oy.X + 3, Oy.Y);
                g.DrawLine(Pen_Oxy, Oy.X - 3, Oy.Y + _Chart_Height, Oy.X + 3, Oy.Y + _Chart_Height);
                g.DrawString(AxisY_Maximum.ToString(zLableY_Format), zAxis_Value_Font, Brushes.White, Oy.X - 3, Oy.Y - 5, zFormat);
                g.DrawString(AxisY_Minimum.ToString(zLableY_Format), zAxis_Value_Font, Brushes.White, Oy.X - 3, Oy.Y + _Chart_Height - 5, zFormat);

                zPoint_Value = new PointF(0, zUnitBlocl_Y);
                while (zPoint_Value.Y < AxisY_Maximum)
                {
                    zPoint_Draw = new PointF(Oxy.X, Oxy.Y - (zPoint_Value.Y * _Chart_Height_Unit));
                    g.DrawLine(Pen_Oxy, zPoint_Draw.X - 2, zPoint_Draw.Y, zPoint_Draw.X + 2, zPoint_Draw.Y);
                    g.DrawString(zPoint_Value.Y.ToString(zLableY_Format), zAxis_Value_Font, Brushes.White, zPoint_Draw.X - 2, zPoint_Draw.Y - 5, zFormat);
                    zPoint_Value.Y += zUnitBlocl_Y;
                }

                zPoint_Value = new PointF(0, -zUnitBlocl_Y);
                while (zPoint_Value.Y > AxisY_Minimum)
                {
                    zPoint_Draw = new PointF(Oxy.X, Oxy.Y - (zPoint_Value.Y * _Chart_Height_Unit));
                    g.DrawLine(Pen_Oxy, zPoint_Draw.X - 2, zPoint_Draw.Y, zPoint_Draw.X + 2, zPoint_Draw.Y);
                    g.DrawString(zPoint_Value.Y.ToString(zLableY_Format), zAxis_Value_Font, Brushes.White, zPoint_Draw.X - 2, zPoint_Draw.Y - 5, zFormat);
                    zPoint_Value.Y -= zUnitBlocl_Y;
                }

                zPoint_Value = new PointF(0, zUnitBlocl_X);
                zFormat.Alignment = StringAlignment.Center;
                while (zPoint_Value.X <= AxisX_Maximum)
                {
                    zPoint_Draw = new PointF(Oxy.X + (zPoint_Value.X * _Chart_Width_Unit), Oxy.Y);
                    g.DrawLine(Pen_Oxy, zPoint_Draw.X, zPoint_Draw.Y - 2, zPoint_Draw.X, zPoint_Draw.Y + 2);
                    zPoint_Value.X += zUnitBlocl_X;
                }

            }
            #endregion

            #region [ Box ]
            if (_View == "Box")
            {
                int zLend = 10;
                Pen Pen_Grid = new Pen(Color.FromArgb(100, 212, 212, 212), 1.0F);
                PointF[] zBoxPoints = new PointF[8];
                zBoxPoints[0] = new PointF(zLend, 3);
                zBoxPoints[1] = new PointF(0, zLend);
                zBoxPoints[2] = new PointF(0, zDrawing.Height - zLend);
                zBoxPoints[3] = new PointF(zLend, zDrawing.Height - 4);
                zBoxPoints[4] = new PointF(zDrawing.Width - zLend - 1, zDrawing.Height - 4);
                zBoxPoints[5] = new PointF(zDrawing.Width - 1, zDrawing.Height - zLend);
                zBoxPoints[6] = new PointF(zDrawing.Width - 1, zLend);
                zBoxPoints[7] = new PointF(zDrawing.Width - zLend - 1, 3);

                g.DrawLine(new Pen(Color.White, 2), zBoxPoints[0], zBoxPoints[1]);
                g.DrawLine(new Pen(Color.White, 1), zBoxPoints[1], zBoxPoints[2]);
                g.DrawLine(new Pen(Color.White, 2), zBoxPoints[2], zBoxPoints[3]);
                //g.FillEllipse(Brushes.White, zBoxPoints[0].X - 3, zBoxPoints[0].Y - 3, 6, 6);
                //g.FillEllipse(Brushes.White, zBoxPoints[3].X - 3, zBoxPoints[3].Y - 3, 6, 6);

                g.DrawLine(new Pen(Color.White, 2), zBoxPoints[4], zBoxPoints[5]);
                g.DrawLine(new Pen(Color.White, 1), zBoxPoints[5], zBoxPoints[6]);
                g.DrawLine(new Pen(Color.White, 2), zBoxPoints[6], zBoxPoints[7]);
                //g.FillEllipse(Brushes.White, zBoxPoints[4].X - 3, zBoxPoints[4].Y - 3, 6, 6);
                //g.FillEllipse(Brushes.White, zBoxPoints[7].X - 3, zBoxPoints[7].Y - 3, 6, 6);

                g.FillPolygon(new SolidBrush(Color.FromArgb(30, Color.White)), zBoxPoints);

                zPoint_Value = new PointF(0, 0);
                while (zPoint_Value.Y <= AxisY_Maximum)
                {
                    zPoint_Draw = new PointF(Oxy.X, Oxy.Y - (zPoint_Value.Y * _Chart_Height_Unit));
                    g.DrawLine(Pen_Grid, zPoint_Draw.X, zPoint_Draw.Y, zPoint_Draw.X + _Chart_Width, zPoint_Draw.Y);
                    zPoint_Value.Y += zUnitBlocl_Y;
                }

                zPoint_Value = new PointF(0, -zUnitBlocl_Y);
                zPoint_Draw = new PointF(Oxy.X, Oxy.Y);
                while (zPoint_Value.Y >= AxisY_Minimum)
                {
                    zPoint_Draw = new PointF(Oxy.X, Oxy.Y - (zPoint_Value.Y * _Chart_Height_Unit));
                    g.DrawLine(Pen_Grid, zPoint_Draw.X, zPoint_Draw.Y, zPoint_Draw.X + _Chart_Width, zPoint_Draw.Y);
                    zPoint_Value.Y -= zUnitBlocl_Y;
                }
                float zOy_Down = zPoint_Draw.Y;
                zPoint_Value = new PointF(0, zUnitBlocl_X);
                zFormat.Alignment = StringAlignment.Center;
                while (zPoint_Value.X <= AxisX_Maximum)
                {
                    zPoint_Draw = new PointF(Oxy.X + (zPoint_Value.X * _Chart_Width_Unit), zOy_Down);
                    g.DrawLine(Pen_Grid, zPoint_Draw.X, Oy.Y, zPoint_Draw.X, zPoint_Draw.Y);
                    zPoint_Value.X += zUnitBlocl_X;
                }
            }
            #endregion

            #region [ View Data ]  
            zFormat.Alignment = StringAlignment.Far;
            int k = 0;
            foreach (Series zSeries in Series)
            {
                int n = zSeries.Points_Count;
                if (zSeries.ChartType == "Line")
                {
                    PointF[] zPoints_Draw = new PointF[n];
                    for (int i = 0; i < n; i++)
                    {
                        zPoint_Draw = new PointF();
                        zPoint_Draw.X = Oxy.X + ((zSeries.Points_X[i] - zSeries.Points_X[0]) * _Chart_Width_Unit);
                        zPoint_Draw.Y = Oxy.Y - (zSeries.Points_Y[i] * _Chart_Height_Unit);
                        zPoints_Draw[i] = zPoint_Draw;
                    }
                    if (n > 1)
                        g.DrawLines(zSeries.Pen, zPoints_Draw);

                    if (_View == "Box")
                    {
                        if (n > 1)
                        {
                            g.DrawString(zSeries.Points_Y[n - 1].ToString("#,##0.00"), Y_Value_Font, zSeries.Brush, _Width - 20 - (k * 200), _Height - 25, zFormat);
                        }
                    }
                }
                k++;
            }

            //g.DrawLine(Pens.Orange, Oy.X, Oy.Y, Oy.X, Oy.Y + _Chart_Height);
            //g.DrawLine(Pens.Orange, Oxy, Ox);
            #endregion

            //==============================================
            g.Dispose();
            return zDrawing;
        }

    }
}
