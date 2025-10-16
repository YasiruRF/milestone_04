//Curtin University
//Mechatronics Engineering
//Line Follower with Bang-Bang Control + GUI Remote Control

// Serial communication protocol: <startByte><commandByte><dataByte><checkByte>

//Declare variables for storing the port values. 
byte output1 = 255;  // Left motor speed
byte output2 = 255;  // Right motor speed
byte input1 = 0;     // Left IR sensor
byte input2 = 0;     // Right IR sensor

//Declare variables for each byte of the message.
byte startByte = 0;
byte commandByte = 0;
byte dataByte = 0;
byte checkByte = 0;

//Declare variable for calculating the check sum
byte checkSum = 0;

//Declare a constant for the start byte
const byte START = 255;

//Declare constants to enumerate the port values.
const byte INPUT1 = 0;   // Left sensor
const byte INPUT2 = 1;   // Right sensor
const byte OUTPUT1 = 2;  // Left motor
const byte OUTPUT2 = 3;  // Right motor

// Pin definitions for DACs and sensors
const byte DACPIN1[8] = {9, 8, 7, 6, 5, 4, 3, 2}; 
const byte DACPIN2[8] = {A2, A3, A4, A5, A1, A0, 11, 10};
const byte SENSOR1 = A6;  // Left IR sensor
const byte SENSOR2 = A7;  // Right IR sensor

// Line follower control parameters
// Motor speeds (0-255 range, 128 is stop, 255 is full forward, 0 is full reverse)
const byte MOTOR_STOP = 128;
const byte MOTOR_FAST = 220;   // Fast forward speed
const byte MOTOR_SLOW = 200;   // Slow forward speed (for turning)
const byte MOTOR_REVERSE = 50;      // Fast reverse speed
const byte MOTOR_REVERSE_SLOW = 90; // Slow reverse speed (for turning)

// Sensor thresholds (adjust these based on your sensor readings)
// White line detection: readings between 200-300 (closer to 230)
// Black surface: readings between 700-900 (closer to 789)
const int WHITE_THRESHOLD = 500;  // Values below this = white line detected

// Timing for serial monitor updates
unsigned long lastSendTime = 0;
const unsigned long SEND_INTERVAL = 50;  // Send data every 50ms

byte lastLineSide = OUTPUT1;           // Tracks last line position for forward mode
byte lastLineSideReverse = OUTPUT1;    // Tracks last line position for reverse mode

// Command constants
const byte START_CMD = 4;
const byte STOP_CMD = 5;
const byte REVERSE_CMD = 6;

// Car state variables
int carState = 0;
const int STATE_STOPPED = 0;
const int STATE_RUNNING = 1;
const int STATE_REVERSING = 2;

// Function to output byte value to DAC1 (Left Motor)
void outputToDAC1(byte data)
{
  for(int i = 0; i <= 7; i++)
  {
    digitalWrite(DACPIN1[i], ((data >> i) & 1 ? HIGH : LOW));
  }
}

// Function to output byte value to DAC2 (Right Motor)
void outputToDAC2(byte data)
{
  for(int i = 0; i <= 7; i++)
  {
    digitalWrite(DACPIN2[i], ((data >> i) & 1 ? HIGH : LOW));
  }
}

// Initialize DAC pins as outputs
void initDACs()
{
  for(int i = 0; i <= 7; i++)
  {
    pinMode(DACPIN1[i], OUTPUT);
    pinMode(DACPIN2[i], OUTPUT);
  }
}

// Stop both motors
void stopMotors()
{
  output1 = MOTOR_STOP;
  output2 = MOTOR_STOP;
  outputToDAC1(output1);
  outputToDAC2(output2);
}

// Setup: Initialize serial communication and pins
void setup() 
{
  Serial.begin(9600);
  initDACs();
  
  // Initialize both motors to stopped position
  stopMotors();
  
  // Set initial state to stopped
  carState = STATE_STOPPED;
  
  delay(2000);  // 2 second delay before starting
  
  Serial.println("=== Line Follower Started ===");
  Serial.println("State: STOPPED");
  Serial.println("Waiting for commands from GUI...");
}

