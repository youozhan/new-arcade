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
  println("button K is: " + buttonKPressed);
  println("button H is: " + buttonHPressed);
  println("button J is: " + buttonJPressed);
  println("button L is: " + buttonLPressed);

  if (buttonKPressed == 0) {
    robot.keyPress(KeyEvent.VK_K);
    //robot.delay(50);
  } else {
    robot.keyRelease(KeyEvent.VK_K);
  }

  if (buttonHPressed == 0) {
    robot.keyPress(KeyEvent.VK_H);
    //robot.delay(50);
  } else {
    robot.keyRelease(KeyEvent.VK_H);
  }
  
  if (buttonJPressed == 0) {
    robot.keyPress(KeyEvent.VK_J);
    //robot.delay(50);
  } else {
    robot.keyRelease(KeyEvent.VK_J);
  }
  
  if (buttonLPressed == 0) {
    robot.keyPress(KeyEvent.VK_L);
    //robot.delay(50);
  } else {
    robot.keyRelease(KeyEvent.VK_L);
  }
}
