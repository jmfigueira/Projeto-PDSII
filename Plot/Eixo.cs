using System;
using System.Collections.Generic;

namespace Plot
{
    public class Eixo
    {
        public double min { get; set; }
        public double max { get; set; }
        public int pxSize { get; private set; }
        public bool inverted { get; set; }

        public double unitsPerPx { get; private set; }
        public double pxPerUnit { get; private set; }
        public double span { get { return max - min; } }
        public double center { get { return (max + min) / 2.0; } }

        public Eixo(double min, double max, int sizePx = 500, bool inverted = false)
        {
            this.min = min;
            this.max = max;
            this.inverted = inverted;
            Resize(sizePx);
        }

        public void Resize(int sizePx)
        {
            this.pxSize = sizePx;
            RecalculateScale();
        }

        public void RecalculateScale()
        {
            this.pxPerUnit = pxSize / (max - min);
            this.unitsPerPx = (max - min) / pxSize;
            RecalculateTicks();
        }

        public void Pan(double Shift)
        {
            min += Shift;
            max += Shift;
            RecalculateScale();
        }

        public void Zoom(double zoomFrac)
        {
            double newSpan = span / zoomFrac;
            double newCenter = center;
            min = newCenter - newSpan / 2;
            max = newCenter + newSpan / 2;
            RecalculateScale();
        }

        public int GetPixel(double unit)
        {
            int px = (int)((unit - min) * pxPerUnit);
            if (inverted) px = pxSize - px;
            return px;
        }

        public double GetUnit(int px)
        {
            if (inverted) px = pxSize - px;
            return min + px * unitsPerPx;
        }

        private double RoundNumberNear(double target)
        {
            target = Math.Abs(target);
            int lastDivision = 2;
            double round = 1000000000000;
            while (round > 0.00000000001)
            {
                if (round <= target) return round;
                round /= lastDivision;
                if (lastDivision == 2) lastDivision = 5;
                else lastDivision = 2;
            }
            return 0;
        }

        public Tick[] GenerateTicks(int targetTickCount)
        {
            List<Tick> ticks = new List<Tick>();

            if (targetTickCount > 0)
            {
                double tickSize = RoundNumberNear(((max - min) / targetTickCount) * 1.5);
                int lastTick = 123456789;
                for (int i = 0; i < pxSize; i++)
                {
                    double thisPosition = i * unitsPerPx + min;
                    int thisTick = (int)(thisPosition / tickSize);
                    if (thisTick != lastTick)
                    {
                        lastTick = thisTick;
                        double thisPositionRounded = (int)(thisPosition / tickSize) * tickSize;
                        if (thisPositionRounded > min && thisPositionRounded < max)
                        {
                            ticks.Add(new Tick(thisPositionRounded, GetPixel(thisPositionRounded), max - min));
                        }
                    }
                }
            }
            return ticks.ToArray();
        }

        public Tick[] ticksMajor;
        public Tick[] ticksMinor;
        public double pixelsPerTick = 70;
        private void RecalculateTicks()
        {
            double tick_density_x = pxSize / pixelsPerTick;
            ticksMajor = GenerateTicks((int)(tick_density_x * 5));
            ticksMinor = GenerateTicks((int)(tick_density_x * 1));
        }

        public class Tick
        {
            public double posUnit { get; set; }
            public int posPixel { get; set; }
            public double spanUnits { get; set; }

            public Tick(double value, int pixel, double axisSpan)
            {
                this.posUnit = value;
                this.posPixel = pixel;
                this.spanUnits = axisSpan;
            }

            public string label
            {
                get
                {
                    if (spanUnits < .01) return string.Format("{0:0.0000}", posUnit);
                    if (spanUnits < .1) return string.Format("{0:0.000}", posUnit);
                    if (spanUnits < 1) return string.Format("{0:0.00}", posUnit);
                    if (spanUnits < 10) return string.Format("{0:0.0}", posUnit);
                    return string.Format("{0:0}", posUnit);
                }
            }
        }

    }



}
