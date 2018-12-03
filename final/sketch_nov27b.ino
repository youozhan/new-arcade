const int proceedPin = 2;
const int restartPin = 3;
int prevProceed = 1;
int prevRestart = 1;
int reading[2];
int current[2];

void setup() {
  // put your setup code here, to run once:
  pinMode(proceedPin, INPUT_PULLUP);
  pinMode(restartPin, INPUT_PULLUP);
  Serial.begin(9600);
}

void loop() {
  current[0] = digitalRead(proceedPin);
  current[1] = digitalRead(restartPin);
  
  if (current[0] == 1 && prevProceed == 0) {
    reading[0] = 1;
  } else {
    reading[0] = 0;
  }

  if (current[1] == 1 && prevRestart == 0) {
    reading[1] = 1;
  } else {
    reading[1] = 0;
  }

  prevProceed = current[0];
  prevRestart = current[1];

  Serial.print(reading[0]);
  Serial.print(",");
  Serial.println(reading[1]);
}
