int potPin[] = {A0, A1, A2, A3};
int in1[] = {6, 8, 10, 12};
int in2[] = {7, 9, 11, 13};
int en[] = {2, 3, 4, 5};
int pinCount = 4;
int motorCount = 4;
int posx1;
int posy1;
int posx2;
int posy2;
int player;
int motorValue[4];

int buttonPin1 = A5;
int buttonPin2 = A4;

void setup() {
  Serial.begin(9600);
  for (int thisPin = 0; thisPin < pinCount; thisPin++) {
    pinMode(potPin[thisPin], INPUT);
    pinMode(in1[thisPin], OUTPUT);
    pinMode(in2[thisPin], OUTPUT);
    pinMode(en[thisPin], OUTPUT);
  }

  pinMode(buttonPin1, INPUT_PULLUP);
  pinMode(buttonPin2, INPUT_PULLUP);

  SlideToGrid(1, 0, 0);
  SlideToGrid(2, 0, 0);
  delay(1000);
}

void loop() {
//    SlideToValue(0, 400);
//    SlideToValue(1, 400);
//    SlideToValue(2, 400);
//    SlideToValue(3, 400);
//    Serial.println(analogRead(potPin[0]));
//    Serial.println(analogRead(potPin[1]));
//    Serial.println(analogRead(potPin[2]));
//    Serial.println(analogRead(potPin[3]));
//
  Coordinate();
  CheckPos();
  PrintPos();

  Serial.print(posx1);
  Serial.print(",");
  Serial.print(posy1);
  Serial.print(",");
  Serial.print(posx2);
  Serial.print(",");
  Serial.println(posy2);
}

void SlideToValue(int thisMotor, int targetValue) {
  int val = analogRead(potPin[thisMotor]);
  if (abs(val - targetValue) > 20) {
    digitalWrite(en[thisMotor], HIGH);
    if (val > targetValue) {
      digitalWrite(in1[thisMotor], LOW);
      digitalWrite(in2[thisMotor], HIGH);
    } else {
      digitalWrite(in1[thisMotor], HIGH);
      digitalWrite(in2[thisMotor], LOW);
    }
  } else {
    // Turn off motor
    digitalWrite(in1[thisMotor], LOW);
    digitalWrite(in2[thisMotor], LOW);
    digitalWrite(en[thisMotor], LOW);
  }
}

void SlideToGrid(int p, int x, int y) {
  if (p == 1) {
    SlideToValue(0, 995 - x * 135);
    SlideToValue(1, 50 + y * 135);
  }

  if (p == 2) {
    SlideToValue(2, 995 - x * 135);
    SlideToValue(3, 50 + y * 135);
  }
}

void Coordinate() {
  for (int thisMotor = 0; thisMotor < motorCount; thisMotor++) {
    motorValue[thisMotor] = analogRead(potPin[thisMotor]);
  }

//  for (int thisMotor = 0; thisMotor < motorCount; thisMotor++) {
//    if (motorValue[thisMotor] > 995) {
//      SlideToValue(thisMotor, 995);
//    }
//
//    if (motorValue[thisMotor] < 50) {
//      SlideToValue(thisMotor, 50);
//    }
//  }
}

void CheckPos() {
  if (analogRead(buttonPin1) < 50) {
    //    SlideToGrid(1, 0, 0);
    //    SlideToGrid(2, 0, 0);
    //    delay(1000);
//    if (motorValue[0] != motorValue[2]  || motorValue[1] != motorValue[3]) {
      SlideToValue(2, motorValue[0]);
      SlideToValue(3, motorValue[1]);
    }
//  } else {
//    
//  }
  
  if (analogRead(buttonPin2) < 100) {
//    if (motorValue[0] != motorValue[2]) {
      SlideToValue(0, motorValue[2]);
      SlideToValue(1, motorValue[3]);
//    }
//
//    if (motorValue[1] != motorValue[3] || motorValue[1] != motorValue[3]) {
//      SlideToValue(0, motorValue[2]);
//      SlideToValue(1, motorValue[3]);
    }
//  } else {
//  }
}

void PrintPos() {
  posx1 = (995 - motorValue[0]) / 135;
  posy1 = 7 - (motorValue[1] - 50) / 135;
  posx2 = (995 - motorValue[2]) / 135;
  posy2 = 7 - (motorValue[3] - 50) / 135;
}
