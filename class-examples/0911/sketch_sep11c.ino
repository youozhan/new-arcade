void setup() {
  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);
}

void loop() {
  // read the input on analog pin 0 and 1
  int sensor0 = analogRead(A0);
  int sensor1 = analogRead(A1);

  // print out the two number values from 0-1024 separated by a comma
  Serial.print(sensor0);
  Serial.print(",");
  Serial.println(sensor1);
  
  //this technique can be extended for more sensor values, separating them all with commas or any other character then splitting it into an array in processing.
}
