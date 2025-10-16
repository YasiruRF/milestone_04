//Curtin University
//Mechatronics Engineering
//Enhanced Line Follower with PWM Control + GUI Communication

// Serial communication protocol: <startByte><commandByte><dataByte><checkByte>

//Declare variables for storing the port values. 
byte output1 = 128;  // Left motor speed (128 = stop)
byte output2 = 128;  // Right motor speed (128 = stop)
byte input1 = 0;     // Left IR sensor
byte input2 = 0;     // Right IR sensor

//Declare variables for each byte of the message.
byte startByte = 0;
byte commandByte = 0;
byte dataByte = 0;
byte checkByte = 0;
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
const byte SENSOR1 = A7;  // Left IR sensor
const byte SENSOR2 = A6;  // Right IR sensor

// Motor speeds (0-255 range, 128 is stop, >128 forward, <128 reverse)
const byte MOTOR_STOP = 128;

// PWM values (adjustable from GUI)
byte fastPWM = 245;      // Default 85% = 217
byte slowPWM = 225;      // Default 75% = 191

// Calculate motor speeds based on PWM percentages
byte motorFast = MOTOR_STOP + fastPWM - 128;        // Forward fast
byte motorSlow = MOTOR_STOP + slowPWM - 128;        // Forward slow
byte motorReverseFast = MOTOR_STOP - (fastPWM - 128); // Reverse fast
byte motorReverseSlow = MOTOR_STOP - (slowPWM - 128); // Reverse slow

// Sensor threshold (adjust based on your sensor readings)
const int WHITE_THRESHOLD = 500;  // Values below = white line detected

// Timing for serial monitor updates
unsigned long lastSendTime = 0;
const unsigned long SEND_INTERVAL = 50;  // Send data every 50ms

// Line tracking variables
byte lastLineSide = OUTPUT1;           // Last line position for forward mode
byte lastLineSideReverse = OUTPUT1;    // Last line position for reverse mode

// Command constants
const byte START_CMD = 4;
const byte STOP_CMD = 5;
const byte REVERSE_CMD = 6;
const byte SET_FAST_PWM = 7;
const byte SET_SLOW_PWM = 8;

// Car state variables
int carState = 0;
const int STATE_STOPPED = 0;
const int STATE_RUNNING = 1;
const int STATE_REVERSING = 2;

// Smoothing variables to reduce jitter
const int SMOOTH_SAMPLES = 2;
int leftSensorBuffer[SMOOTH_SAMPLES];
int rightSensorBuffer[SMOOTH_SAMPLES];
int sampleIndex = 0;

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

// Update motor speed values when PWM settings change
void updateMotorSpeeds()
{
  motorFast = MOTOR_STOP + (fastPWM - 128);
  motorSlow = MOTOR_STOP + (slowPWM - 128);
  motorReverseFast = MOTOR_STOP - (fastPWM - 128);
  motorReverseSlow = MOTOR_STOP - (slowPWM - 128);
}

// Read sensors with smoothing to reduce jitter
void readSensorsSmooth(int &leftVal, int &rightVal)
{
  // Read raw values
  leftSensorBuffer[sampleIndex] = analogRead(SENSOR1);
  rightSensorBuffer[sampleIndex] = analogRead(SENSOR2);
  
  sampleIndex = (sampleIndex + 1) % SMOOTH_SAMPLES;
  
  // Calculate average
  long leftSum = 0;
  long rightSum = 0;
  for(int i = 0; i < SMOOTH_SAMPLES; i++)
  {
    leftSum += leftSensorBuffer[i];
    rightSum += rightSensorBuffer[i];
  }
  
  leftVal = leftSum / SMOOTH_SAMPLES;
  rightVal = rightSum / SMOOTH_SAMPLES;
}

// Setup: Initialize serial communication and pins
void setup() 
{
  Serial.begin(9600);
  initDACs();
  
  // Initialize sensor buffers
  for(int i = 0; i < SMOOTH_SAMPLES; i++)
  {
    leftSensorBuffer[i] = analogRead(SENSOR1);
    rightSensorBuffer[i] = analogRead(SENSOR2);
  }
  
  // Initialize both motors to stopped position
  stopMotors();
  
  // Set initial state to stopped
  carState = STATE_STOPPED;
  
  delay(2000);  // 2 second delay before starting
  
  Serial.println("=== Enhanced Line Follower Started ===");
  Serial.println("State: STOPPED");
  Serial.println("Waiting for commands from GUI...");
}

