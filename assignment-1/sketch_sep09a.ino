const int buttonPin1 = 5;
const int buttonPin2 = 4;
const int buttonPin3 = 3;
const int buttonPin4 = 2;

void setup() {
  // put your setup code here, to run once:
  pinMode(buttonPin1, INPUT_PULLUP);
  pinMode(buttonPin2, INPUT_PULLUP);
  pinMode(buttonPin3, INPUT_PULLUP);
  pinMode(buttonPin4, INPUT_PULLUP);
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  Serial.print(digitalRead(buttonPin1));
  Serial.print(",");
  Serial.print(digitalRead(buttonPin2));
  Serial.print(",");
  Serial.print(digitalRead(buttonPin3));
  Serial.print(",");
  Serial.println(digitalRead(buttonPin4));
}
