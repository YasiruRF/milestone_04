//Curtin University
//Mechatronics Engineering
//Line Follower with Bang-Bang Control

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
const byte MOTOR_FAST = 200;   // Fast forward speed
const byte MOTOR_SLOW = 150;   // Slow forward speed (for turning)

// Sensor thresholds (adjust these based on your sensor readings)
// White line detection: readings between 200-300 (closer to 230)
// Black surface: readings between 700-900 (closer to 789)
const int WHITE_THRESHOLD = 500;  // Values below this = white line detected

// Timing for serial monitor updates
unsigned long lastPrintTime = 0;
const unsigned long PRINT_INTERVAL = 200;  // Print every 200ms

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

// Setup: Initialize serial communication and pins
void setup() 
{
  Serial.begin(9600);
  initDACs();
  
  // Initialize both motors to stopped position
  outputToDAC1(MOTOR_STOP);
  outputToDAC2(MOTOR_STOP);
  
  delay(2000);  // 2 second delay before starting
  
  Serial.println("=== Line Follower Started ===");
  Serial.println("Left_Sensor, Right_Sensor, Left_Motor, Right_Motor");
}

// Bang-Bang Line Following Algorithm
void bangBangLineFollow()
{
  // Read analog values from both IR sensors
  int leftSensor = analogRead(SENSOR1);   // Left sensor
  int rightSensor = analogRead(SENSOR2);  // Right sensor
  
  // Determine if each sensor detects white line (true) or black surface (false)
  bool leftOnWhite = (leftSensor < WHITE_THRESHOLD);
  bool rightOnWhite = (rightSensor < WHITE_THRESHOLD);
  
  // Bang-Bang Control Logic:
  // Case 1: Both sensors on black - stop
  if (!leftOnWhite && !rightOnWhite)
  {
    output1 = MOTOR_STOP;  // Stop left motor
    output2 = MOTOR_STOP;  // Stop right motor

  }
  // Case 2: Left sensor on white, right on black - turn left
  else if (leftOnWhite && !rightOnWhite)
  {
    output1 = MOTOR_SLOW;  // Left motor slow (or reverse for sharper turn)
    output2 = MOTOR_FAST;  // Right motor fast
  }
  // Case 3: Right sensor on white, left on black - turn right
  else if (!leftOnWhite && rightOnWhite)
  {
    output1 = MOTOR_FAST;  // Left motor fast
    output2 = MOTOR_SLOW;  // Right motor slow (or reverse for sharper turn)
  }
  // Case 4: Both sensors on white - stop or continue last action
  else // Both on white
  {

    output1 = MOTOR_FAST;  // Left motor forward
    output2 = MOTOR_FAST;  // Right motor forward
  }
  
  // Apply motor speeds to DACs
  outputToDAC1(output1);
  outputToDAC2(output2);
  
  // Print sensor readings and motor speeds to serial monitor
  if (millis() - lastPrintTime >= PRINT_INTERVAL)
  {
    Serial.print(leftSensor);
    Serial.print(", ");
    Serial.print(rightSensor);
    Serial.print(", ");
    Serial.print(output1);
    Serial.print(", ");
    Serial.println(output2);
    
    lastPrintTime = millis();
  }
}

// Main loop: Handle serial communication and line following
void loop() 
{
  // Execute line following algorithm
  bangBangLineFollow();
  
  // Handle serial communication for remote control (if needed)
  if (Serial.available() >= 4)
  {
    startByte = Serial.read();

    if(startByte == START)
    {
      commandByte = Serial.read();
      dataByte = Serial.read();
      checkByte = Serial.read();

      checkSum = startByte + commandByte + dataByte;

      if(checkByte == checkSum)
      {
        switch(commandByte)
        {
          case INPUT1: // Read left sensor
          {
            input1 = analogRead(SENSOR1) >> 2;  // Scale 10-bit to 8-bit
                        
            Serial.write(START);
            Serial.write(commandByte);
            Serial.write(input1);
            int checkSum1 = START + commandByte + input1;
            Serial.write(checkSum1);
          }          
          break;
          
          case INPUT2: // Read right sensor
          {
            input2 = analogRead(SENSOR2) >> 2;  // Scale 10-bit to 8-bit
            
            Serial.write(START);
            Serial.write(commandByte);
            Serial.write(input2);
            int checkSum2 = START + commandByte + input2;
            Serial.write(checkSum2);
          }               
          break;
          
          case OUTPUT1: // Manual control of left motor
          {
            output1 = dataByte;
            outputToDAC1(output1);
          } 
          break;
          
          case OUTPUT2: // Manual control of right motor
          {
            output2 = dataByte;
            outputToDAC2(output2);
          }         
          break;
        }
      }
    }    
  }
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
