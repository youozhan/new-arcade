#define ENABLE    5
#define DIRA      3
#define DIRB      4
#define potPin    A0
#define enA       2
#define in1       6
#define in2       7
//#define pressurePin A4

int buttonPin = 9;
int slideValue = 200;

void setup() {
  Serial.begin(9600);
  pinMode(ENABLE, OUTPUT);
  pinMode(DIRA, OUTPUT);
  pinMode(DIRB, OUTPUT);
  pinMode(potPin, INPUT);
  pinMode(enA, OUTPUT);
  pinMode(in1, OUTPUT);
  pinMode(in2, OUTPUT);
  //  pinMode(pressurePin, INPUT);
  pinMode(buttonPin, INPUT_PULLUP);
}

void loop() {
  Serial.println(digitalRead(buttonPin));
  Serial.println(analogRead(potPin));
//  Serial.println(slideValue);
  
  SlideToValue(slideValue);
  
  if (digitalRead(buttonPin) == 0) {
    slideValue = 100;
    digitalWrite(ENABLE, HIGH); //enable on
    digitalWrite(DIRA, HIGH); //one way
    digitalWrite(DIRB, LOW);
  } else {
    slideValue = 500;
    digitalWrite(ENABLE, LOW); //slow stop
  }

//  delay(1000);
}

void SlideToValue(int targetValue) {
  int val = analogRead(potPin);
  if (abs(val - targetValue) > 40) {

    if (val > targetValue) {
      digitalWrite(in1, LOW);
      digitalWrite(in2, HIGH);
    } else {
      digitalWrite(in1, HIGH);
      digitalWrite(in2, LOW);
    }

    analogWrite(enA, max(min(abs(val - targetValue), 255), 200));

  } else {
    // Turn off motor
    digitalWrite(in1, LOW);
    digitalWrite(in2, LOW);
    analogWrite(enA, 0);
  }
}
