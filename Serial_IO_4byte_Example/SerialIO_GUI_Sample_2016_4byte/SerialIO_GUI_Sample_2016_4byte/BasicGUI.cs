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

        byte[] Outputs = new byte[4];
        byte[] Inputs = new byte[4];

        const byte START = 255;
        const byte ZERO = 0;
        

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
                    }
                    catch
                    {
                        statusBox.Enabled = false;
                        statusBox.Text ="ERROR: Failed to connect.";     //If the serial does not connect return an error.
                    }
                }
            }
        }

        // Send a four byte message to the Arduino via serial.
        private void sendIO(byte PORT, byte DATA)
        {
            Outputs[0] = START;    //Set the first byte to the start value that indicates the beginning of the message.
            Outputs[1] = PORT;     //Set the second byte to represent the port where, Input 1 = 0, Input 2 = 1, Output 1 = 2 & Output 2 = 3. This could be enumerated to make writing code simpler... (see Arduino driver)
            Outputs[2] = DATA;  //Set the third byte to the value to be assigned to the port. This is only necessary for outputs, however it is best to assign a consistent value such as 0 for input ports.
            Outputs[3] = (byte)(START + PORT + DATA); //Calculate the checksum byte, the same calculation is performed on the Arduino side to confirm the message was received correctly.

            if (serial.IsOpen)
            {
                serial.Write(Outputs, 0, 4);         //Send all four bytes to the IO card.                      
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
            return(byte)Math.Round(scaled);

           
        }

        // Alternative method to map -100% to +100% to -15 to +15 then to 0-255
        //private byte MapDutyCycle(decimal percent)
        //{
        //    // Map 0–100% -> -15 to +15
        //    double mappedVal = -15 + (double)percent / 100.0 * 30.0;

        //    // Map -15..+15 -> 0–255
        //    double scaled = ((mappedVal + 15) / 30.0) * 255.0;

        //    return (byte)Math.Round(scaled);
        //}




        private void Send1_Click(object sender, EventArgs e) //Press the button to send the value to Output 1, Arduino Port A.
        {
            byte mappedDutyCycle = MapDutyCycle(OutputBox1.Value);
            sendIO(2, mappedDutyCycle);
        }

        private void Send2_Click(object sender, EventArgs e) //Press the button to send the value to Output 2, Arduino Port C.
        {
            byte mappedDutyCycle = MapDutyCycle(OutputBox2.Value);
            sendIO(3, mappedDutyCycle);
        }

        private void Get1_Click(object sender, EventArgs e) //Press the button to request value from Input 1, Arduino Port F.
        {
            sendIO(0, ZERO);  // The value 0 indicates Input 1, ZERO just maintains a fixed value for the discarded data in order to maintain a consistent package format.
        }

        private void Get2_Click(object sender, EventArgs e) //Press the button to request value from Input 1, Arduino Port K.
        {
            sendIO(1, ZERO);  // The value 1 indicates Input 2, ZERO maintains a consistent value for the message output.
        }

        private void getIOtimer_Tick(object sender, EventArgs e) //It is best to continuously check for incoming data as handling the buffer or waiting for event is not practical in C#.
        {
            if (serial.IsOpen) //Check that a serial connection exists.
            {
                if (serial.BytesToRead >= 4) //Check that the buffer contains a full four byte package.
                {
                    //statusBox.Text = "Incoming"; // A status box can be used for debugging code.
                    Inputs[0] = (byte)serial.ReadByte(); //Read the first byte of the package.

                    if (Inputs[0] == START) //Check that the first byte is in fact the start byte.
                    {
                        //statusBox.Text = "Start Accepted";

                        //Read the rest of the package.
                        Inputs[1] = (byte)serial.ReadByte();
                        Inputs[2] = (byte)serial.ReadByte();
                        Inputs[3] = (byte)serial.ReadByte();

                        //Calculate the checksum.
                        byte checkSum = (byte)(Inputs[0] + Inputs[1] + Inputs[2]);

                        //Check that the calculated check sum matches the checksum sent with the message.
                        if (Inputs[3] == checkSum)
                        {
                            //statusBox.Text = "CheckSum Accepted";

                            //Check which port the incoming data is associated with.
                            switch (Inputs[1])
                            {
                                case 0: //Save the data to a variable and place in the textbox.
                                    //statusBox.Text = "Input1";
                                    Input1 = Inputs[2];
                                    InputBox1.Text = Input1.ToString();
                                    break;
                                case 1: //Save the data to a variable and place in the textbox. 
                                    //statusBox.Text = "Input2";
                                    Input2 = Inputs[2];
                                    InputBox2.Text = Input2.ToString();
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    
}
