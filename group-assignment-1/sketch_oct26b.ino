int sensor[2];
int reading[2];
int current[2];
int previous[2];


void setup() {
  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);
  previous[0] = 0;
  previous[1] = 0;
}

void loop() {
  // read the input on analog pin 0 and 1
  sensor[0] = analogRead(A0);
  sensor[1] = analogRead(A1);

  if (sensor[0] < 500) {
    current[0] = 1;
  } else {
    current[0] = 0;
  }

  if (sensor[1] < 500) {
    current[1] = 1;
  } else {
    current[1] = 0;
  }

  if (current[0] == 1 && previous[0] == 0) {
    reading[0] = 1;
  } else {
    reading[0] = 0;
  }

  if (current[1] == 1 && previous[1] == 0) {
    reading[1] = 1;
  } else {
    reading[1] = 0;
  }

  previous[0] = current[0];
  previous[1] = current[1];

  // print out the two number values from 0-1024 separated by a comma
  Serial.print(reading[0]);
  Serial.print(",");
  Serial.println(reading[1]);

  //this technique can be extended for more sensor values, separating them all with commas or any other character then splitting it into an array in processing.
}
