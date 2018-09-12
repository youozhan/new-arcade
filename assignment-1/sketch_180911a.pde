import processing.serial.*;
import java.awt.AWTException;
import java.awt.Robot;
import java.awt.event.KeyEvent;

Serial port;

Robot robot;

int buttonKPressed;
int buttonHPressed;
int buttonJPressed;
int buttonLPressed;
int pace;

void setup() {
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
  
  pace = int(random(10,500));
}

void serialEvent(Serial port) {
  String inString = port.readStringUntil('\n');
  if (inString != null) {
    inString = trim(inString);
    //println(inString);
    int[] allButtons = int(split(inString, ','));
    if (allButtons.length == 4) { 
      buttonKPressed = allButtons[0];
      buttonHPressed = allButtons[1];
      buttonJPressed = allButtons[2];
      buttonLPressed = allButtons[3];
    }
  }
}

void draw() {
  println("SpeedUp is: " + buttonKPressed);
  println("SpeedDown is: " + buttonHPressed);
  println("button KL is: " + buttonJPressed);
  println("button HJ is: " + buttonLPressed);
  println("Delay is " + pace);
  move();
}

void move(){
  if (buttonKPressed == 0) {
    pace -= 10;
    if(pace == 0){
      pace = 200;
    }
  } else {
  }
  
  if (buttonHPressed == 0) {
    pace += 10;
    if(pace == 1000){
      pace = 200;
    }
  } else {
  }
  
 if (buttonJPressed == 0) {
    robot.keyPress(KeyEvent.VK_K);
    robot.delay(pace);
    robot.keyPress(KeyEvent.VK_L);
    robot.delay(pace);
  } else {
    robot.keyRelease(KeyEvent.VK_K);
    robot.keyRelease(KeyEvent.VK_L);
  }

 if (buttonLPressed == 0) {
    robot.keyPress(KeyEvent.VK_H);
    robot.delay(pace);
    robot.keyPress(KeyEvent.VK_J);
    robot.delay(pace);
  } else {
    robot.keyRelease(KeyEvent.VK_H);
    robot.keyRelease(KeyEvent.VK_J);
  }
}
