namespace PlotMicrophoneFFT
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PlotUC1 = new Plot.PlotUC();
            this.PlotUC2 = new Plot.PlotUC();
            this.timerReplot = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.PlotUC1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.PlotUC2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(912, 672);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // PlotUC1
            // 
            this.PlotUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlotUC1.Location = new System.Drawing.Point(2, 2);
            this.PlotUC1.Margin = new System.Windows.Forms.Padding(2);
            this.PlotUC1.Name = "PlotUC1";
            this.PlotUC1.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.PlotUC1.Size = new System.Drawing.Size(908, 332);
            this.PlotUC1.TabIndex = 0;
            // 
            // PlotUC2
            // 
            this.PlotUC2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlotUC2.Location = new System.Drawing.Point(2, 338);
            this.PlotUC2.Margin = new System.Windows.Forms.Padding(2);
            this.PlotUC2.Name = "PlotUC2";
            this.PlotUC2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.PlotUC2.Size = new System.Drawing.Size(908, 332);
            this.PlotUC2.TabIndex = 1;
            // 
            // timerReplot
            // 
            this.timerReplot.Interval = 2;
            this.timerReplot.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 672);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Plot Microphone FFT Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Plot.PlotUC PlotUC1;
        private Plot.PlotUC PlotUC2;
        private System.Windows.Forms.Timer timerReplot;
    }
}

