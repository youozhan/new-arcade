const int buttonPin = 2;
const int otherButtonPin = 3;

void setup() {
  // put your setup code here, to run once:
  pinMode(buttonPin, INPUT_PULLUP);
  pinMode(otherButtonPin, INPUT_PULLUP);
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  Serial.print(digitalRead(buttonPin));
  Serial.print(",");
  Serial.println(digitalRead(otherButtonPin));
}
