using NAudio.Wave;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PlotMicrophoneFFT
{
    public partial class Form1 : Form
    {
        private int RATE = 44100;
        private int BUFFERSIZE = (int)Math.Pow(2, 11);

        public BufferedWaveProvider bwp;

        public Form1()
        {
            InitializeComponent();
            SetupGraphLabels();
            StartListeningToMicrophone();
            timerReplot.Enabled = true;
        }

        void AudioDataAvailable(object sender, WaveInEventArgs e)
        {
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void SetupGraphLabels()
        {
            PlotUC1.fig.labelTitle = "Entrada Microfone";
            PlotUC1.fig.labelY = "Amplitude (PCM)";
            PlotUC1.fig.labelX = "Tempo (ms)";
            PlotUC1.Redraw();

            PlotUC2.fig.labelTitle = "FFT - Resultado";
            PlotUC2.fig.labelY = "Power (raw)";
            PlotUC2.fig.labelX = "Frequência (Hz)";
            PlotUC2.Redraw();
        }

        public void StartListeningToMicrophone(int audioDeviceNumber = 0)
        {
            WaveIn wi = new WaveIn();
            wi.DeviceNumber = audioDeviceNumber;
            wi.WaveFormat = new NAudio.Wave.WaveFormat(RATE, 1);
            wi.BufferMilliseconds = (int)(BUFFERSIZE / (double)RATE * 1000.0);
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(AudioDataAvailable);
            bwp = new BufferedWaveProvider(wi.WaveFormat);
            bwp.BufferLength = BUFFERSIZE * 2;
            bwp.DiscardOnBufferOverflow = true;
            try
            {
                wi.StartRecording();
            }
            catch
            {
                string msg = "Não foi possível gravar a partir do dispositivo de áudio!\n\n";
                msg += "Seu microfone está conectado?\n";
                msg += "Está definido como o seu dispositivo de gravação padrão?";
                MessageBox.Show(msg, "ERROR");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timerReplot.Enabled = false;
            PlotLatestData();
            timerReplot.Enabled = true;
        }

        public int numberOfDraws = 0;
        public bool needsAutoScaling = true;
        public void PlotLatestData()
        {
            int frameSize = BUFFERSIZE;
            var audioBytes = new byte[frameSize];
            bwp.Read(audioBytes, 0, frameSize);

            if (audioBytes.Length == 0)
                return;
            if (audioBytes[frameSize - 2] == 0)
                return;

            int BYTES_PER_POINT = 2;

            int graphPointCount = audioBytes.Length / BYTES_PER_POINT;

            double[] pcm = new double[graphPointCount];
            double[] fft = new double[graphPointCount];
            double[] fftReal = new double[graphPointCount / 2];

            for (int i = 0; i < graphPointCount; i++)
            {
                short val = BitConverter.ToInt16(audioBytes, i * 2);

                pcm[i] = val / Math.Pow(2, 16) * 200.0;
            }

            fft = FFT(pcm);

            double pcmPointSpacingMs = RATE / 1000;
            double fftMaxFreq = RATE / 2;
            double fftPointSpacingHz = fftMaxFreq / graphPointCount;

            Array.Copy(fft, fftReal, fftReal.Length);

            PlotUC1.Clear();
            PlotUC1.PlotSignal(pcm, pcmPointSpacingMs, Color.Blue);
            PlotUC2.Clear();
            PlotUC2.PlotSignal(fftReal, fftPointSpacingHz, Color.Blue);

            if (needsAutoScaling)
            {
                PlotUC1.AxisAuto();
                PlotUC2.AxisAuto();
                needsAutoScaling = false;
            }

            numberOfDraws += 1;

            Application.DoEvents();

        }

        public double[] FFT(double[] data)
        {
            double[] fft = new double[data.Length];
            System.Numerics.Complex[] fftComplex = new System.Numerics.Complex[data.Length];
            for (int i = 0; i < data.Length; i++)
                fftComplex[i] = new System.Numerics.Complex(data[i], 0.0);
            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            for (int i = 0; i < data.Length; i++)
                fft[i] = fftComplex[i].Magnitude;
            return fft;
        }
    }
}
