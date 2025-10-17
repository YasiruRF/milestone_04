// Curtin University
// Mechatronics Engineering
// Enhanced Line Follower GUI with Gemini AI Integration

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

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
        const byte AI_MODE_CMD = 9;
        const byte MANUAL_MODE_CMD = 10;
        const byte AI_MOTOR_CMD = 11;

        // Sensor and motor values
        int leftSensor = 0;
        int rightSensor = 0;
        int leftMotor = 128;
        int rightMotor = 128;

        // PWM values (0-100%)
        int fastPWM = 85;
        int slowPWM = 75;

        // State tracking
        bool isRunning = false;
        bool isReverse = false;
        bool isAIMode = false;
        DateTime startTime;
        TimeSpan elapsedTime;

        // Chart data
        Queue<int> leftSensorHistory = new Queue<int>();
        Queue<int> rightSensorHistory = new Queue<int>();
        Queue<int> leftMotorHistory = new Queue<int>();
        Queue<int> rightMotorHistory = new Queue<int>();
        const int MAX_CHART_POINTS = 100;

        // AI Training metrics
        Queue<double> aiErrorHistory = new Queue<double>();
        Queue<double> aiConfidenceHistory = new Queue<double>();
        const int MAX_AI_CHART_POINTS = 200;
        double currentAIError = 0;
        double averageAIError = 0;
        int aiDecisionCount = 0;

        // AI Training data
        List<TrainingData> trainingDataset = new List<TrainingData>();
        const int MAX_TRAINING_SAMPLES = 1000;
        bool isCollectingData = false;

        // Gemini API
        private string geminiApiKey = "AIzaSyCvFwV1_8CkW6UjjK8p8oxtp8FjAsjHaf4"; // Replace with your actual API key
        private HttpClient httpClient = new HttpClient();
        private System.Windows.Forms.Timer aiControlTimer;
        private int aiUpdateInterval = 100; // AI decision every 100ms

        public Form1()
        {
            InitializeComponent();
            InitializeChart();
            InitializeAI();

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

        private void InitializeAI()
        {
            // Initialize AI control timer
            aiControlTimer = new System.Windows.Forms.Timer();
            aiControlTimer.Interval = aiUpdateInterval;
            aiControlTimer.Tick += AiControlTimer_Tick;

            // Try to load existing training data
            LoadTrainingData();
        }

        private async void AiControlTimer_Tick(object sender, EventArgs e)
        {
            if (isAIMode && isRunning)
            {
                await GetAIDecision();
            }
        }

        private async Task GetAIDecision()
        {
            try
            {
                // Prepare context for Gemini
                string context = PrepareAIContext();

                // Get decision from Gemini
                var decision = await QueryGeminiAPI(context);

                // Parse and apply decision
                ApplyAIDecision(decision);

                // Update AI status
                lblAIStatus.Text = "AI Active: Processing";
                lblAIStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblAIStatus.Text = "AI Error: " + ex.Message;
                lblAIStatus.ForeColor = Color.Red;
            }
        }

        private string PrepareAIContext()
        {
            // Get recent training data summary
            string trainingContext = "";
            if (trainingDataset.Count > 0)
            {
                var recentData = trainingDataset.Skip(Math.Max(0, trainingDataset.Count - 50)).ToList();
                trainingContext = $"\n\nRecent training examples ({recentData.Count} samples):\n";
                foreach (var data in recentData)
                {
                    trainingContext += $"Left Sensor: {data.LeftSensor}, Right Sensor: {data.RightSensor} -> " +
                                     $"Left Motor: {data.LeftMotor}, Right Motor: {data.RightMotor}\n";
                }
            }

            string prompt = $@"You are controlling a line-following robot car. Your task is to decide motor speeds based on sensor readings.

CURRENT SENSOR READINGS:
- Left IR Sensor: {leftSensor} (0-255, lower values = white line detected)
- Right IR Sensor: {rightSensor} (0-255, lower values = white line detected)
- White threshold: ~100 (values below indicate white line)

MOTOR CONTROL:
- Motor values range: 0-255
- 128 = STOP
- >128 = Forward (255 = max forward)
- <128 = Reverse (0 = max reverse)
- Current Left Motor: {leftMotor}
- Current Right Motor: {rightMotor}

CONTROL STRATEGY:
- When both sensors detect black (high values): Turn sharply toward last known line position
- When left sensor detects white: Turn gently left (slow left motor, fast right motor)
- When right sensor detects white: Turn gently right (fast left motor, slow right motor)
- When both detect white: Go straight at full speed

AVAILABLE PWM VALUES:
- Fast PWM: {fastPWM}% (for straight/faster wheel)
- Slow PWM: {slowPWM}% (for turning/slower wheel)
{trainingContext}

Based on the current sensor readings and training data, provide motor control commands.
Respond ONLY with two numbers separated by a comma: leftMotorSpeed,rightMotorSpeed
Example response: 200,180

Your response:";

            return prompt;
        }

        private async Task<string> QueryGeminiAPI(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.3,
                        maxOutputTokens = 100
                    }
                };

                string jsonRequest = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key={geminiApiKey}";

                var response = await httpClient.PostAsync(url, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API Error: {response.StatusCode} - {responseBody}");
                }

                dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
                string aiResponse = jsonResponse.candidates[0].content.parts[0].text.ToString().Trim();

                return aiResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Gemini API call failed: {ex.Message}");
            }
        }

        private void ApplyAIDecision(string decision)
        {
            try
            {
                // Parse AI response (expecting format: "leftMotor,rightMotor")
                string cleanDecision = decision.Replace(" ", "").Split('\n')[0];
                string[] values = cleanDecision.Split(',');

                if (values.Length == 2)
                {
                    byte leftMotorValue = byte.Parse(values[0]);
                    byte rightMotorValue = byte.Parse(values[1]);

                    // Clamp values to valid range
                    leftMotorValue = (byte)Math.Max(0, Math.Min(255, (int)leftMotorValue));
                    rightMotorValue = (byte)Math.Max(0, Math.Min(255, (int)rightMotorValue));

                    // Send to Arduino
                    SendAIMotorCommand(leftMotorValue, rightMotorValue);

                    // Update display
                    txtAIDecision.Text = $"L:{leftMotorValue} R:{rightMotorValue}";
                }
            }
            catch (Exception ex)
            {
                lblAIStatus.Text = "Parse Error: " + ex.Message;
            }
        }

        private void SendAIMotorCommand(byte leftMotor, byte rightMotor)
        {
            // Send left motor command
            sendIO(AI_MOTOR_CMD, leftMotor);
            System.Threading.Thread.Sleep(5);

            // Send right motor command (using command+1)
            sendIO((byte)(AI_MOTOR_CMD + 1), rightMotor);
        }

        private void btnAIMode_Click(object sender, EventArgs e)
        {
            if (!isAIMode)
            {
                // Validate API key
                if (geminiApiKey == "YOUR_API_KEY_HERE" || string.IsNullOrEmpty(geminiApiKey))
                {
                    MessageBox.Show("Please set your Gemini API key in the code before using AI mode.",
                        "API Key Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Switch to AI mode
                isAIMode = true;
                sendIO(AI_MODE_CMD, ZERO);
                aiControlTimer.Start();

                btnAIMode.Text = "🤖 AI MODE: ON";
                btnAIMode.BackColor = Color.LightBlue;
                panelAI.BackColor = Color.FromArgb(200, 230, 255);
                lblAIStatus.Text = "AI Mode: Active";
                lblAIStatus.ForeColor = Color.Green;

                statusBox.Text = "AI Mode Activated";
            }
            else
            {
                // Switch to manual mode
                isAIMode = false;
                sendIO(MANUAL_MODE_CMD, ZERO);
                aiControlTimer.Stop();

                btnAIMode.Text = "🤖 AI MODE: OFF";
                btnAIMode.BackColor = Color.LightGray;
                panelAI.BackColor = Color.FromArgb(240, 240, 240);
                lblAIStatus.Text = "AI Mode: Inactive";
                lblAIStatus.ForeColor = Color.Gray;

                statusBox.Text = "Manual Mode Activated";
            }
        }

        private void btnStartTraining_Click(object sender, EventArgs e)
        {
            if (!isCollectingData)
            {
                isCollectingData = true;
                btnStartTraining.Text = "⏸ Stop Training";
                btnStartTraining.BackColor = Color.LightCoral;
                lblTrainingStatus.Text = $"Collecting: {trainingDataset.Count}/{MAX_TRAINING_SAMPLES}";
                lblTrainingStatus.ForeColor = Color.Green;
            }
            else
            {
                isCollectingData = false;
                btnStartTraining.Text = "▶ Start Training";
                btnStartTraining.BackColor = Color.LightGreen;
                lblTrainingStatus.Text = $"Stopped: {trainingDataset.Count} samples";
                lblTrainingStatus.ForeColor = Color.Gray;

                // Save training data
                SaveTrainingData();
            }
        }

        private void btnClearTraining_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show($"Clear all {trainingDataset.Count} training samples?",
                "Clear Training Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                trainingDataset.Clear();
                lblTrainingStatus.Text = "Training data cleared";
                lblTrainingStatus.ForeColor = Color.Gray;
            }
        }

        private void CollectTrainingData()
        {
            if (isCollectingData && isRunning && !isAIMode && trainingDataset.Count < MAX_TRAINING_SAMPLES)
            {
                var data = new TrainingData
                {
                    Timestamp = DateTime.Now,
                    LeftSensor = leftSensor,
                    RightSensor = rightSensor,
                    LeftMotor = leftMotor,
                    RightMotor = rightMotor,
                    IsReverse = isReverse
                };

                trainingDataset.Add(data);
                lblTrainingStatus.Text = $"Collecting: {trainingDataset.Count}/{MAX_TRAINING_SAMPLES}";

                if (trainingDataset.Count >= MAX_TRAINING_SAMPLES)
                {
                    isCollectingData = false;
                    btnStartTraining.Text = "▶ Start Training";
                    btnStartTraining.BackColor = Color.LightGreen;
                    lblTrainingStatus.Text = $"Complete: {trainingDataset.Count} samples";
                    SaveTrainingData();
                    MessageBox.Show("Training data collection complete!", "Training Complete",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void SaveTrainingData()
        {
            try
            {
                string json = JsonConvert.SerializeObject(trainingDataset, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText("training_data.json", json);
                MessageBox.Show($"Saved {trainingDataset.Count} training samples.", "Save Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save training data: {ex.Message}", "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTrainingData()
        {
            try
            {
                if (File.Exists("training_data.json"))
                {
                    string json = File.ReadAllText("training_data.json");
                    trainingDataset = JsonConvert.DeserializeObject<List<TrainingData>>(json);
                    lblTrainingStatus.Text = $"Loaded: {trainingDataset.Count} samples";
                }
            }
            catch (Exception  ex)
            {
                lblTrainingStatus.Text = "Failed to load training data";
                // Optionally log ex.Message or ex.ToString() for debugging
                // Example: Console.WriteLine(ex);
            }
        }

        // Rest of the original methods...
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

            Series leftMotorSeries = new Series("Left Motor");
            leftMotorSeries.ChartType = SeriesChartType.Line;
            leftMotorSeries.Color = Color.Blue;
            leftMotorSeries.BorderWidth = 2;
            chartPWM.Series.Add(leftMotorSeries);

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
            leftMotorHistory.Enqueue(leftMotor);
            rightMotorHistory.Enqueue(rightMotor);

            if (leftMotorHistory.Count > MAX_CHART_POINTS)
                leftMotorHistory.Dequeue();
            if (rightMotorHistory.Count > MAX_CHART_POINTS)
                rightMotorHistory.Dequeue();

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
                this.BackColor = Color.FromArgb(200, 220, 255);
                panelTop.BackColor = Color.FromArgb(100, 150, 255);
                btnToggleDirection.Text = "← REVERSE MODE";
                btnToggleDirection.BackColor = Color.LightBlue;
                lblMode.Text = "REVERSE";
                lblMode.ForeColor = Color.Blue;
            }
            else
            {
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
            byte pwmByte = (byte)((fastPWM * 255) / 100);
            sendIO(SET_FAST_PWM, pwmByte);
        }

        private void sliderSlowPWM_Scroll(object sender, EventArgs e)
        {
            slowPWM = sliderSlowPWM.Value;
            lblSlowPWMValue.Text = slowPWM.ToString() + "%";
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
                                Inputs[1] = (byte)serial.ReadByte();
                                Inputs[2] = (byte)serial.ReadByte();
                                Inputs[3] = (byte)serial.ReadByte();
                                Inputs[4] = (byte)serial.ReadByte();
                                Inputs[5] = (byte)serial.ReadByte();

                                byte checkSum = (byte)(Inputs[0] + Inputs[1] + Inputs[2] + Inputs[3] + Inputs[4]);

                                if (Inputs[5] == checkSum)
                                {
                                    leftSensor = Inputs[1];
                                    rightSensor = Inputs[2];
                                    leftMotor = Inputs[3];
                                    rightMotor = Inputs[4];

                                    txtLeftSensor.Text = leftSensor.ToString();
                                    txtRightSensor.Text = rightSensor.ToString();
                                    txtLeftMotor.Text = leftMotor.ToString();
                                    txtRightMotor.Text = rightMotor.ToString();

                                    UpdateChart();

                                    // Collect training data if enabled
                                    CollectTrainingData();
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
    }

    // Training data structure
    public class TrainingData
    {
        public DateTime Timestamp { get; set; }
        public int LeftSensor { get; set; }
        public int RightSensor { get; set; }
        public int LeftMotor { get; set; }
        public int RightMotor { get; set; }
        public bool IsReverse { get; set; }
    }
}