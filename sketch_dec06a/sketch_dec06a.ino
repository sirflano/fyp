// constants won't change. They're used here to
// set pin numbers:
#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>

#define BNO055_SAMPLERATE_DELAY_MS (100)

const int buttonPin = 2;     // the number of the pushbutton pin
const int ledPin =  13;      // the number of the LED pin
Adafruit_BNO055 bno = Adafruit_BNO055(55);

// variables will change:
int buttonState = 0;         // variable for reading the pushbutton status

void setup() {
  // initialize the LED pin as an output:
  // initialize the pushbutton pin as an input:
  Serial.begin(9600);
  pinMode(buttonPin, INPUT);
  if(!bno.begin())
  {
    /* There was a problem detecting the BNO055 ... check your connections */
    Serial.print("Ooops, no BNO055 detected ... Check your wiring or I2C ADDR!");
    while(1);
  }
  
  delay(1000);
    
  bno.setExtCrystalUse(true);
}

void loop() {
  // read the state of the pushbutton value:
  buttonState = digitalRead(buttonPin);

  // check if the pushbutton is pressed.
    sensors_event_t event; 
  bno.getEvent(&event);
  
  /* Display the floating point data */
  
  // if it is, the buttonState is HIGH:
  if(displayCalStatus()){
    if (buttonState == HIGH) {
      // turn LED on:
      Serial.print("H");
    }
    else if(buttonState == LOW) {
      Serial.print("L");
    }
    Serial.print(":");
    Serial.print(event.orientation.x, 4);
    Serial.print(":");
    Serial.print(event.orientation.y, 4);
    Serial.print(":");
    Serial.print(event.orientation.z, 4);
  
    Serial.println("");
    //delay(BNO055_SAMPLERATE_DELAY_MS);
  }
}

bool displayCalStatus(void)
{
  /* Get the four calibration values (0..3) */
  /* Any sensor data reporting 0 should be ignored, */
  /* 3 means 'fully calibrated" */
  uint8_t system, gyro, accel, mag;
  system = gyro = accel = mag = 0;
 
  bno.getCalibration(&system, &gyro, &accel, &mag);
 
  /* The data should be ignored until the system calibration is > 0 */
  Serial.print("\t");
  if (!system)
  {
    Serial.print("! ");
  }

  if(gyro > 2 && accel > 2 && mag > 2 && system > 2){
    return true;
  }
  else {
    /* Display the individual values */
    Serial.print("Sys:");
    Serial.print(system, DEC);
    Serial.print(" G:");
    Serial.print(gyro, DEC);
    Serial.print(" A:");
    Serial.print(accel, DEC);
    Serial.print(" M:");
    Serial.println(mag, DEC);
    return false;
  }
}