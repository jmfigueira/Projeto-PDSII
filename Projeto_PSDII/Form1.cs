using PlotWrapper;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows.Forms;

namespace Projeto_PDSII
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_FFT(object sender, EventArgs e)
        {
            uint N = 1024;
            uint zeros = 0;
            double samplingRateHz = Convert.ToDouble(txtFs.Text);

            string selectedWindowName = DSPLib_Test.Window.Type.None.ToString();
            DSPLib_Test.Window.Type windowToApply = (DSPLib_Test.Window.Type)Enum.Parse(typeof(DSPLib_Test.Window.Type), selectedWindowName);

            double[] timeSeries = GenerateTimeSeriesData(N);

            double[] wc = DSPLib_Test.Window.Coefficients(windowToApply, N);

            double windowScaleFactor = DSPLib_Test.Window.ScaleFactor.Signal(wc);
            double[] windowedTimeSeries = DSPLib_Test.MathOperations.Multiply(timeSeries, wc);

            Plot fig1 = new Plot("FFT - Entrada de séries no domínio do tempo", "Amostras", "Volts");
            fig1.PlotData(windowedTimeSeries);
            fig1.Show();

            DSPLib_Test.FFT fft = new DSPLib_Test.FFT();
            fft.Initialize(N, zeros);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Complex[] cpxResult = fft.Execute(windowedTimeSeries);

            stopwatch.Stop();

            double[] magResult = DSPLib_Test.ConvertComplex.ToMagnitude(cpxResult);
            magResult = DSPLib_Test.MathOperations.Multiply(magResult, windowScaleFactor);

            Plot fig2 = new Plot("FFT - Magnitude", "FFT Bin", "Mag (Vrms)");
            fig2.PlotData(magResult);
            fig2.Show();

            double[] fSpan = fft.FrequencySpan(samplingRateHz);

            double[] mag = DSPLib_Test.ConvertComplex.ToMagnitude(cpxResult);
            mag = DSPLib_Test.MathOperations.Multiply(mag, windowScaleFactor);
            double[] magLog = DSPLib_Test.ConvertMagnitude.ToMagnitudeDBV(mag);
            Plot fig3 = new Plot("FFT - Log Magnitude", "Frequência (Hz)", "Mag (dBV)");
            fig3.PlotData(fSpan, magLog);
            fig3.Show();
        }

        private void button_DFT(object sender, EventArgs e)
        {
            uint N = 1024;
            uint zeros = 0;
            double samplingRateHz = Convert.ToDouble(txtFs.Text);

            string selectedWindowName = DSPLib_Test.Window.Type.None.ToString();
            DSPLib_Test.Window.Type windowToApply = (DSPLib_Test.Window.Type)Enum.Parse(typeof(DSPLib_Test.Window.Type), selectedWindowName);

            double[] timeSeries = GenerateTimeSeriesData(N);

            double[] wc = DSPLib_Test.Window.Coefficients(windowToApply, N);

            double windowScaleFactor = DSPLib_Test.Window.ScaleFactor.Signal(wc);
            double[] windowedTimeSeries = DSPLib_Test.MathOperations.Multiply(timeSeries, wc);

            Plot fig1 = new Plot("DFT - Entrada de séries no domínio do tempo", "Amostras", "Volts");
            fig1.PlotData(windowedTimeSeries);
            fig1.Show();

            DSPLib_Test.DFT dft = new DSPLib_Test.DFT();
            dft.Inicializar(N, zeros);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Complex[] cpxResult = dft.Executar(windowedTimeSeries);

            stopwatch.Stop();

            double[] magResult = DSPLib_Test.ConvertComplex.ToMagnitude(cpxResult);
            magResult = DSPLib_Test.MathOperations.Multiply(magResult, windowScaleFactor);


            Plot fig2 = new Plot("DFT - Magnitude", "DFT Bin", "Mag (Vrms)");
            fig2.PlotData(magResult);
            fig2.Show();

            double[] fSpan = dft.FrequencySpan(samplingRateHz);

            double[] mag = DSPLib_Test.ConvertComplex.ToMagnitude(cpxResult);
            mag = DSPLib_Test.MathOperations.Multiply(mag, windowScaleFactor);
            double[] magLog = DSPLib_Test.ConvertMagnitude.ToMagnitudeDBV(mag);
            Plot fig3 = new Plot("DFT - Log Magnitude", "Frequência (Hz)", "Mag (dBV)");
            fig3.PlotData(fSpan, magLog);
            fig3.Show();
        }

        private double[] GenerateTimeSeriesData(uint N)
        {
            double freqIn = Convert.ToDouble(txtFreqIn.Text);
            double ampRMS = Convert.ToDouble(txtAmplitude.Text);
            double freqSampling = Convert.ToDouble(txtFs.Text);
            double ampDC = 0.0;

            double[] timeSeries = DSPLib_Test.Generate.ToneSampling(ampRMS, freqIn, freqSampling, N, ampDC);

            return timeSeries;
        }

        private void btnReal_Click_1(object sender, EventArgs e)
        {
            PlotMicrophoneFFT.Form1 fr = new PlotMicrophoneFFT.Form1();
            fr.ShowDialog();
        }
    }
}
