namespace SerialGUISample
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
            this.serial = new System.IO.Ports.SerialPort(this.components);
            this.getIOtimer = new System.Windows.Forms.Timer(this.components);
            this.statusBox = new System.Windows.Forms.TextBox();
            this.lblLeftSensor = new System.Windows.Forms.Label();
            this.txtLeftSensor = new System.Windows.Forms.TextBox();
            this.lblRightSensor = new System.Windows.Forms.Label();
            this.txtRightSensor = new System.Windows.Forms.TextBox();
            this.lblLeftMotor = new System.Windows.Forms.Label();
            this.txtLeftMotor = new System.Windows.Forms.TextBox();
            this.lblRightMotor = new System.Windows.Forms.Label();
            this.txtRightMotor = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnReverse = new System.Windows.Forms.Button();
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
            // statusBox
            // 
            this.statusBox.Location = new System.Drawing.Point(92, 30);
            this.statusBox.Margin = new System.Windows.Forms.Padding(4);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(167, 22);
            this.statusBox.TabIndex = 5;
            // 
            // lblLeftSensor
            // 
            this.lblLeftSensor.AutoSize = true;
            this.lblLeftSensor.Location = new System.Drawing.Point(89, 62);
            this.lblLeftSensor.Name = "lblLeftSensor";
            this.lblLeftSensor.Size = new System.Drawing.Size(77, 16);
            this.lblLeftSensor.TabIndex = 6;
            this.lblLeftSensor.Text = "Left Sensor:";
            this.lblLeftSensor.Click += new System.EventHandler(this.lblLeftSensor_Click);
            // 
            // txtLeftSensor
            // 
            this.txtLeftSensor.Location = new System.Drawing.Point(174, 59);
            this.txtLeftSensor.Name = "txtLeftSensor";
            this.txtLeftSensor.ReadOnly = true;
            this.txtLeftSensor.Size = new System.Drawing.Size(85, 22);
            this.txtLeftSensor.TabIndex = 7;
            this.txtLeftSensor.TextChanged += new System.EventHandler(this.txtLeftSensor_TextChanged);
            // 
            // lblRightSensor
            // 
            this.lblRightSensor.AutoSize = true;
            this.lblRightSensor.Location = new System.Drawing.Point(89, 90);
            this.lblRightSensor.Name = "lblRightSensor";
            this.lblRightSensor.Size = new System.Drawing.Size(87, 16);
            this.lblRightSensor.TabIndex = 8;
            this.lblRightSensor.Text = "Right Sensor:";
            this.lblRightSensor.Click += new System.EventHandler(this.lblRightSensor_Click);
            // 
            // txtRightSensor
            // 
            this.txtRightSensor.Location = new System.Drawing.Point(174, 87);
            this.txtRightSensor.Name = "txtRightSensor";
            this.txtRightSensor.ReadOnly = true;
            this.txtRightSensor.Size = new System.Drawing.Size(85, 22);
            this.txtRightSensor.TabIndex = 9;
            this.txtRightSensor.TextChanged += new System.EventHandler(this.txtRightSensor_TextChanged);
            // 
            // lblLeftMotor
            // 
            this.lblLeftMotor.AutoSize = true;
            this.lblLeftMotor.Location = new System.Drawing.Point(89, 118);
            this.lblLeftMotor.Name = "lblLeftMotor";
            this.lblLeftMotor.Size = new System.Drawing.Size(68, 16);
            this.lblLeftMotor.TabIndex = 10;
            this.lblLeftMotor.Text = "Left Motor:";
            this.lblLeftMotor.Click += new System.EventHandler(this.lblLeftMotor_Click);
            // 
            // txtLeftMotor
            // 
            this.txtLeftMotor.Location = new System.Drawing.Point(174, 115);
            this.txtLeftMotor.Name = "txtLeftMotor";
            this.txtLeftMotor.ReadOnly = true;
            this.txtLeftMotor.Size = new System.Drawing.Size(85, 22);
            this.txtLeftMotor.TabIndex = 11;
            this.txtLeftMotor.TextChanged += new System.EventHandler(this.txtLeftMotor_TextChanged);
            // 
            // lblRightMotor
            // 
            this.lblRightMotor.AutoSize = true;
            this.lblRightMotor.Location = new System.Drawing.Point(89, 146);
            this.lblRightMotor.Name = "lblRightMotor";
            this.lblRightMotor.Size = new System.Drawing.Size(78, 16);
            this.lblRightMotor.TabIndex = 12;
            this.lblRightMotor.Text = "Right Motor:";
            this.lblRightMotor.Click += new System.EventHandler(this.lblRightMotor_Click);
            // 
            // txtRightMotor
            // 
            this.txtRightMotor.Location = new System.Drawing.Point(174, 143);
            this.txtRightMotor.Name = "txtRightMotor";
            this.txtRightMotor.ReadOnly = true;
            this.txtRightMotor.Size = new System.Drawing.Size(85, 22);
            this.txtRightMotor.TabIndex = 13;
            this.txtRightMotor.TextChanged += new System.EventHandler(this.txtRightMotor_TextChanged);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LightGreen;
            this.btnStart.Location = new System.Drawing.Point(48, 185);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(80, 35);
            this.btnStart.TabIndex = 14;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.LightCoral;
            this.btnStop.Location = new System.Drawing.Point(136, 185);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(80, 35);
            this.btnStop.TabIndex = 15;
            this.btnStop.Text = "STOP";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnReverse
            // 
            this.btnReverse.BackColor = System.Drawing.Color.LightBlue;
            this.btnReverse.Location = new System.Drawing.Point(224, 185);
            this.btnReverse.Margin = new System.Windows.Forms.Padding(4);
            this.btnReverse.Name = "btnReverse";
            this.btnReverse.Size = new System.Drawing.Size(80, 35);
            this.btnReverse.TabIndex = 16;
            this.btnReverse.Text = "REVERSE";
            this.btnReverse.UseVisualStyleBackColor = false;
            this.btnReverse.Click += new System.EventHandler(this.btnReverse_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 251);
            this.Controls.Add(this.txtRightMotor);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnReverse);
            this.Controls.Add(this.lblRightMotor);
            this.Controls.Add(this.txtLeftMotor);
            this.Controls.Add(this.lblLeftMotor);
            this.Controls.Add(this.txtRightSensor);
            this.Controls.Add(this.lblRightSensor);
            this.Controls.Add(this.txtLeftSensor);
            this.Controls.Add(this.lblLeftSensor);
            this.Controls.Add(this.statusBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer getIOtimer;
        private System.IO.Ports.SerialPort serial;
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
        private System.Windows.Forms.Button btnReverse;
    }
}

