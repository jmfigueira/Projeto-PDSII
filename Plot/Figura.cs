using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Plot
{
    public class Figura
    {
        private Point graphPos = new Point(0, 0);

        private Bitmap bmpFrame;
        private Graphics gfxFrame;

        private Bitmap bmpGraph { get; set; }
        public Graphics gfxGraph;
        public Eixo xAxis = new Eixo(-10, 10, 100, false);
        public Eixo yAxis = new Eixo(-10, 10, 100, true);

        public Color colorFigBg;
        public Color colorGraphBg;
        public Color colorAxis;
        public Color colorGridLines;

        const string font = "Arial";
        readonly Font fontTicks = new Font(font, 9, FontStyle.Regular);
        readonly Font fontTitle = new Font(font, 20, FontStyle.Bold);
        readonly Font fontAxis = new Font(font, 12, FontStyle.Bold);

        public string labelY = "";
        public string labelX = "";
        public string labelTitle = "";

        private int padL = 50, padT = 47, padR = 50, padB = 47;

        private System.Diagnostics.Stopwatch stopwatch;

        public Figura(int width, int height)
        {
            stopwatch = System.Diagnostics.Stopwatch.StartNew();
            stopwatch.Stop();
            stopwatch.Reset();

            styleWeb();
            Resize(width, height);

            gfxGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfxFrame.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfxFrame.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            FrameRedraw();
            GraphClear();
        }


        public void Resize(int width, int height)
        {
            if (width - padL - padR < 1) width = padL + padR + 1;
            if (height - padT - padB < 1) height = padT + padB + 1;

            bmpFrame = new Bitmap(width, height);
            gfxFrame = Graphics.FromImage(bmpFrame);

            FramePad(null, null, null, null);

            bmpGraph = new Bitmap(bmpFrame.Width - padL - padR, bmpFrame.Height - padT - padB);
            gfxGraph = Graphics.FromImage(bmpGraph);

            xAxis.Resize(bmpGraph.Width);
            yAxis.Resize(bmpGraph.Height);
        }

        public void FramePad(int? left, int? right, int? top, int? bottom)
        {
            if (left != null) padL = (int)left;
            if (right != null) padR = (int)right;
            if (top != null) padT = (int)top;
            if (bottom != null) padB = (int)bottom;
            graphPos = new Point(padL, padT);
        }

        public void FrameRedraw()
        {

            gfxFrame.Clear(colorFigBg);

            Pen penAxis = new Pen(new SolidBrush(colorAxis));
            Pen penGrid = new Pen(colorGridLines) { DashPattern = new float[] { 4, 4 } };
            Brush brush = new SolidBrush(colorAxis);
            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            StringFormat sfRight = new StringFormat();
            sfRight.Alignment = StringAlignment.Far;
            int posB = bmpGraph.Height + padT;
            int posCx = bmpGraph.Width / 2 + padL;
            int posCy = bmpGraph.Height / 2 + padT;

            int tick_size_minor = 2;
            int tick_size_major = 5;

            gfxFrame.DrawRectangle(penAxis, graphPos.X - 1, graphPos.Y - 1, bmpGraph.Width + 1, bmpGraph.Height + 1);
            gfxFrame.FillRectangle(new SolidBrush(colorGraphBg), graphPos.X, graphPos.Y, bmpGraph.Width, bmpGraph.Height);
            foreach (Eixo.Tick tick in xAxis.ticksMajor)
                gfxFrame.DrawLine(penAxis, new Point(padL + tick.posPixel, posB + 1), new Point(padL + tick.posPixel, posB + 1 + tick_size_minor));
            foreach (Eixo.Tick tick in yAxis.ticksMajor)
                gfxFrame.DrawLine(penAxis, new Point(padL - 1, padT + tick.posPixel), new Point(padL - 1 - tick_size_minor, padT + tick.posPixel));
            foreach (Eixo.Tick tick in xAxis.ticksMinor)
            {
                gfxFrame.DrawLine(penGrid, new Point(padL + tick.posPixel, padT), new Point(padL + tick.posPixel, padT + bmpGraph.Height - 1));
                gfxFrame.DrawLine(penAxis, new Point(padL + tick.posPixel, posB + 1), new Point(padL + tick.posPixel, posB + 1 + tick_size_major));
                gfxFrame.DrawString(tick.label, fontTicks, brush, new Point(tick.posPixel + padL, posB + 7), sfCenter);
            }
            foreach (Eixo.Tick tick in yAxis.ticksMinor)
            {
                gfxFrame.DrawLine(penGrid, new Point(padL, padT + tick.posPixel), new Point(padL + bmpGraph.Width, padT + tick.posPixel));
                gfxFrame.DrawLine(penAxis, new Point(padL - 1, padT + tick.posPixel), new Point(padL - 1 - tick_size_major, padT + tick.posPixel));
                gfxFrame.DrawString(tick.label, fontTicks, brush, new Point(padL - 6, tick.posPixel + padT - 7), sfRight);
            }

            gfxFrame.DrawString(labelX, fontAxis, brush, new Point(posCx, posB + 24), sfCenter);
            gfxFrame.DrawString(labelTitle, fontTitle, brush, new Point(bmpFrame.Width / 2, 8), sfCenter);
            gfxFrame.TranslateTransform(gfxFrame.VisibleClipBounds.Size.Width, 0);
            gfxFrame.RotateTransform(-90);
            gfxFrame.DrawString(labelY, fontAxis, brush, new Point(-posCy, -bmpFrame.Width + 2), sfCenter);
            gfxFrame.ResetTransform();

            GraphClear();
        }

        public void GraphClear()
        {
            gfxGraph.DrawImage(bmpFrame, new Point(-padL, -padT));
            pointCount = 0;
        }

        private long pointCount = 0;
        private string benchmarkMessage
        {
            get
            {
                double ms = this.stopwatch.ElapsedTicks * 1000.0 / System.Diagnostics.Stopwatch.Frequency;
                double hz = 1.0 / ms * 1000.0;
                string msg = "";
                double imageSizeMB = bmpFrame.Width * bmpFrame.Height * 4.0 / 1024 / 1024;
                msg += string.Format("{0} x {1} ({2:0.00} MB) ", bmpFrame.Width, bmpFrame.Height, imageSizeMB);
                msg += string.Format("with {0:n0} data points rendered in ", pointCount);
                msg += string.Format("{0:0.00 ms} ({1:0.00} Hz)", ms, hz);
                return msg;
            }
        }
        public Bitmap Render()
        {
            Bitmap bmpMerged = new Bitmap(bmpFrame);
            Graphics gfx = Graphics.FromImage(bmpMerged);
            gfx.DrawImage(bmpGraph, graphPos);

            if (this.stopwatch.ElapsedTicks > 0)
            {
                Font fontStamp = new Font(font, 8, FontStyle.Regular);
                SolidBrush brushStamp = new SolidBrush(colorAxis);
                Point pointStamp = new Point(bmpFrame.Width - padR - 2, bmpFrame.Height - padB - 14);
                StringFormat sfRight = new StringFormat();
                sfRight.Alignment = StringAlignment.Far;
                gfx.DrawString(benchmarkMessage, fontStamp, brushStamp, pointStamp, sfRight);

            }

            return bmpMerged;
        }

        public void AxisSet(double? x1, double? x2, double? y1, double? y2)
        {
            if (x1 != null) xAxis.min = (double)x1;
            if (x2 != null) xAxis.max = (double)x2;
            if (y1 != null) yAxis.min = (double)y1;
            if (y2 != null) yAxis.max = (double)y2;
            if (x1 != null || x2 != null) xAxis.RecalculateScale();
            if (y1 != null || y2 != null) yAxis.RecalculateScale();
            if (x1 != null || x2 != null || y1 != null || y2 != null) FrameRedraw();
        }

        public void Zoom(double? xFrac, double? yFrac)
        {
            if (xFrac != null) xAxis.Zoom((double)xFrac);
            if (yFrac != null) yAxis.Zoom((double)yFrac);
            FrameRedraw();
        }

        public void styleWeb()
        {
            colorFigBg = Color.White;
            colorGraphBg = Color.FromArgb(255, 235, 235, 235);
            colorAxis = Color.Black;
            colorGridLines = Color.LightGray;
        }

        public void styleForm()
        {
            colorFigBg = SystemColors.Control;
            colorGraphBg = Color.White;
            colorAxis = Color.Black;
            colorGridLines = Color.LightGray;
        }

        public void BenchmarkThis(bool enable = true)
        {
            if (enable)
            {
                stopwatch.Restart();
            }
            else
            {
                stopwatch.Stop();
                stopwatch.Reset();
            }
        }

        private Point[] PointsFromArrays(double[] Xs, double[] Ys)
        {
            int pointCount = Math.Min(Xs.Length, Ys.Length);
            Point[] points = new Point[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                points[i] = new Point(xAxis.GetPixel(Xs[i]), yAxis.GetPixel(Ys[i]));
            }
            return points;
        }

        public void PlotLines(double[] Xs, double[] Ys, float lineWidth = 1, Color? lineColor = null)
        {
            if (lineColor == null) lineColor = Color.Red;

            Point[] points = PointsFromArrays(Xs, Ys);
            Pen penLine = new Pen(new SolidBrush((Color)lineColor), lineWidth);

            penLine.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            penLine.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            penLine.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

            gfxGraph.DrawLines(penLine, points);
            this.pointCount += points.Length;
        }

        public void PlotSignal(double[] values, double pointSpacing = 1, double offsetX = 0, double offsetY = 0, float lineWidth = 1, Color? lineColor = null)
        {
            if (lineColor == null) lineColor = Color.Red;
            if (values == null) return;

            int pointCount = values.Length;
            double lastPointX = offsetX + values.Length * pointSpacing;
            int dataMinPx = (int)((offsetX - xAxis.min) / xAxis.unitsPerPx);
            int dataMaxPx = (int)((lastPointX - xAxis.min) / xAxis.unitsPerPx);
            double binUnitsPerPx = xAxis.unitsPerPx / pointSpacing;
            double dataPointsPerPixel = xAxis.unitsPerPx / pointSpacing;

            List<Point> points = new List<Point>();
            List<double> Ys = new List<double>();

            for (int i = 0; i < values.Length; i++) Ys.Add(values[i]);

            if (dataPointsPerPixel < 1)
            {
                int iLeft = (int)(((xAxis.min - offsetX) / xAxis.unitsPerPx) * dataPointsPerPixel);
                int iRight = iLeft + (int)(dataPointsPerPixel * bmpGraph.Width);
                for (int i = Math.Max(0, iLeft - 2); i < Math.Min(iRight + 3, Ys.Count - 1); i++)
                {
                    int xPx = xAxis.GetPixel(i * pointSpacing + offsetX);
                    int yPx = yAxis.GetPixel(Ys[i]);
                    points.Add(new Point(xPx, yPx));
                }
            }
            else
            {
                for (int xPixel = Math.Max(0, dataMinPx); xPixel < Math.Min(bmpGraph.Width, dataMaxPx); xPixel++)
                {
                    int iLeft = (int)(binUnitsPerPx * (xPixel - dataMinPx));
                    int iRight = (int)(iLeft + binUnitsPerPx);
                    iLeft = Math.Max(iLeft, 0);
                    iRight = Math.Min(Ys.Count - 1, iRight);
                    iRight = Math.Max(iRight, 0);
                    if (iLeft == iRight) continue;
                    double yPxMin = Ys.GetRange(iLeft, iRight - iLeft).Min() + offsetY;
                    double yPxMax = Ys.GetRange(iLeft, iRight - iLeft).Max() + offsetY;
                    points.Add(new Point(xPixel, yAxis.GetPixel(yPxMin)));
                    points.Add(new Point(xPixel, yAxis.GetPixel(yPxMax)));
                }
            }

            if (points.Count < 2) return;
            Pen penLine = new Pen(new SolidBrush((Color)lineColor), lineWidth);
            float markerSize = 3;
            SolidBrush markerBrush = new SolidBrush((Color)lineColor);
            System.Drawing.Drawing2D.SmoothingMode originalSmoothingMode = gfxGraph.SmoothingMode;
            gfxGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            try
            {
                gfxGraph.DrawLines(penLine, points.ToArray());

                if (dataPointsPerPixel < .5)
                {
                    foreach (Point pt in points)
                    {
                        gfxGraph.FillEllipse(markerBrush, pt.X - markerSize / 2, pt.Y - markerSize / 2, markerSize, markerSize);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Exception plotting");
            }


            gfxGraph.SmoothingMode = originalSmoothingMode;
            this.pointCount += values.Length;
        }

        public void PlotScatter(double[] Xs, double[] Ys, float markerSize = 3, Color? markerColor = null)
        {
            if (markerColor == null) markerColor = Color.Red;
            Point[] points = PointsFromArrays(Xs, Ys);
            for (int i = 0; i < points.Length; i++)
            {
                gfxGraph.FillEllipse(new SolidBrush((Color)markerColor),
                                     points[i].X - markerSize / 2,
                                     points[i].Y - markerSize / 2,
                                     markerSize, markerSize);
            }
            pointCount += points.Length;
        }

        MouseAxis mousePan = null;
        MouseAxis mouseZoom = null;

        public void MousePanStart(int xPx, int yPx) { mousePan = new MouseAxis(xAxis, yAxis, xPx, yPx); }
        public void MousePanEnd() { mousePan = null; }
        public void MouseZoomStart(int xPx, int yPx) { mouseZoom = new MouseAxis(xAxis, yAxis, xPx, yPx); }
        public void MouseZoomEnd() { mouseZoom = null; }
        public bool MouseIsDragging() { return (mousePan != null || mouseZoom != null); }

        public void MouseMove(int xPx, int yPx)
        {
            if (mousePan != null)
            {
                mousePan.Pan(xPx, yPx);
                AxisSet(mousePan.x1, mousePan.x2, mousePan.y1, mousePan.y2);
            }
            else if (mouseZoom != null)
            {
                mouseZoom.Zoom(xPx, yPx);
                AxisSet(mouseZoom.x1, mouseZoom.x2, mouseZoom.y1, mouseZoom.y2);
            }
        }
    }
}
