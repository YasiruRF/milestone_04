namespace SerialGUISample
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.serial = new System.IO.Ports.SerialPort(this.components);
            this.getIOtimer = new System.Windows.Forms.Timer(this.components);
            this.timerElapsed = new System.Windows.Forms.Timer(this.components);
            this.statusBox = new System.Windows.Forms.TextBox();
            this.txtLeftSensor = new System.Windows.Forms.TextBox();
            this.lblLeftSensor = new System.Windows.Forms.Label();
            this.txtRightSensor = new System.Windows.Forms.TextBox();
            this.lblRightSensor = new System.Windows.Forms.Label();
            this.txtLeftMotor = new System.Windows.Forms.TextBox();
            this.lblLeftMotor = new System.Windows.Forms.Label();
            this.txtRightMotor = new System.Windows.Forms.TextBox();
            this.lblRightMotor = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnToggleDirection = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblMode = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelPWM = new System.Windows.Forms.Panel();
            this.lblSlowPWMValue = new System.Windows.Forms.Label();
            this.lblFastPWMValue = new System.Windows.Forms.Label();
            this.sliderSlowPWM = new System.Windows.Forms.TrackBar();
            this.lblSlowPWM = new System.Windows.Forms.Label();
            this.sliderFastPWM = new System.Windows.Forms.TrackBar();
            this.lblFastPWM = new System.Windows.Forms.Label();
            this.chartPWM = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelTimer = new System.Windows.Forms.Panel();
            this.lblTimerValue = new System.Windows.Forms.Label();
            this.lblTimer = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelPWM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderSlowPWM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderFastPWM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPWM)).BeginInit();
            this.panelTimer.SuspendLayout();
            this.SuspendLayout();
            // 
            // serial
            // 
            this.serial.PortName = "COM12";
            // 
            // getIOtimer
            // 
            this.getIOtimer.Enabled = true;
            this.getIOtimer.Interval = 10;
            this.getIOtimer.Tick += new System.EventHandler(this.getIOtimer_Tick);
            // 
            // timerElapsed
            // 
            this.timerElapsed.Interval = 10;
            this.timerElapsed.Tick += new System.EventHandler(this.timerElapsed_Tick);
            // 
            // statusBox
            // 
            this.statusBox.BackColor = System.Drawing.Color.LightGray;
            this.statusBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.statusBox.Location = new System.Drawing.Point(27, 111);
            this.statusBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.statusBox.Name = "statusBox";
            this.statusBox.ReadOnly = true;
            this.statusBox.Size = new System.Drawing.Size(332, 30);
            this.statusBox.TabIndex = 0;
            this.statusBox.Text = "Ready";
            this.statusBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtLeftSensor
            // 
            this.txtLeftSensor.BackColor = System.Drawing.Color.White;
            this.txtLeftSensor.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtLeftSensor.Location = new System.Drawing.Point(200, 166);
            this.txtLeftSensor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLeftSensor.Name = "txtLeftSensor";
            this.txtLeftSensor.ReadOnly = true;
            this.txtLeftSensor.Size = new System.Drawing.Size(159, 31);
            this.txtLeftSensor.TabIndex = 1;
            this.txtLeftSensor.Text = "0";
            this.txtLeftSensor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLeftSensor
            // 
            this.lblLeftSensor.AutoSize = true;
            this.lblLeftSensor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLeftSensor.Location = new System.Drawing.Point(27, 170);
            this.lblLeftSensor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLeftSensor.Name = "lblLeftSensor";
            this.lblLeftSensor.Size = new System.Drawing.Size(98, 23);
            this.lblLeftSensor.TabIndex = 2;
            this.lblLeftSensor.Text = "Left Sensor:";
            this.lblLeftSensor.Click += new System.EventHandler(this.lblLeftSensor_Click);
            // 
            // txtRightSensor
            // 
            this.txtRightSensor.BackColor = System.Drawing.Color.White;
            this.txtRightSensor.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtRightSensor.Location = new System.Drawing.Point(200, 209);
            this.txtRightSensor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRightSensor.Name = "txtRightSensor";
            this.txtRightSensor.ReadOnly = true;
            this.txtRightSensor.Size = new System.Drawing.Size(159, 31);
            this.txtRightSensor.TabIndex = 3;
            this.txtRightSensor.Text = "0";
            this.txtRightSensor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblRightSensor
            // 
            this.lblRightSensor.AutoSize = true;
            this.lblRightSensor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRightSensor.Location = new System.Drawing.Point(27, 213);
            this.lblRightSensor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRightSensor.Name = "lblRightSensor";
            this.lblRightSensor.Size = new System.Drawing.Size(110, 23);
            this.lblRightSensor.TabIndex = 4;
            this.lblRightSensor.Text = "Right Sensor:";
            // 
            // txtLeftMotor
            // 
            this.txtLeftMotor.BackColor = System.Drawing.Color.White;
            this.txtLeftMotor.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.txtLeftMotor.ForeColor = System.Drawing.Color.Blue;
            this.txtLeftMotor.Location = new System.Drawing.Point(200, 258);
            this.txtLeftMotor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLeftMotor.Name = "txtLeftMotor";
            this.txtLeftMotor.ReadOnly = true;
            this.txtLeftMotor.Size = new System.Drawing.Size(159, 31);
            this.txtLeftMotor.TabIndex = 5;
            this.txtLeftMotor.Text = "128";
            this.txtLeftMotor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLeftMotor
            // 
            this.lblLeftMotor.AutoSize = true;
            this.lblLeftMotor.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLeftMotor.Location = new System.Drawing.Point(27, 262);
            this.lblLeftMotor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLeftMotor.Name = "lblLeftMotor";
            this.lblLeftMotor.Size = new System.Drawing.Size(103, 23);
            this.lblLeftMotor.TabIndex = 6;
            this.lblLeftMotor.Text = "Left Motor:";
            // 
            // txtRightMotor
            // 
            this.txtRightMotor.BackColor = System.Drawing.Color.White;
            this.txtRightMotor.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.txtRightMotor.ForeColor = System.Drawing.Color.Red;
            this.txtRightMotor.Location = new System.Drawing.Point(200, 302);
            this.txtRightMotor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRightMotor.Name = "txtRightMotor";
            this.txtRightMotor.ReadOnly = true;
            this.txtRightMotor.Size = new System.Drawing.Size(159, 31);
            this.txtRightMotor.TabIndex = 7;
            this.txtRightMotor.Text = "128";
            this.txtRightMotor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblRightMotor
            // 
            this.lblRightMotor.AutoSize = true;
            this.lblRightMotor.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRightMotor.Location = new System.Drawing.Point(27, 305);
            this.lblRightMotor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRightMotor.Name = "lblRightMotor";
            this.lblRightMotor.Size = new System.Drawing.Size(115, 23);
            this.lblRightMotor.TabIndex = 8;
            this.lblRightMotor.Text = "Right Motor:";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LightGreen;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnStart.Location = new System.Drawing.Point(27, 357);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(160, 55);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "▶ START";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.LightCoral;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnStop.Location = new System.Drawing.Point(200, 357);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(160, 55);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "■ STOP";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnToggleDirection
            // 
            this.btnToggleDirection.BackColor = System.Drawing.Color.LightGreen;
            this.btnToggleDirection.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnToggleDirection.Location = new System.Drawing.Point(27, 425);
            this.btnToggleDirection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnToggleDirection.Name = "btnToggleDirection";
            this.btnToggleDirection.Size = new System.Drawing.Size(333, 49);
            this.btnToggleDirection.TabIndex = 11;
            this.btnToggleDirection.Text = "FORWARD MODE →";
            this.btnToggleDirection.UseVisualStyleBackColor = false;
            this.btnToggleDirection.Click += new System.EventHandler(this.btnToggleDirection_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(100)))));
            this.panelTop.Controls.Add(this.lblMode);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1200, 86);
            this.panelTop.TabIndex = 12;
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblMode.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblMode.Location = new System.Drawing.Point(1000, 25);
            this.lblMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(152, 37);
            this.lblMode.TabIndex = 1;
            this.lblMode.Text = "FORWARD";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(27, 18);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(421, 46);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🚗 Line Follower Control";
            // 
            // panelPWM
            // 
            this.panelPWM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.panelPWM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPWM.Controls.Add(this.lblSlowPWMValue);
            this.panelPWM.Controls.Add(this.lblFastPWMValue);
            this.panelPWM.Controls.Add(this.sliderSlowPWM);
            this.panelPWM.Controls.Add(this.lblSlowPWM);
            this.panelPWM.Controls.Add(this.sliderFastPWM);
            this.panelPWM.Controls.Add(this.lblFastPWM);
            this.panelPWM.Location = new System.Drawing.Point(400, 111);
            this.panelPWM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelPWM.Name = "panelPWM";
            this.panelPWM.Size = new System.Drawing.Size(373, 221);
            this.panelPWM.TabIndex = 13;
            // 
            // lblSlowPWMValue
            // 
            this.lblSlowPWMValue.AutoSize = true;
            this.lblSlowPWMValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblSlowPWMValue.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblSlowPWMValue.Location = new System.Drawing.Point(293, 129);
            this.lblSlowPWMValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSlowPWMValue.Name = "lblSlowPWMValue";
            this.lblSlowPWMValue.Size = new System.Drawing.Size(43, 23);
            this.lblSlowPWMValue.TabIndex = 5;
            this.lblSlowPWMValue.Text = "75%";
            // 
            // lblFastPWMValue
            // 
            this.lblFastPWMValue.AutoSize = true;
            this.lblFastPWMValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblFastPWMValue.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblFastPWMValue.Location = new System.Drawing.Point(293, 31);
            this.lblFastPWMValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFastPWMValue.Name = "lblFastPWMValue";
            this.lblFastPWMValue.Size = new System.Drawing.Size(43, 23);
            this.lblFastPWMValue.TabIndex = 4;
            this.lblFastPWMValue.Text = "85%";
            // 
            // sliderSlowPWM
            // 
            this.sliderSlowPWM.Location = new System.Drawing.Point(20, 160);
            this.sliderSlowPWM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sliderSlowPWM.Maximum = 100;
            this.sliderSlowPWM.Name = "sliderSlowPWM";
            this.sliderSlowPWM.Size = new System.Drawing.Size(333, 56);
            this.sliderSlowPWM.TabIndex = 3;
            this.sliderSlowPWM.TickFrequency = 5;
            this.sliderSlowPWM.Value = 75;
            this.sliderSlowPWM.Scroll += new System.EventHandler(this.sliderSlowPWM_Scroll);
            // 
            // lblSlowPWM
            // 
            this.lblSlowPWM.AutoSize = true;
            this.lblSlowPWM.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSlowPWM.Location = new System.Drawing.Point(20, 129);
            this.lblSlowPWM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSlowPWM.Name = "lblSlowPWM";
            this.lblSlowPWM.Size = new System.Drawing.Size(181, 23);
            this.lblSlowPWM.TabIndex = 2;
            this.lblSlowPWM.Text = "Slow PWM (Turning):";
            // 
            // sliderFastPWM
            // 
            this.sliderFastPWM.Location = new System.Drawing.Point(20, 62);
            this.sliderFastPWM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sliderFastPWM.Maximum = 100;
            this.sliderFastPWM.Name = "sliderFastPWM";
            this.sliderFastPWM.Size = new System.Drawing.Size(333, 56);
            this.sliderFastPWM.TabIndex = 1;
            this.sliderFastPWM.TickFrequency = 5;
            this.sliderFastPWM.Value = 85;
            this.sliderFastPWM.Scroll += new System.EventHandler(this.sliderFastPWM_Scroll);
            // 
            // lblFastPWM
            // 
            this.lblFastPWM.AutoSize = true;
            this.lblFastPWM.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFastPWM.Location = new System.Drawing.Point(20, 31);
            this.lblFastPWM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFastPWM.Name = "lblFastPWM";
            this.lblFastPWM.Size = new System.Drawing.Size(195, 23);
            this.lblFastPWM.TabIndex = 0;
            this.lblFastPWM.Text = "Fast PWM (Full Speed):";
            // 
            // chartPWM
            // 
            chartArea1.Name = "ChartArea1";
            this.chartPWM.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartPWM.Legends.Add(legend1);
            this.chartPWM.Location = new System.Drawing.Point(400, 357);
            this.chartPWM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chartPWM.Name = "chartPWM";
            this.chartPWM.Size = new System.Drawing.Size(773, 369);
            this.chartPWM.TabIndex = 14;
            this.chartPWM.Text = "PWM Chart";
            // 
            // panelTimer
            // 
            this.panelTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.panelTimer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTimer.Controls.Add(this.lblTimerValue);
            this.panelTimer.Controls.Add(this.lblTimer);
            this.panelTimer.Location = new System.Drawing.Point(800, 111);
            this.panelTimer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelTimer.Name = "panelTimer";
            this.panelTimer.Size = new System.Drawing.Size(373, 221);
            this.panelTimer.TabIndex = 15;
            // 
            // lblTimerValue
            // 
            this.lblTimerValue.AutoSize = true;
            this.lblTimerValue.Font = new System.Drawing.Font("Consolas", 32F, System.Drawing.FontStyle.Bold);
            this.lblTimerValue.ForeColor = System.Drawing.Color.DarkRed;
            this.lblTimerValue.Location = new System.Drawing.Point(27, 98);
            this.lblTimerValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimerValue.Name = "lblTimerValue";
            this.lblTimerValue.Size = new System.Drawing.Size(267, 64);
            this.lblTimerValue.TabIndex = 1;
            this.lblTimerValue.Text = "00:00:00";
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTimer.Location = new System.Drawing.Point(33, 31);
            this.lblTimer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(205, 32);
            this.lblTimer.TabIndex = 0;
            this.lblTimer.Text = "⏱ Elapsed Time";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(255)))), ((int)(((byte)(220)))));
            this.ClientSize = new System.Drawing.Size(1200, 751);
            this.Controls.Add(this.panelTimer);
            this.Controls.Add(this.chartPWM);
            this.Controls.Add(this.panelPWM);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.btnToggleDirection);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblRightMotor);
            this.Controls.Add(this.txtRightMotor);
            this.Controls.Add(this.lblLeftMotor);
            this.Controls.Add(this.txtLeftMotor);
            this.Controls.Add(this.lblRightSensor);
            this.Controls.Add(this.txtRightSensor);
            this.Controls.Add(this.lblLeftSensor);
            this.Controls.Add(this.txtLeftSensor);
            this.Controls.Add(this.statusBox);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Line Follower Control - Windows Vista Edition";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelPWM.ResumeLayout(false);
            this.panelPWM.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderSlowPWM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderFastPWM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPWM)).EndInit();
            this.panelTimer.ResumeLayout(false);
            this.panelTimer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serial;
        private System.Windows.Forms.Timer getIOtimer;
        private System.Windows.Forms.Timer timerElapsed;
        private System.Windows.Forms.TextBox statusBox;
        private System.Windows.Forms.TextBox txtLeftSensor;
        private System.Windows.Forms.Label lblLeftSensor;
        private System.Windows.Forms.TextBox txtRightSensor;
        private System.Windows.Forms.Label lblRightSensor;
        private System.Windows.Forms.TextBox txtLeftMotor;
        private System.Windows.Forms.Label lblLeftMotor;
        private System.Windows.Forms.TextBox txtRightMotor;
        private System.Windows.Forms.Label lblRightMotor;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnToggleDirection;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Panel panelPWM;
        private System.Windows.Forms.TrackBar sliderFastPWM;
        private System.Windows.Forms.Label lblFastPWM;
        private System.Windows.Forms.TrackBar sliderSlowPWM;
        private System.Windows.Forms.Label lblSlowPWM;
        private System.Windows.Forms.Label lblFastPWMValue;
        private System.Windows.Forms.Label lblSlowPWMValue;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPWM;
        private System.Windows.Forms.Panel panelTimer;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label lblTimerValue;
    }
}