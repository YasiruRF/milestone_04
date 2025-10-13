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
            this.InputBox1 = new System.Windows.Forms.TextBox();
            this.OutputBox1 = new System.Windows.Forms.NumericUpDown();
            this.Send1 = new System.Windows.Forms.Button();
            this.Send2 = new System.Windows.Forms.Button();
            this.Get1 = new System.Windows.Forms.Button();
            this.Get2 = new System.Windows.Forms.Button();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.InputBox2 = new System.Windows.Forms.TextBox();
            this.OutputBox2 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.OutputBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputBox2)).BeginInit();
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
            // InputBox1
            // 
            this.InputBox1.Location = new System.Drawing.Point(53, 119);
            this.InputBox1.Margin = new System.Windows.Forms.Padding(4);
            this.InputBox1.Name = "InputBox1";
            this.InputBox1.Size = new System.Drawing.Size(167, 22);
            this.InputBox1.TabIndex = 0;
            this.InputBox1.Text = "0";
            // 
            // OutputBox1
            // 
            this.OutputBox1.DecimalPlaces = 1;
            this.OutputBox1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.OutputBox1.Location = new System.Drawing.Point(53, 84);
            this.OutputBox1.Margin = new System.Windows.Forms.Padding(4);
            this.OutputBox1.Name = "OutputBox1";
            this.OutputBox1.Size = new System.Drawing.Size(167, 22);
            this.OutputBox1.TabIndex = 3;
            // 
            // Send1
            // 
            this.Send1.Location = new System.Drawing.Point(228, 48);
            this.Send1.Margin = new System.Windows.Forms.Padding(4);
            this.Send1.Name = "Send1";
            this.Send1.Size = new System.Drawing.Size(100, 28);
            this.Send1.TabIndex = 4;
            this.Send1.Text = "DutyCycle1";
            this.Send1.UseVisualStyleBackColor = true;
            this.Send1.Click += new System.EventHandler(this.Send1_Click);
            // 
            // Send2
            // 
            this.Send2.Location = new System.Drawing.Point(228, 84);
            this.Send2.Margin = new System.Windows.Forms.Padding(4);
            this.Send2.Name = "Send2";
            this.Send2.Size = new System.Drawing.Size(100, 28);
            this.Send2.TabIndex = 4;
            this.Send2.Text = "DutyCycle2";
            this.Send2.UseVisualStyleBackColor = true;
            this.Send2.Click += new System.EventHandler(this.Send2_Click);
            // 
            // Get1
            // 
            this.Get1.Location = new System.Drawing.Point(227, 117);
            this.Get1.Margin = new System.Windows.Forms.Padding(4);
            this.Get1.Name = "Get1";
            this.Get1.Size = new System.Drawing.Size(100, 28);
            this.Get1.TabIndex = 4;
            this.Get1.Text = "Input 1";
            this.Get1.UseVisualStyleBackColor = true;
            this.Get1.Click += new System.EventHandler(this.Get1_Click);
            // 
            // Get2
            // 
            this.Get2.Location = new System.Drawing.Point(227, 153);
            this.Get2.Margin = new System.Windows.Forms.Padding(4);
            this.Get2.Name = "Get2";
            this.Get2.Size = new System.Drawing.Size(100, 28);
            this.Get2.TabIndex = 4;
            this.Get2.Text = "Input 2";
            this.Get2.UseVisualStyleBackColor = true;
            this.Get2.Click += new System.EventHandler(this.Get2_Click);
            // 
            // statusBox
            // 
            this.statusBox.Location = new System.Drawing.Point(53, 217);
            this.statusBox.Margin = new System.Windows.Forms.Padding(4);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(167, 22);
            this.statusBox.TabIndex = 5;
            // 
            // InputBox2
            // 
            this.InputBox2.Location = new System.Drawing.Point(53, 153);
            this.InputBox2.Margin = new System.Windows.Forms.Padding(4);
            this.InputBox2.Name = "InputBox2";
            this.InputBox2.Size = new System.Drawing.Size(167, 22);
            this.InputBox2.TabIndex = 0;
            this.InputBox2.Text = "0";
            // 
            // OutputBox2
            // 
            this.OutputBox2.DecimalPlaces = 1;
            this.OutputBox2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.OutputBox2.Location = new System.Drawing.Point(53, 48);
            this.OutputBox2.Margin = new System.Windows.Forms.Padding(4);
            this.OutputBox2.Name = "OutputBox2";
            this.OutputBox2.Size = new System.Drawing.Size(167, 22);
            this.OutputBox2.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 256);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.Get2);
            this.Controls.Add(this.Get1);
            this.Controls.Add(this.Send2);
            this.Controls.Add(this.Send1);
            this.Controls.Add(this.OutputBox2);
            this.Controls.Add(this.OutputBox1);
            this.Controls.Add(this.InputBox2);
            this.Controls.Add(this.InputBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OutputBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer getIOtimer;
        private System.Windows.Forms.TextBox InputBox1;
        private System.Windows.Forms.NumericUpDown OutputBox1;
        private System.IO.Ports.SerialPort serial;
        private System.Windows.Forms.Button Send1;
        private System.Windows.Forms.Button Send2;
        private System.Windows.Forms.Button Get1;
        private System.Windows.Forms.Button Get2;
        private System.Windows.Forms.TextBox statusBox;
        private System.Windows.Forms.TextBox InputBox2;
        private System.Windows.Forms.NumericUpDown OutputBox2;
    }
}

