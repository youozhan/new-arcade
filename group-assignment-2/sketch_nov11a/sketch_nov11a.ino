int potPin[] = {A0, A1};
int in1[] = {6, 8};
int in2[] = {7, 9};
int en[] = {2, 3};
int pinCount = 2;
int motorCount = 2;
int posx;
int posy;

void setup() {
  Serial.begin(9600);
  for (int thisPin = 0; thisPin < pinCount; thisPin++) {
    pinMode(potPin[thisPin], INPUT);
    pinMode(in1[thisPin], OUTPUT);
    pinMode(in2[thisPin], OUTPUT);
    pinMode(en[thisPin], OUTPUT);
  }

  SlideToGrid(0, 0);
}

void loop() {
//  SlideToValue(0, 900);
//  SlideToValue(1, 420);
//  Serial.println(analogRead(potPin[0]));
//  Serial.println(analogRead(potPin[1]));
  // orginal point
  //  SlideToValue(1024, 50);

  coordinate();
  
  Serial.print(posx);
  Serial.print(" ");
  Serial.println(posy);
}

void SlideToValue(int thisMotor, int targetValue) {
  //  int targetValue[] = {targetValue1, targetValue2};
  // check each slider
  //  for (int thisMotor = 0; thisMotor < motorCount; thisMotor++) {
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

void SlideToGrid(int x, int y) {
  SlideToValue(0, 900 - x * 70);
  SlideToValue(1, 410 + y * 70);
}

void coordinate() {
  int motorValue1 = analogRead(potPin[0]);
  int motorValue2 = analogRead(potPin[1]);

  if (motorValue1 > 900) {
    SlideToValue(0, 900);
  }

  if (motorValue1 < 410) {
    SlideToValue(0, 410);
  }

  if (motorValue2 > 900) {
    SlideToValue(1, 900);
  }

  if (motorValue2 < 410) {
    SlideToValue(1, 410);
  }

  posx = (900 - motorValue1) / 70;
  posy = (motorValue2 - 410) / 60;
}
