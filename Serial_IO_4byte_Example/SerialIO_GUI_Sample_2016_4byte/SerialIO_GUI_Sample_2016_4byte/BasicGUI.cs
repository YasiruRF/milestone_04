// Curtin University
// Mechatronics Engineering
// Serial I/O Card - Sample GUI Code

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SerialGUISample
{

    public partial class Form1 : Form
    {
        // Declare variables to store inputs and outputs.
        bool runSerial = true;
        bool byteRead = false;
        int Input1 = 0;
        int Input2 = 0;
        int leftMotor = 0;
        int rightMotor = 0;

        byte[] Outputs = new byte[4];
        byte[] Inputs = new byte[6];
        const byte START = 255;
        const byte ZERO = 0;
        const byte START_CMD = 4;
        const byte STOP_CMD = 5;
        const byte REVERSE_CMD = 6;


        public Form1()
        {
            // Initialize required for form controls.
            InitializeComponent();

            // Establish connection with serial
            if (runSerial == true)
            {
                if (!serial.IsOpen)                                  // Check if the serial has been connected.
                {
                    try
                    {
                        serial.Open();                               //Try to connect to the serial.
                        statusBox.Text = "Connected to " + serial.PortName;
                    }
                    catch
                    {
                        statusBox.Enabled = false;
                        statusBox.Text = "ERROR: Failed to connect.";     //If the serial does not connect return an error.
                    }
                }
            }
        }

        // Send a four byte message to the Arduino via serial.
        private void sendIO(byte PORT, byte DATA)
        {
            Outputs[0] = START;    //Set the first byte to the start value that indicates the beginning of the message.
            Outputs[1] = PORT;     //Set the second byte to represent the port where, Input 1 = 0, Input 2 = 1, Output 1 = 2 & Output 2 = 3.
            Outputs[2] = DATA;     //Set the third byte to the value to be assigned to the port.
            Outputs[3] = (byte)(START + PORT + DATA); //Calculate the checksum byte.

            if (serial.IsOpen)
            {
                serial.Write(Outputs, 0, 4);         //Send all four bytes to the Arduino.                      
            }
        }

        private byte Maptobyte(decimal value) //to map the value from -15 to 15 into a byte value from 0 to 255.
        {
            double scaled = ((double)value * 8.5) + 127.5;
            return (byte)Math.Round(scaled);
        }

        private string ToBinaryStrong(byte value) //to convert a byte value to a binary string for display.
        {
            return Convert.ToString(value, 2).PadLeft(8, '0');
        }

        // Map 0-100% to 0-255
        private byte MapDutyCycle(decimal value)
        {
            double scaled = ((double)value * 2.55);
            return (byte)Math.Round(scaled);
        }

        private void Send1_Click(object sender, EventArgs e) //Press the button to send the value to Output 1, Arduino Port A.
        {
        }

        private void Send2_Click(object sender, EventArgs e) //Press the button to send the value to Output 2, Arduino Port C.
        {
        }

        private void getIOtimer_Tick(object sender, EventArgs e) //Continuously check for incoming data
        {
            if (serial.IsOpen) //Check that a serial connection exists.
            {
                try
                {
                    // Look for START byte (255) at the beginning of buffer
                    while (serial.BytesToRead > 0)
                    {
                        byte firstByte = (byte)serial.ReadByte();

                        if (firstByte == START) // Found start byte
                        {
                            // Check if we have all 6 bytes now
                            if (serial.BytesToRead >= 5)
                            {
                                Inputs[0] = firstByte;
                                Inputs[1] = (byte)serial.ReadByte(); // Left Sensor
                                Inputs[2] = (byte)serial.ReadByte(); // Right Sensor
                                Inputs[3] = (byte)serial.ReadByte(); // Left Motor
                                Inputs[4] = (byte)serial.ReadByte(); // Right Motor
                                Inputs[5] = (byte)serial.ReadByte(); // Checksum

                                // Calculate checksum: START + LeftSensor + RightSensor + LeftMotor + RightMotor
                                byte checkSum = (byte)(Inputs[0] + Inputs[1] + Inputs[2] + Inputs[3] + Inputs[4]);

                                // Verify checksum
                                if (Inputs[5] == checkSum)
                                {
                                    Input1 = Inputs[1];
                                    Input2 = Inputs[2];
                                    leftMotor = Inputs[3];
                                    rightMotor = Inputs[4];

                                    // Update display
                                    txtLeftSensor.Text = Input1.ToString();
                                    txtRightSensor.Text = Input2.ToString();
                                    txtLeftMotor.Text = leftMotor.ToString();
                                    txtRightMotor.Text = rightMotor.ToString();

                                    statusBox.Text = "Data received OK";
                                }
                                else
                                {
                                    statusBox.Text = "Checksum error";
                                }

                                break; // Exit while loop after processing one complete message
                            }
                            else
                            {
                                // Not enough bytes yet, wait for more data
                                break;
                            }
                        }
                        // If not START byte, continue loop to search for next START byte
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

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            sendIO(START_CMD, ZERO);
            statusBox.Text = "Motors Started";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            sendIO(STOP_CMD, ZERO);
            statusBox.Text = "Motors Stopped";
        }

        private void btnReverse_Click(object sender, EventArgs e)
        {
            sendIO(REVERSE_CMD, ZERO);
            statusBox.Text = "Motors Reversed";
        }

        private void Get1_Click(object sender, EventArgs e)
        {
            sendIO(0, ZERO);
        }

        private void Get2_Click(object sender, EventArgs e)
        {
            sendIO(1, ZERO);
        }

        private void lblRightSensor_Click(object sender, EventArgs e)
        {

        }

        private void lblRightMotor_Click(object sender, EventArgs e)
        {

        }

        private void txtLeftMotor_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblLeftMotor_Click(object sender, EventArgs e)
        {

        }

        private void txtRightSensor_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRightMotor_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLeftSensor_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblLeftSensor_Click(object sender, EventArgs e)
        {

        }
    }

}