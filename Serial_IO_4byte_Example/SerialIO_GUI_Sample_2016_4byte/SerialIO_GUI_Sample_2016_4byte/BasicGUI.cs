// Curtin University
// Mechatronics Engineering
// Enhanced Line Follower GUI with PWM Control

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SerialGUISample
{
    public partial class Form1 : Form
    {
        // Serial communication variables
        bool runSerial = true;
        byte[] Outputs = new byte[4];
        byte[] Inputs = new byte[6];
        const byte START = 255;
        const byte ZERO = 0;

        // Command constants
        const byte START_CMD = 4;
        const byte STOP_CMD = 5;
        const byte REVERSE_CMD = 6;
        const byte SET_FAST_PWM = 7;
        const byte SET_SLOW_PWM = 8;

        // Sensor and motor values
        int leftSensor = 0;
        int rightSensor = 0;
        int leftMotor = 128;
        int rightMotor = 128;

        // PWM values (0-100%)
        int fastPWM = 85;  // Default 85%
        int slowPWM = 75;  // Default 75%

        // State tracking
        bool isRunning = false;
        bool isReverse = false;
        DateTime startTime;
        TimeSpan elapsedTime;

        // Chart data
        Queue<int> leftSensorHistory = new Queue<int>();
        Queue<int> rightSensorHistory = new Queue<int>();
        Queue<int> leftMotorHistory = new Queue<int>();
        Queue<int> rightMotorHistory = new Queue<int>();
        const int MAX_CHART_POINTS = 100;

        public Form1()
        {
            InitializeComponent();
            InitializeChart();

            // Set initial slider values
            sliderFastPWM.Value = fastPWM;
            sliderSlowPWM.Value = slowPWM;
            lblFastPWMValue.Text = fastPWM.ToString() + "%";
            lblSlowPWMValue.Text = slowPWM.ToString() + "%";

            // Initialize timer display
            lblTimerValue.Text = "00:00:00";

            // Initialize motor displays
            txtLeftMotor.Text = leftMotor.ToString();
            txtRightMotor.Text = rightMotor.ToString();

            // Establish serial connection
            if (runSerial == true)
            {
                if (!serial.IsOpen)
                {
                    try
                    {
                        serial.Open();
                        statusBox.Text = "Connected to " + serial.PortName;
                        statusBox.BackColor = Color.LightGreen;
                    }
                    catch
                    {
                        statusBox.Enabled = false;
                        statusBox.Text = "ERROR: Failed to connect.";
                        statusBox.BackColor = Color.LightCoral;
                    }
                }
            }

            // Set initial UI state
            UpdateUIState();
        }

        private void InitializeChart()
        {
            chartPWM.Series.Clear();
            chartPWM.ChartAreas.Clear();

            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.AxisX.Title = "Time";
            chartArea.AxisY.Title = "Value (0-255)";
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Maximum = 255;
            chartArea.BackColor = Color.FromArgb(240, 240, 255);
            chartPWM.ChartAreas.Add(chartArea);

            // Left Motor series
            Series leftMotorSeries = new Series("Left Motor");
            leftMotorSeries.ChartType = SeriesChartType.Line;
            leftMotorSeries.Color = Color.Blue;
            leftMotorSeries.BorderWidth = 2;
            chartPWM.Series.Add(leftMotorSeries);

            // Right Motor series
            Series rightMotorSeries = new Series("Right Motor");
            rightMotorSeries.ChartType = SeriesChartType.Line;
            rightMotorSeries.Color = Color.Red;
            rightMotorSeries.BorderWidth = 2;
            chartPWM.Series.Add(rightMotorSeries);

            chartPWM.Legends.Clear();
            Legend legend = new Legend();
            legend.Docking = Docking.Top;
            chartPWM.Legends.Add(legend);
        }

        private void UpdateChart()
        {
            // Add current values to history
            leftMotorHistory.Enqueue(leftMotor);
            rightMotorHistory.Enqueue(rightMotor);

            // Maintain max points
            if (leftMotorHistory.Count > MAX_CHART_POINTS)
                leftMotorHistory.Dequeue();
            if (rightMotorHistory.Count > MAX_CHART_POINTS)
                rightMotorHistory.Dequeue();

            // Update chart
            chartPWM.Series["Left Motor"].Points.Clear();
            chartPWM.Series["Right Motor"].Points.Clear();

            int index = 0;
            foreach (int val in leftMotorHistory)
            {
                chartPWM.Series["Left Motor"].Points.AddXY(index, val);
                index++;
            }

            index = 0;
            foreach (int val in rightMotorHistory)
            {
                chartPWM.Series["Right Motor"].Points.AddXY(index, val);
                index++;
            }
        }

        private void sendIO(byte PORT, byte DATA)
        {
            Outputs[0] = START;
            Outputs[1] = PORT;
            Outputs[2] = DATA;
            Outputs[3] = (byte)(START + PORT + DATA);

            if (serial.IsOpen)
            {
                serial.Write(Outputs, 0, 4);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                isRunning = true;
                startTime = DateTime.Now;
                timerElapsed.Enabled = true;

                if (isReverse)
                {
                    sendIO(REVERSE_CMD, ZERO);
                    statusBox.Text = "RUNNING - Reverse Mode";
                }
                else
                {
                    sendIO(START_CMD, ZERO);
                    statusBox.Text = "RUNNING - Forward Mode";
                }

                btnStart.Enabled = false;
                btnStop.Enabled = true;
                UpdateUIState();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                isRunning = false;
                timerElapsed.Enabled = false;
                sendIO(STOP_CMD, ZERO);
                statusBox.Text = "STOPPED";
                
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                UpdateUIState();
            }
        }

        private void btnToggleDirection_Click(object sender, EventArgs e)
        {
            // Can only toggle when stopped
            if (!isRunning)
            {
                isReverse = !isReverse;
                UpdateUIState();
            }
            else
            {
                MessageBox.Show("Please stop the car before changing direction.",
                    "Direction Change", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateUIState()
        {
            if (isReverse)
            {
                // Blue theme for reverse
                this.BackColor = Color.FromArgb(200, 220, 255);
                panelTop.BackColor = Color.FromArgb(100, 150, 255);
                btnToggleDirection.Text = "← REVERSE MODE";
                btnToggleDirection.BackColor = Color.LightBlue;
                lblMode.Text = "REVERSE";
                lblMode.ForeColor = Color.Blue;
            }
            else
            {
                // Green theme for forward
                this.BackColor = Color.FromArgb(220, 255, 220);
                panelTop.BackColor = Color.FromArgb(100, 200, 100);
                btnToggleDirection.Text = "FORWARD MODE →";
                btnToggleDirection.BackColor = Color.LightGreen;
                lblMode.Text = "FORWARD";
                lblMode.ForeColor = Color.DarkGreen;
            }

            if (isRunning)
            {
                statusBox.BackColor = isReverse ? Color.LightBlue : Color.LightGreen;
            }
            else
            {
                statusBox.BackColor = Color.LightGray;
            }
        }

        private void sliderFastPWM_Scroll(object sender, EventArgs e)
        {
            fastPWM = sliderFastPWM.Value;
            lblFastPWMValue.Text = fastPWM.ToString() + "%";

            // Send to Arduino
            byte pwmByte = (byte)((fastPWM * 255) / 100);
            sendIO(SET_FAST_PWM, pwmByte);
        }

        private void sliderSlowPWM_Scroll(object sender, EventArgs e)
        {
            slowPWM = sliderSlowPWM.Value;
            lblSlowPWMValue.Text = slowPWM.ToString() + "%";

            // Send to Arduino
            byte pwmByte = (byte)((slowPWM * 255) / 100);
            sendIO(SET_SLOW_PWM, pwmByte);
        }

        private void timerElapsed_Tick(object sender, EventArgs e)
        {
            if (isRunning)
            {
                elapsedTime = DateTime.Now - startTime;
                lblTimerValue.Text = string.Format("{0:00}:{1:00}:{2:00}",
                    (int)elapsedTime.TotalMinutes,
                    elapsedTime.Seconds,
                    elapsedTime.Milliseconds / 10);
            }
        }

        private void getIOtimer_Tick(object sender, EventArgs e)
        {
            if (serial.IsOpen)
            {
                try
                {
                    while (serial.BytesToRead > 0)
                    {
                        byte firstByte = (byte)serial.ReadByte();

                        if (firstByte == START)
                        {
                            if (serial.BytesToRead >= 5)
                            {
                                Inputs[0] = firstByte;
                                Inputs[1] = (byte)serial.ReadByte(); // Left Sensor
                                Inputs[2] = (byte)serial.ReadByte(); // Right Sensor
                                Inputs[3] = (byte)serial.ReadByte(); // Left Motor
                                Inputs[4] = (byte)serial.ReadByte(); // Right Motor
                                Inputs[5] = (byte)serial.ReadByte(); // Checksum

                                byte checkSum = (byte)(Inputs[0] + Inputs[1] + Inputs[2] + Inputs[3] + Inputs[4]);

                                if (Inputs[5] == checkSum)
                                {
                                    leftSensor = Inputs[1];
                                    rightSensor = Inputs[2];
                                    leftMotor = Inputs[3];
                                    rightMotor = Inputs[4];

                                    // Update display
                                    txtLeftSensor.Text = leftSensor.ToString();
                                    txtRightSensor.Text = rightSensor.ToString();
                                    txtLeftMotor.Text = leftMotor.ToString();
                                    txtRightMotor.Text = rightMotor.ToString();

                                    // Update chart
                                    UpdateChart();
                                }

                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    statusBox.Text = "Serial Error: " + ex.Message;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
        }

        private void lblLeftSensor_Click(object sender, EventArgs e)
        {

        }
    }
}