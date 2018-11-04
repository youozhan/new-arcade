int sensor[2];
int current[2];
int previous[2];

#define outputA1 4
#define outputB1 5
#define outputA2 6
#define outputB2 7

int aState[2];
int aLastState[2];
int waitTime1 = 50;
int waitTime2 = 50;

void setup() {
  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);

  pinMode (outputA1, INPUT);
  pinMode (outputB1, INPUT);
  pinMode (outputA2, INPUT);
  pinMode (outputB2, INPUT);

  // Reads the initial state of the outputA
  aLastState[0] = digitalRead(outputA1);
  aLastState[1] = digitalRead(outputA2);
}

void loop() {

  // read the input on digital pin 6 and 7
  aState[0] = digitalRead(outputA1); // Reads the "current" state of the outputA
  // If the previous and the current state of the outputA are different, that means a Pulse has occured
  if (aState[0] != aLastState[0]) {
    // If the outputB state is different to the outputA state, that means the encoder is rotating clockwise
    if (digitalRead(outputB1) != aState[0]) {
      sensor[0] --;
      current[0] = 0;
    } else {
      sensor[0] ++;
      if (waitTime1 > 0) {
        current[0] = 1;
      }
    }
  }

  aState[1] = digitalRead(outputA2); // Reads the "current" state of the outputA
  // If the previous and the current state of the outputA are different, that means a Pulse has occured
  if (aState[1] != aLastState[1]) {
    // If the outputB state is different to the outputA state, that means the encoder is rotating clockwise
    if (digitalRead(outputB2) != aState[1]) {
      sensor[1] --;
      current[1] = 0;
    } else {
      sensor[1] ++;
      if (waitTime2 > 0) {
        current[1] = 1;
      }
    }
  }

  // If the encoder is rotating clockwise but not rotating, start the countdown and set the current reading to 0 if the countdown is over
  if (sensor[0] != previous[0] && waitTime1 == 0 ) {
    waitTime1 = 50;
  } else if (sensor[0] == previous[0] && waitTime1 == 0) {
    current[0] = 0;
  } else if (sensor[0] == previous[0] && waitTime1 > 0) {
    waitTime1 --;
  }

  aLastState[0] = aState[0]; // Updates the previous state of the outputA with the current

  if (sensor[1] != previous[1] && waitTime2 == 0 ) {
    waitTime2 = 50;
  } else if (sensor[1] == previous[1] && waitTime2 == 0) {
    current[1] = 0;
  } else if (sensor[1] == previous[1] && waitTime2 > 0) {
    waitTime2 --;
  }

  aLastState[1] = aState[1]; // Updates the previous state of the outputA with the current

  // print out the two number values from 0-1024 separated by a comma
  Serial.print(current[0]);
  Serial.print(",");
  Serial.println(current[1]);

  previous[0] = sensor[0];
  previous[1] = sensor[1];

  //this technique can be extended for more sensor values, separating them all with commas or any other character then splitting it into an array in processing.
}