// Bang-Bang Line Following Algorithm (Forward)
void bangBangLineFollow()
{
  // Read analog values from both IR sensors
  int leftSensor = analogRead(SENSOR1);   // Left sensor
  int rightSensor = analogRead(SENSOR2);  // Right sensor
  
  // Determine if each sensor detects white line (true) or black surface (false)
  bool leftOnWhite = (leftSensor < WHITE_THRESHOLD);
  bool rightOnWhite = (rightSensor < WHITE_THRESHOLD);
  
  // Bang-Bang Control Logic:
  // Case 1: Both sensors on black - turn based on last known line position
  if (!leftOnWhite && !rightOnWhite)
  {
    if (lastLineSide == OUTPUT1)  // Last line was on left, turn left sharply
    {
      output1 = 255;        // Left motor reverse
      output2 = 128;        // Right motor forward
    }
    else  // Last line was on right, turn right sharply
    {
      output1 = 128;        // Left motor forward
      output2 = 255;        // Right motor reverse
    }
  }
  // Case 2: Left sensor on white, right on black - turn left
  else if (leftOnWhite && !rightOnWhite)
  {
    lastLineSide = OUTPUT1;
    output1 = MOTOR_FAST;  // Left motor fast
    output2 = MOTOR_SLOW;  // Right motor slow
  }
  // Case 3: Right sensor on white, left on black - turn right
  else if (!leftOnWhite && rightOnWhite)
  {
    lastLineSide = OUTPUT2;
    output1 = MOTOR_SLOW;  // Left motor slow
    output2 = MOTOR_FAST;  // Right motor fast
  }
  // Case 4: Both sensors on white - go straight
  else
  {
    output1 = MOTOR_FAST;  // Left motor forward
    output2 = MOTOR_FAST;  // Right motor forward
  }
  
  // Apply motor speeds to DACs
  outputToDAC1(output1);
  outputToDAC2(output2);
}

// Bang-Bang Line Following Algorithm (Reverse)
void bangBangLineFollowReverse()
{
  // Read analog values from both IR sensors
  int leftSensor = analogRead(SENSOR1);   // Left sensor
  int rightSensor = analogRead(SENSOR2);  // Right sensor
  
  // Determine if each sensor detects white line (true) or black surface (false)
  bool leftOnWhite = (leftSensor < WHITE_THRESHOLD);
  bool rightOnWhite = (rightSensor < WHITE_THRESHOLD);
  
  // Bang-Bang Control Logic for REVERSE (inverted motor directions):
  // Case 1: Both sensors on black - turn based on last known line position
  if (!leftOnWhite && !rightOnWhite)
  {
    if (lastLineSideReverse == OUTPUT1)  // Last line was on left, turn left sharply
    {
      output1 = 25;         // Left motor reverse (stronger)
      output2 = 128;        // Right motor stop
    }
    else  // Last line was on right, turn right sharply
    {
      output1 = 128;        // Left motor stop
      output2 = 25;         // Right motor reverse (stronger)
    }
  }
  // Case 2: Left sensor on white, right on black - turn left
  else if (leftOnWhite && !rightOnWhite)
  {
    lastLineSideReverse = OUTPUT1;
    output1 = MOTOR_REVERSE;       // Left motor fast reverse
    output2 = MOTOR_REVERSE_SLOW;  // Right motor slow reverse
  }
  // Case 3: Right sensor on white, left on black - turn right
  else if (!leftOnWhite && rightOnWhite)
  {
    lastLineSideReverse = OUTPUT2;
    output1 = MOTOR_REVERSE_SLOW;  // Left motor slow reverse
    output2 = MOTOR_REVERSE;       // Right motor fast reverse
  }
  // Case 4: Both sensors on white - go straight in reverse
  else
  {
    output1 = MOTOR_REVERSE;  // Left motor reverse
    output2 = MOTOR_REVERSE;  // Right motor reverse
  }
  
  // Apply motor speeds to DACs
  outputToDAC1(output1);
  outputToDAC2(output2);
}

