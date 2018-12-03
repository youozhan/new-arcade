/* continue 22, restart 23, pedal 24, keypad 25-32, microswitch 33, 
 * rotary encoder 34 35, 36 37
 * signal led 53, pass led 52
*/
#include <Keypad.h>

// define buttons input
const int proceedPin = 22;
const int restartPin = 23;
const int pedalPin = 24;
const int switchPin = 33;

// define keypad
const byte ROWS = 4;
const byte COLS = 4;
char hexaKeys[ROWS][COLS] = {
  {'1', '2', '3', 'A'},
  {'4', '5', '6', 'B'},
  {'7', '8', '9', 'C'},
  {'*', '0', '#', 'D'}
};

byte rowPins[ROWS] = {25, 26, 27, 28};
byte colPins[COLS] = {29, 30, 31, 32};

Keypad customKeypad = Keypad(makeKeymap(hexaKeys), rowPins, colPins, ROWS, COLS);

#define Password_Length 8
//int digits = 4;
char Data[Password_Length];
char Master[Password_Length] = "123A456";
byte data_count = 0, master_count = 0;
bool Pass_is_good;
char customKey;

// defint output
int greenPin = 52;
int yellowPin = 53;

// define additional readings
//int reading[4];
int lock = 0;

void setup() {
  // put your setup code here, to run once:
  pinMode(proceedPin, INPUT_PULLUP);
  pinMode(restartPin, INPUT_PULLUP);
  pinMode(pedalPin, INPUT_PULLUP);
  pinMode(switchPin, INPUT_PULLUP);

  pinMode(yellowPin, OUTPUT);
  pinMode(greenPin, OUTPUT);
  Serial.begin(9600);
}

void loop() {
  //  readings();

  unlockFridge();

  Serial.print(digitalRead(proceedPin));
  Serial.print(",");
  Serial.print(digitalRead(restartPin));
  Serial.print(",");
  Serial.print(digitalRead(pedalPin));
  Serial.print(",");
  Serial.print(digitalRead(switchPin));
  Serial.print(",");
  Serial.println(lock);

  restart();

}

void unlockFridge() {
  customKey = customKeypad.getKey();
  if (customKey) {
    digitalWrite(yellowPin, HIGH);
    delay(100);
    digitalWrite(yellowPin, LOW);
    Data[data_count] = customKey;
    data_count++;
  }

  if (data_count == Password_Length - 1) {

    if (!strcmp(Data, Master)) {
      Serial.println("Correct");
      digitalWrite(greenPin, HIGH);
      lock = 1;
    }
    else {
      Serial.println("Incorrect");

    }

    while (data_count != 0) {
      Data[data_count--] = 0;
    }
    return;
  }
}


void restart() {
  if (digitalRead(restartPin) == 0) {
    digitalWrite(greenPin, LOW);
    lock = 0;
  }
}
//void readings() {
//  if (digitalRead(proceedPin) == 0) {
//    reading[0] = 1;
//  } else {
//    reading[0] = 0;
//  }
//
//  if (digitalRead(restartPin) == 0) {
//    reading[1] = 1;
//  } else {
//    reading[1] = 0;
//  }
//
//}
