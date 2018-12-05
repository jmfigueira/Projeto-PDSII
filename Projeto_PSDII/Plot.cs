using System;
using System.Windows.Forms;

namespace PlotWrapper
{
    public partial class Plot : Form
    {
        private string mTitle;
        private string mAxisX;
        private string mAxisY;

        public Plot(string mainTitle, string xAxisTitle, string yAxisTitle)
        {
            InitializeComponent();

            mTitle = mainTitle;
            mAxisX = xAxisTitle;
            mAxisY = yAxisTitle;
        }

        private void Plot_Load(object sender, EventArgs e)
        {
            chart1.Titles["Title"].Text = mTitle;
            this.Text = mTitle;
            chart1.Titles["AxisX"].Text = mAxisX; 
            chart1.Titles["AxisY"].Text = mAxisY;

            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
        }

        public void PlotData(double[] yData)
        {
            chart1.Series["Series1"].Points.Clear();

            double[] xData = DSPLib_Test.Generate.LinSpace(0, yData.Length-1, (uint)yData.Length);
            chart1.Series["Series1"].Points.DataBindXY(xData, yData);
        }

        public void PlotData(double[] xData, double[] yData)
        {
            chart1.Series["Series1"].Points.Clear();
            chart1.Series["Series1"].Points.DataBindXY(xData, yData);
        }

    }
}