// Send sensor and motor data to GUI (called continuously)
void sendDataToGUI()
{
  if (millis() - lastSendTime >= SEND_INTERVAL)
  {
    // Read current sensor values and scale them down (10-bit to 8-bit)
    byte leftSensorVal = analogRead(SENSOR1) >> 2;
    byte rightSensorVal = analogRead(SENSOR2) >> 2;
    
    // Calculate checksum
    byte checksum = (byte)(START + leftSensorVal + rightSensorVal + output1 + output2);
    
    // Send 6-byte message: START, LeftSensor, RightSensor, LeftMotor, RightMotor, Checksum
    Serial.write(START);
    Serial.write(leftSensorVal);
    Serial.write(rightSensorVal);
    Serial.write(output1);
    Serial.write(output2);
    Serial.write(checksum);
    
    lastSendTime = millis();
  }
}

// FIXED: Handle serial commands from GUI with proper 4-byte protocol
void handleSerialCommands()
{
  // Only try to read commands if we have at least 4 bytes available
  if (Serial.available() >= 4)
  {
    byte receivedStartByte = Serial.peek();  // Peek at first byte without consuming it
    
    // Check if this looks like a command (starts with START byte 255)
    // Commands have format: 255, 4/5/6, 0, checksum
    if (receivedStartByte == START)
    {
      // Now consume the bytes
      Serial.read();  // START
      byte receivedCommandByte = Serial.read();
      byte receivedDataByte = Serial.read();
      byte receivedChecksum = Serial.read();
      
      // Calculate checksum to verify message integrity
      byte calculatedChecksum = (byte)(START + receivedCommandByte + receivedDataByte);
      
      if (receivedChecksum == calculatedChecksum)
      {
        // Checksum valid - process the command
        switch(receivedCommandByte)
        {
          case START_CMD:  // Command 4
            carState = STATE_RUNNING;
            lastLineSide = OUTPUT1;  // Reset forward tracker
            Serial.println("Command: START - Car now following line forward");
            break;
            
          case STOP_CMD:   // Command 5
            carState = STATE_STOPPED;
            Serial.println("Command: STOP - Car stopped");
            break;
            
          case REVERSE_CMD:  // Command 6
            carState = STATE_REVERSING;
            lastLineSideReverse = OUTPUT1;  // Reset reverse tracker
            Serial.println("Command: REVERSE - Car following line in reverse");
            break;
            
          default:
            // Unknown command, ignore
            Serial.print("Unknown command received: ");
            Serial.println(receivedCommandByte);
            break;
        }
      }
      else
      {
        Serial.print("Checksum mismatch - Expected: ");
        Serial.print(calculatedChecksum);
        Serial.print(", Received: ");
        Serial.println(receivedChecksum);
      }
    }
  }
}

// Main loop: Handle serial communication and line following
void loop() 
{
  // Handle commands from GUI
  handleSerialCommands();
  
  // Always send sensor and motor data to GUI
  sendDataToGUI();
  
  // Execute behavior based on carState
  switch(carState)
  {
    case STATE_RUNNING:
      bangBangLineFollow();
      break;
      
    case STATE_STOPPED:
      stopMotors();
      break;
      
    case STATE_REVERSING:
      // Follow line in reverse
      bangBangLineFollowReverse();
      break;
  }

  delay(1);
}

// Function to reverse the order of the bits (utility function)
byte bitFlip(byte value)
{
  byte bFlip = 0;
  byte j = 7;
  for (byte i = 0; i < 8; i++)
  { 
    bitWrite(bFlip, i, bitRead(value, j));
    j--;
  }
  return bFlip;
}