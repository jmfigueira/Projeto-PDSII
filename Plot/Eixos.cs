using System;

namespace Plot
{
    class MouseAxis
    {
        public Eixo xAxStart, yAxStart;
        public int xMouseStart, yMouseStart;
        public double x1, x2, y1, y2;

        public MouseAxis(Eixo xAxis, Eixo yAxis, int mouseX, int mouseY)
        {
            xAxStart = new Eixo(xAxis.min, xAxis.max, xAxis.pxSize, xAxis.inverted);
            yAxStart = new Eixo(yAxis.min, yAxis.max, yAxis.pxSize, yAxis.inverted);
            xMouseStart = mouseX;
            yMouseStart = mouseY;
            Pan(0, 0);
        }

        public void Pan(int xMouseNow, int yMouseNow)
        {
            int dX = xMouseStart - xMouseNow;
            int dY = yMouseNow - yMouseStart;
            x1 = xAxStart.min + dX * xAxStart.unitsPerPx;
            x2 = xAxStart.max + dX * xAxStart.unitsPerPx;
            y1 = yAxStart.min + dY * yAxStart.unitsPerPx;
            y2 = yAxStart.max + dY * yAxStart.unitsPerPx;
        }

        public void Zoom(int xMouseNow, int yMouseNow)
        {
            double dX = (xMouseNow - xMouseStart) * xAxStart.unitsPerPx;
            double dY = (yMouseStart - yMouseNow) * yAxStart.unitsPerPx;

            double dXFrac = dX / (Math.Abs(dX) + xAxStart.span);
            double dYFrac = dY / (Math.Abs(dY) + yAxStart.span);

            double xNewSpan = xAxStart.span / Math.Pow(10, dXFrac);
            double yNewSpan = yAxStart.span / Math.Pow(10, dYFrac);

            double xNewCenter = xAxStart.center;
            double yNewCenter = yAxStart.center;

            x1 = xNewCenter - xNewSpan / 2;
            x2 = xNewCenter + xNewSpan / 2;

            y1 = yNewCenter - yNewSpan / 2;
            y2 = yNewCenter + yNewSpan / 2;
        }
    }
}
