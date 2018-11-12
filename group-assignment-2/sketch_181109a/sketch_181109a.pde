import processing.serial.*;
import cc.arduino.*;
PFont font;

Block[] blocks = new Block[128];
Person person1;
Person person2;

Serial port;
int sensor0;
int sensor1;

Arduino arduino;
int ledPin = 13;

int x1 = int(random(0, 7));
int y1 = int(random(0, 7));
int x2 = int(random(0, 7));
int y2 = int(random(0, 7));
int xSwap1 = int(random(0, 7));
int ySwap1 = int(random(0, 7));
int xSwap2 = int(random(0, 7));
int ySwap2 = int(random(0, 7));

float prev1;
float prev2;

boolean status = true;

void setup() {
  //println(Serial.list());
  port = new Serial(this, Serial.list()[3], 9600);

  arduino = new Arduino(this, Arduino.list()[3], 9600);
  arduino.pinMode(ledPin, Arduino.OUTPUT);

  port.bufferUntil('\n');

  size(720, 480);
  background(255);
  font = createFont("Futura-Medium", 28);
  textFont(font);
  textAlign(LEFT);
  textSize(14);

  x1 = int(random(0, 7));
  y1 = int(random(0, 7));
  x2 = int(random(0, 7));
  y2 = int(random(0, 7));
  xSwap1 = int(random(0, 7));
  ySwap1 = int(random(0, 7));
  xSwap2 = int(random(0, 7));
  ySwap2 = int(random(0, 7));

  int index = 0;

  for (int i = 0; i < 8; i++) {
    for ( int k = 0; k < 8; k++) { 
      blocks[index] = new Block(120 + 20 * i, 160 + 20 * k);
      blocks[64 + index] = new Block(440 + 20 * i, 160 + 20 * k);
      index ++;
    }
  }

  person1 = new Person(120, 160);
  person2 = new Person(580, 160);
}

void serialEvent(Serial port) {
  String inString = port.readStringUntil('\n');
  if (inString != null) {
    inString = trim(inString);
    println(inString);
    int[] allData = int(split(inString, ','));
    //println(allData);
    if (allData.length >= 2) { 
      sensor0 = allData[0];
      //println(sensor0);
      sensor1 = allData[1];

      //buttonKPressed = allButtons[0];
      //buttonHPressed = allButtons[1];
      //buttonJPressed = allButtons[2];
      //buttonLPressed = allButtons[3];
    }
  }

  //person1.posx = 120 + 20 * sensor0;
  //person1.posy = 160 + 20 * sensor1;
}

void draw() {
  background(255);
  stroke(0);
  line(width/2, 0, width/2, height);
  person1.update();
  person2.update();
  treasure();
  //swap();

  for (int  i = 0; i < 128; i ++) {
    if (blocks[i].posx == person1.posx && blocks[i].posy == person1.posy) {
      blocks[i].collide = true;
    } else if (blocks[i].posx == person2.posx && blocks[i].posy == person2.posy) {
      blocks[i].collide = true;
    } else {
      blocks[i].collide = false;
    }

    blocks[i].update();
    blocks[i].display();
  }

  //println (person1.posx + " " + person1.posy + " " + person2.posx + " " + person2. posy);

  if (person1.special) {
    text("Player 1 gets the swap power", 40, 80);
  }

  if (person2.special) {
    text("Player 2 gets the swap power", 400, 80);
  }
}

void keyReleased() {
  if (status) {
    //person1.update();
    //person2.update();

    if (key == 'd') {
      person1.posx += 20;
      person2.posx -= 20;
    }

    if (key == 'a') {
      person1.posx -= 20;
      person2.posx += 20;
    }
    if (key == 'w') {
      person1.posy -= 20;
      person2.posy -= 20;
    }
    if (key == 's') {
      person1.posy += 20;
      person2.posy += 20;
    }

    if (keyCode == RIGHT) {
      person1.posx -= 20;
      person2.posx += 20;
    }

    if (keyCode == LEFT ) {
      person1.posx += 20;
      person2.posx -= 20;
    }
    if (keyCode == UP) {
      person1.posy -= 20;
      person2.posy -= 20;
    }
    if (keyCode == DOWN) {
      person1.posy += 20;
      person2.posy += 20;
    }
  }

  if (key == 'r') {
    person1.posx = 120;
    person1.posy = 160;
    person2.posx = 580;
    person2.posy = 160;
    person1.goal = false;
    person2.goal = false;
    status = true;

    x1 = int(random(0, 7));
    y1 = int(random(0, 7));
    x2 = int(random(0, 7));
    y2 = int(random(0, 7));
    xSwap1 = int(random(0, 7));
    ySwap1 = int(random(0, 7));
    xSwap2 = int(random(0, 7));
    ySwap2 = int(random(0, 7));
  }

  if (key == 'x' && person1.special) {
    int tempx = person1.posx;
    //int tempy = person1.posy;
    person1.posx = 120 + person2.posx - 440;
    //person1.posy = person2.posy;
    person2.posx = tempx - 120 + 440;
    //person2.posy = x tempy;

    person1.special = false;

    println("position swapped");
  }

  if (key == ' ' && person2.special) {
    int tempx = person1.posx;
    //int tempy = person1.posy;
    person1.posx = 120 + person2.posx - 440;
    //person1.posy = person2.posy;
    person2.posx = tempx - 120 + 440;
    //person2.posy = tempy;

    person2.special = false;

    println("position swapped");
  }

  swap();
}

void treasure() {
  arduino.digitalWrite(ledPin, Arduino.HIGH);
  delay(1000);
  arduino.digitalWrite(ledPin, Arduino.LOW);
  delay(1000);

  //println(x1 + " " + y1 + " " + x2 + " " + y2 + " " + xSwap1 + " " + ySwap1 + " " + xSwap2 + " " + ySwap2);

  person1.distance = abs(dist(person1.posx, person1.posy, 120+20*x1, 160+20*y1));
  person2.distance = abs(dist(person2.posx, person2.posy, 440+20*x2, 160+20*y2));

  if (person1.distance < prev1) {
    person1.close = true;
    text("Player 1 is closer", 40, 40);
  } else {
    person1.close = false;
  }

  if (person2.distance < prev2) {
    person2.close = true;
    text("Player 2 is closer", 400, 40);
  } else {
    person2.close = false;
  }

  if (person1.distance == 0) {
    person1.goal = true;
    println("person1 wins");
    text("Player 1 wins", 40, 40);
    text("Player 1 wins", 400, 40);
    status = false;
  }

  if (person2.distance == 0) {
    person2.goal = true;
    println("person2 wins");
    text("Player 2 wins", 40, 40);
    text("Player 2 wins", 400, 40);
    status = false;
  }

  prev1 = person1.distance;
  prev2 = person2.distance;
}

void swap() {
  if (person1.posx == 120 + 20 * xSwap1 && person1.posy == 160 + 20 * ySwap1) {
    person1.special = true;
    //text("Player 1 gets the swap power", 40, 40);
  }

  if (person2.posx == 440 + 20 * xSwap2 && person2.posy == 160 + 20 * ySwap2) {
    person2.special = true;
    //text("Player 2 gets the swap power", 400, 40);
  }
}
