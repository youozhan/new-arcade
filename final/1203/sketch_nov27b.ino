/* continue 22, restart 23, pedal 24, keypad 25-32, microswitch 33,
   rotary encoder 34 35, 36 37
   pressure sensor 39 40 41 A0 A1 A2
   slide potentiometer A4
   progress led 53, state led 52, error led 51
*/

#include <Keypad.h>

// define button and switch input
const int proceedPin = 22;
const int restartPin = 23;
const int pedalPin = 24;
const int knifePin = 33;

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
char Data[Password_Length];
char Master[Password_Length] = "123A456";
byte data_count = 0, master_count = 0;
//bool Pass_is_good;
char customKey;

// define pressure sensor
#define numRows 3
#define numCols 3
int rows[] = {A0, A1, A2};
int cols[] = {39, 40, 41};
int crustValues[9] = {};
bool crust[9];

// define rotary encoder reading
#define creamA 34
#define creamB 35
#define crankA 36
#define crankB 37

int creamCounter[2] = {0, 0};
int crankCounter = 0;
int creamState;
int creamLastState;
int crankState;
int crankLastState;

// define potentiometer
int appleLastPos;
int applePos;
int knifeLastState = 1;
int knifeState;
int sliceCounter = 0;

int crustCounter = 0;

// define output
int redPin = 51;
int greenPin = 52;
int yellowPin = 53;

// define additional readings
int unlock = 0;
int appleReady = 0;
int crustReady = 0;
int creamReady = 0;
int pieReady = 0;

//**********************************************
void setup() {
  // put your setup code here, to run once:
  pinMode(proceedPin, INPUT_PULLUP);
  pinMode(restartPin, INPUT_PULLUP);
  pinMode(pedalPin, INPUT_PULLUP);
  pinMode(knifePin, INPUT_PULLUP);

  pinMode(creamA, INPUT);
  pinMode(creamB, INPUT);
  pinMode(crankA, INPUT);
  pinMode(crankB, INPUT);

  pinMode(yellowPin, OUTPUT);
  pinMode(greenPin, OUTPUT);
  pinMode(redPin, OUTPUT);

  for (int i = 0; i < numRows; i++) {
    pinMode(rows[i], INPUT);
  }

  for (int i = 0; i < numCols; i++) {
    pinMode(cols[i], INPUT);
  }

  Serial.begin(9600);

  creamLastState = digitalRead(creamA);
  crankLastState = digitalRead(crankA);

  appleLastPos = analogRead(A4);

  for (int i = 0; i < 9; i++) {
    crust[i] = false;
  }
}

void loop() {

  unlockFridge();
  cutApple();
  makeCrust();
  makeCream();
  finishPie();

  Serial.print(digitalRead(proceedPin));
  Serial.print(",");
  Serial.print(digitalRead(restartPin));
  Serial.print(",");

  Serial.print(digitalRead(pedalPin));
  Serial.print(",");

  //  Serial.print(digitalRead(knifePin));
  //  Serial.print(",");

  Serial.print(unlock);
  Serial.print(",");
  Serial.print(appleReady);
  Serial.print(",");
  Serial.print(crustReady);
  Serial.print(",");
  Serial.print(creamReady);
  Serial.print(",");
  Serial.println(pieReady);

  //  Serial.print(creamCounter[0]);
  //  Serial.print(",");
  //  Serial.println(creamCounter[1]);
  //  Serial.println(crankCounter);
  //  Serial.print(applePos);
  //  Serial.print(",");
  //  Serial.print(knifeState);
  //  Serial.print(",");
  //  Serial.println(sliceCounter);
  //  Serial.println(crustCounter);

  winning();
  restart();
}

//**********************************************
void unlockFridge() {
  customKey = customKeypad.getKey();
  if (customKey) {
    checkingState();
    Data[data_count] = customKey;
    data_count++;
  }

  if (data_count == Password_Length - 1) {

    if (!strcmp(Data, Master)) {
      Serial.println("Correct");
      checkingProgress();
      unlock = 1;
    }
    else {
      Serial.println("Incorrect");
      checkingError();
    }

    while (data_count != 0) {
      Data[data_count--] = 0;
    }
    return;
  }
}

