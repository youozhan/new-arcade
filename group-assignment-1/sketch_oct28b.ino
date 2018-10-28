int sensor[2];
int current[2];
int previous[2];
int reading[2];

#define outputA 6
#define outputB 7

int aState;
int aLastState;
int waitTime = 50;

void setup() {
  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);

  pinMode (outputA, INPUT);
  pinMode (outputB, INPUT);

  // Reads the initial state of the outputA
  aLastState = digitalRead(outputA);
}

void loop() {
  // read the input on analog pin 0 and 1
  //  sensor[0] = analogRead(A0);

  if (analogRead(A0) < 680) {
    sensor[0] = 1;
    current[0] = 1;
  } else {
    sensor[0] = 0;
    current[0] = 0;
  }

  if (previous[0] == 1 && sensor[0] == 0) {
    reading[0] = 1;
  } else {
    reading[0] = 0;
  }

  // read the input on digital pin 6 and 7
  aState = digitalRead(outputA); // Reads the "current" state of the outputA
  // If the previous and the current state of the outputA are different, that means a Pulse has occured
  if (aState != aLastState) {
    // If the outputB state is different to the outputA state, that means the encoder is rotating clockwise
    if (digitalRead(outputB) != aState) {
      sensor[1] --;
      current[1] = 0;
    } else {
      sensor[1] ++;
      if (waitTime > 0) {
        current[1] = 1;
      }
    }
  }

  if (sensor[1] != previous[1] && waitTime == 0 ) {
    waitTime = 50;
  } else if (sensor[1] == previous[1] && waitTime == 0) {
    current[1] = 0;
  } else if (sensor[1] == previous[1] && waitTime > 0){
    waitTime --;
  }

  if (sensor[1] != previous[1]) {
    reading[1] = 1;
  } else {
    reading[1] = 0;
  }

  aLastState = aState; // Updates the previous state of the outputA with the current

    Serial.print(current[0]);
    Serial.print(",");
    Serial.println(current[1]);
//    Serial.print(",");
//    Serial.println(waitTime); 

  previous[0] = sensor[0];
  previous[1] = sensor[1];

  // print out the two number values from 0-1024 separated by a comma


  //this technique can be extended for more sensor values, separating them all with commas or any other character then splitting it into an array in processing.
}
