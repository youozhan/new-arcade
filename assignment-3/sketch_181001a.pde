import processing.serial.*;
import java.awt.AWTException;
import java.awt.Robot;
import java.awt.event.KeyEvent;

PFont f;

Serial port;

Robot robot;

int button1Pressed;
int button2Pressed;

void setup() {
  fullScreen();
  f = createFont("Roboto-Black-48.vlw", 48);
  println(Serial.list());
  port = new Serial(this, Serial.list()[3], 9600);
  port.bufferUntil('\n');

  try {
    robot = new Robot();
  }
  catch (AWTException e) {
    e.printStackTrace();
    exit();
  }
}

void serialEvent(Serial port) {
  String inString = port.readStringUntil('\n');
  if (inString != null) {
    inString = trim(inString);
    //println(inString);
    int[] allButtons = int(split(inString, ','));
    if (allButtons.length == 2) { 
      button1Pressed = allButtons[0];
      button2Pressed = allButtons[1];
    }
  }
}

void draw() {
  background(0);
  stroke(175);
  textFont(f);
  //fill(255);
  //stroke(175);
  //line(width/2, 0, width/2, height);
  
  //println("button 1 is: " + button1Pressed);
  //println("button 2 is: " + button2Pressed);

  if (button1Pressed == 0) {
    robot.keyPress(KeyEvent.VK_X);
    fill(255);
    rect(0,0,width/2,height);
    fill(0);
    textAlign(RIGHT);
    text("LEFT     ", width/2, 400);
  } else {
    robot.keyRelease(KeyEvent.VK_X);
  }

  if (button2Pressed == 0) {
    robot.keyPress(KeyEvent.VK_C);
    fill(255);
    rect(width/2,0,width/2,height);
    fill(0);
    textAlign(LEFT);
    text("     RIGHT", width/2, 600);
  } else {
    robot.keyRelease(KeyEvent.VK_C);
  }
}