void cutApple() {
  applePos = analogRead(A4);
  knifeState = digitalRead(knifePin);

  if ((knifeState == 0) && (knifeLastState == 1)) {
    if (applePos > appleLastPos) {
      sliceCounter ++;
      checkingState();
    }
  }

  appleLastPos = applePos;
  knifeLastState = knifeState;

  if (sliceCounter > 6) {
    checkingProgress();
    appleReady = 1;
    sliceCounter = 0;
  }
}

void makeCrust() {
  if (crustReady == 0) {
    for (int colCount = 0; colCount < numCols; colCount++) {
      pinMode(cols[colCount], OUTPUT);  // set as OUTPUT
      digitalWrite(cols[colCount], LOW);  // set LOW

      for (int rowCount = 0; rowCount < numRows; rowCount++) {
        pinMode(rows[rowCount], INPUT_PULLUP);  // set as INPUT with PULLUP RESISTOR
        delay(1);
        crustValues[ ( (colCount)*numRows) + (rowCount)] = analogRead(rows[rowCount]);  // read INPUT

        // set pin back to INPUT
        pinMode(rows[rowCount], INPUT);

      }// end rowCount

      pinMode(cols[colCount], INPUT);  // set back to INPUT!

    }// end colCount

    for (int i = 0; i < 9; i++) {
      if (crustValues[i] < 70) {
        crust[i] = true;
        checkingState();
        crustCounter ++;
      }
    }
  }

  if (crust[0] && crust[1] && crust[2] && crust[3] && crust[4] && crust[5] && crust[6] && crust[7] && crust[8]) {
    crustReady = 1;
    checkingProgress();
    for (int i = 0; i < 9; i++) {
      crust[i] = false;
    }
    crustCounter = 0;
  }
}

void makeCream() {
  if (creamReady == 0) {
    creamState = digitalRead(creamA); // Reads the "current" state of the outputA
    // If the previous and the current state of the outputA are different, that means a Pulse has occured
    if (creamState != creamLastState) {
      // If the outputB state is different to the outputA state, that means the encoder is rotating clockwise
      if (digitalRead(creamB) != creamState) {
        creamCounter[0] ++;
        checkingState();
      } else {
        creamCounter[1] ++;
        checkingState();
      }
    }
    creamLastState = creamState; // Updates the previous state of the outputA with the current state

    if ((creamCounter[0] >= 15) && (creamCounter[1] >= 15)) {
      checkingProgress();
      creamReady = 1;
      creamCounter[0] = 0;
      creamCounter[1] = 0;
    }
  }
}

void finishPie() {
  if (pieReady == 0) {
    crankState = digitalRead(crankA);
    if (crankState != crankLastState) {
      if (digitalRead(crankB) != crankState) {
        crankCounter ++;
        checkingState();
      } else {
        //        crankCounter --;
      }
    }
    crankLastState = crankState;

    if (crankCounter >= 20) {
      checkingProgress();
      pieReady = 1;
      crankCounter = 0;
    }
  }
}

//**********************************************
void restart() {
  if (digitalRead(restartPin) == 0) {
    digitalWrite(redPin, LOW);
    digitalWrite(greenPin, LOW);
    digitalWrite(yellowPin, LOW);

    unlock = 0;
    appleReady = 0;
    crustReady = 0;
    creamReady = 0;
    pieReady = 0;

  }
}

void winning() {
  if ((unlock == 1) && (appleReady == 1) && (crustReady == 1) && (creamReady == 1) && (pieReady == 1)) {
    if (!proceedPin) {
      digitalWrite(redPin, HIGH);
      digitalWrite(greenPin, HIGH);
      digitalWrite(yellowPin, HIGH);
    }
  }
}

void checkingState() {
  digitalWrite(yellowPin, HIGH);
  delay(100);
  digitalWrite(yellowPin, LOW);
}

void checkingProgress() {
  digitalWrite(greenPin, HIGH);
  delay(100);
  digitalWrite(greenPin, LOW);
}

void checkingError() {
  digitalWrite(redPin, HIGH);
  delay(100);
  digitalWrite(redPin, LOW);
}