// Improved Bang-Bang Line Following Algorithm (Forward)
void bangBangLineFollow()
{
  // Read smoothed sensor values
  int leftSensor, rightSensor;
  readSensorsSmooth(leftSensor, rightSensor);
  
  // Determine if each sensor detects white line (true) or black surface (false)
  bool leftOnWhite = (leftSensor < WHITE_THRESHOLD);
  bool rightOnWhite = (rightSensor < WHITE_THRESHOLD);
  
  // Bang-Bang Control Logic with improved transitions:
  
  // Case 1: Both sensors on black - Sharp turn based on last known line position
  if (!leftOnWhite && !rightOnWhite)
  {
    if (lastLineSide == OUTPUT1)  // Line was on left, turn left sharply
    {
      // output1 = MOTOR_STOP - 30;  // Left motor slight reverse
      // output2 = motorFast;         // Right motor forward fast
      output1 = MOTOR_STOP - 60;  // Left motor slight reverse
      output2 = motorFast;         // Right motor forward fast
    }
    else  // Line was on right, turn right sharply
    {
      // output1 = motorFast;         // Left motor forward fast
      // output2 = MOTOR_STOP - 30;  // Right motor slight reverse
      output1 = motorFast;         // Left motor forward fast
      output2 = MOTOR_STOP - 60;  // Right motor slight reverse
    }
  }
  // Case 2: Left sensor on white, right on black - Gentle left turn
  else if (leftOnWhite && !rightOnWhite)
  {
    lastLineSide = OUTPUT1;
    output1 = motorSlow;   // Left motor slower
    output2 = motorFast;   // Right motor faster
  }
  // Case 3: Right sensor on white, left on black - Gentle right turn
  else if (!leftOnWhite && rightOnWhite)
  {
    lastLineSide = OUTPUT2;
    output1 = motorFast;   // Left motor faster
    output2 = motorSlow;   // Right motor slower
  }
  // Case 4: Both sensors on white - Go straight at full speed
  else
  {
    output1 = motorFast;
    output2 = motorFast;
  }
  
  // Apply motor speeds to DACs
  outputToDAC1(output1);
  outputToDAC2(output2);
}

// Improved Bang-Bang Line Following Algorithm (Reverse)
void bangBangLineFollowReverse()
{
  // Read smoothed sensor values
  int leftSensor, rightSensor;
  readSensorsSmooth(leftSensor, rightSensor);
  
  // Determine if each sensor detects white line (true) or black surface (false)
  bool leftOnWhite = (leftSensor < WHITE_THRESHOLD);
  bool rightOnWhite = (rightSensor < WHITE_THRESHOLD);
  
  // Bang-Bang Control Logic for REVERSE (motor directions inverted):
  
  // Case 1: Both sensors on black - Sharp turn based on last known line position
  if (!leftOnWhite && !rightOnWhite)
  {
    if (lastLineSideReverse == OUTPUT1)  // Line was on left, turn left sharply in reverse
    {
      output1 = MOTOR_STOP + 30;  // Left motor slight forward (to turn left in reverse)
      output2 = motorReverseFast; // Right motor reverse fast
    }
    else  // Line was on right, turn right sharply in reverse
    {
      output1 = motorReverseFast; // Left motor reverse fast
      output2 = MOTOR_STOP + 30;  // Right motor slight forward (to turn right in reverse)
    }
  }
  // Case 2: Left sensor on white, right on black - Gentle left turn in reverse
  else if (leftOnWhite && !rightOnWhite)
  {
    lastLineSideReverse = OUTPUT1;
    output1 = motorReverseSlow;  // Left motor slower reverse
    output2 = motorReverseFast;  // Right motor faster reverse
  }
  // Case 3: Right sensor on white, left on black - Gentle right turn in reverse
  else if (!leftOnWhite && rightOnWhite)
  {
    lastLineSideReverse = OUTPUT2;
    output1 = motorReverseFast;  // Left motor faster reverse
    output2 = motorReverseSlow;  // Right motor slower reverse
  }
  // Case 4: Both sensors on white - Go straight in reverse
  else
  {
    output1 = motorReverseFast;
    output2 = motorReverseFast;
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

// Handle serial commands from GUI with proper 4-byte protocol
void handleSerialCommands()
{
  // Only try to read commands if we have at least 4 bytes available
  if (Serial.available() >= 4)
  {
    byte receivedStartByte = Serial.peek();  // Peek at first byte without consuming it
    
    // Check if this looks like a command (starts with START byte 255)
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
          case START_CMD:  // Command 4 - Start forward
            carState = STATE_RUNNING;
            lastLineSide = OUTPUT1;  // Reset forward tracker
            Serial.println("Command: START - Car now following line forward");
            break;
            
          case STOP_CMD:   // Command 5 - Stop
            carState = STATE_STOPPED;
            Serial.println("Command: STOP - Car stopped");
            break;
            
          case REVERSE_CMD:  // Command 6 - Start reverse
            carState = STATE_REVERSING;
            lastLineSideReverse = OUTPUT1;  // Reset reverse tracker
            Serial.println("Command: REVERSE - Car following line in reverse");
            break;
            
          case SET_FAST_PWM:  // Command 7 - Set fast PWM
            fastPWM = receivedDataByte;
            updateMotorSpeeds();
            Serial.print("Fast PWM updated: ");
            Serial.println(fastPWM);
            break;
            
          case SET_SLOW_PWM:  // Command 8 - Set slow PWM
            slowPWM = receivedDataByte;
            updateMotorSpeeds();
            Serial.print("Slow PWM updated: ");
            Serial.println(slowPWM);
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
    else
    {
      // Not a command start byte, discard it
      Serial.read();
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
      bangBangLineFollowReverse();
      break;
  }

  // delay(0.5);  // Small delay for stability
}