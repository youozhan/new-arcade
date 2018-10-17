// I2C device class (I2Cdev) demonstration Processing sketch for MPU6050 DMP output
// 6/20/2012 by Jeff Rowberg <jeff@rowberg.net>
// Updates should (hopefully) always be available at https://github.com/jrowberg/i2cdevlib
//
// Changelog:
//     2012-06-20 - initial release

/* ============================================
 I2Cdev device library code is placed under the MIT license
 Copyright (c) 2012 Jeff Rowberg
 
 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:
 
 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
 ===============================================
 */

import processing.serial.*;
import processing.opengl.*;
import toxi.geom.*;
import toxi.processing.*;
import ddf.minim.*;

// NOTE: requires ToxicLibs to be installed in order to run properly.
// 1. Download from http://toxiclibs.org/downloads
// 2. Extract into [userdir]/Processing/libraries
//    (location may be different on Mac/Linux)
// 3. Run and bask in awesomeness

ToxiclibsSupport gfx;

// For data transmission
Serial port;                         // The serial port
char[] teapotPacket = new char[14];  // InvenSense Teapot packet
int serialCount = 0;                 // current packet byte position
int synced = 0;
int interval = 0;

float[] q = new float[4];
Quaternion quat = new Quaternion(1, 0, 0, 0);

float[] gravity = new float[3];
float[] euler = new float[3];
float[] ypr = new float[3];

// For cat display
PImage img;
PFont font;
float pupilX1;
float pupilX2;
float pupilY1;
float pupilY2;
//float theta = 0;

// For game mechanics
int gameScreen = 0;
int maxHealth = 100;
float health = 100;
float healthDecrease = 0.2;
int healthBarWidth = 380;
boolean startTime;
int steadyTime = 10;

// For data smoothing
int numReadings = 5;
float[] readings = new float[numReadings];
int readIndex = 0;
float total = 0;
float average = 0;
float reference;

AudioPlayer player;
Minim minim;//audio context

void setup() {
  size(600, 800);
  img = loadImage("Asset_1.png");
  font = createFont("ComicSansMS", 28);
  textFont(font);
  startTime = false;
  gfx = new ToxiclibsSupport(this);

  minim = new Minim(this);
  player = minim.loadFile("musicbox.mp3", 2048);
  //player.play();

  for (int thisReading = 0; thisReading < numReadings; thisReading++) {
    readings[thisReading] = 0;
  }

  // display serial port list for debugging/clarity
  //println(Serial.list());

  // get the first available port (use EITHER this OR the specific port code below)
  String portName = Serial.list()[3];


  // open the serial port
  port = new Serial(this, portName, 115200);

  // send single character to trigger DMP init/start
  // (expected by MPU6050_DMP6 example Arduino sketch)
  port.write('r');
}

void draw() {
  if (gameScreen == 0) {
    initScreen();
  } else if (gameScreen == 1) {
    gameScreen();
  } else if (gameScreen == 2) {
    gameOverScreen();
  }

  if (millis() - interval > 1000) {
    // resend single character to trigger DMP init/start
    // in case the MPU is halted/reset while applet is running
    port.write('r');
    interval = millis();
    startTime = true;
    reference = gravity[1];
  }

  // 3-step rotation from yaw/pitch/roll angles (gimbal lock!)
  // ...and other weirdness I haven't figured out yet
  //rotateY(-ypr[0]);
  //rotateZ(-ypr[1]);
  //rotateX(-ypr[2]);

  //pupilX1 = map(mouseX, 0, width, 280, 290);
  //pupilY1 = map(mouseY, 0, height, 354, 358);
  //pupilX2 = map(mouseX, 0, width, 340, 350);
  //pupilY2 = map(mouseY, 0, height, 345, 348);                

  // toxiclibs direct angle/axis rotation from quaternion (NO gimbal lock!)
  // (axis order [1, 3, 2] and inversion [-1, +1, +1] is a consequence of
  // different coordinate system orientation assumptions between Processing
  // and InvenSense DMP)
  //float[] axis = quat.toAxisAngle();
  //rotate(axis[0], -axis[1], axis[3], axis[2]);
}

void initScreen() {
  background(0);
  textAlign(CENTER);
  textSize(32);
  text("How to fail your cat's attention?", width/2, height/2 - 80);
  textSize(24);
  text("Press S to Start", width/2, height/2);
}

void gameScreen() {
  startTime = true;

  background(255);
  imageMode(CENTER);
  image(img, width/2, height/2);
  noStroke();
  fill(0);
  ellipseMode(CENTER);

  drawHealthBar();
  eyeDisplay();
  checkAccel();
  
  player.play();
}

void gameOverScreen() {
  background(0);
  textAlign(CENTER);
  fill(255);
  text("Game Over", width/2, height/2 - 40);
  text("Press R to Restart", width/2, height/2 + 40);

  health = maxHealth;
  startTime = false;
  reference = gravity[1];
  
  player.pause();
}


void serialEvent(Serial port) {
  interval = millis();
  while (port.available() > 0) {
    int ch = port.read();

    if (synced == 0 && ch != '$') return;   // initial synchronization - also used to resync/realign if needed
    synced = 1;
    //print ((char)ch);

    if ((serialCount == 1 && ch != 2)
      || (serialCount == 12 && ch != '\r')
      || (serialCount == 13 && ch != '\n')) {
      serialCount = 0;
      synced = 0;
      return;
    }

    //if (serialCount > 0 || ch == ''){}

    if (serialCount > 0 || ch == '$') {
      teapotPacket[serialCount++] = (char)ch;
      if (serialCount == 14) {
        serialCount = 0; // restart packet byte position

        // get quaternion from data packet
        q[0] = ((teapotPacket[2] << 8) | teapotPacket[3]) / 16384.0f;
        q[1] = ((teapotPacket[4] << 8) | teapotPacket[5]) / 16384.0f;
        q[2] = ((teapotPacket[6] << 8) | teapotPacket[7]) / 16384.0f;
        q[3] = ((teapotPacket[8] << 8) | teapotPacket[9]) / 16384.0f;
        for (int i = 0; i < 4; i++) if (q[i] >= 2) q[i] = -4 + q[i];

        // set our toxilibs quaternion to new data
        quat.set(q[0], q[1], q[2], q[3]);


        // below calculations unnecessary for orientation only using toxilibs

        // calculate gravity vector
        gravity[0] = 2 * (q[1]*q[3] - q[0]*q[2]);
        gravity[1] = 2 * (q[0]*q[1] + q[2]*q[3]);
        gravity[2] = q[0]*q[0] - q[1]*q[1] - q[2]*q[2] + q[3]*q[3];

        // calculate Euler angles
        euler[0] = atan2(2*q[1]*q[2] - 2*q[0]*q[3], 2*q[0]*q[0] + 2*q[1]*q[1] - 1);
        euler[1] = -asin(2*q[1]*q[3] + 2*q[0]*q[2]);
        euler[2] = atan2(2*q[2]*q[3] - 2*q[0]*q[1], 2*q[0]*q[0] + 2*q[3]*q[3] - 1);

        // calculate yaw/pitch/roll angles
        ypr[0] = atan2(2*q[1]*q[2] - 2*q[0]*q[3], 2*q[0]*q[0] + 2*q[1]*q[1] - 1);
        ypr[1] = atan(gravity[0] / sqrt(gravity[1]*gravity[1] + gravity[2]*gravity[2]));
        ypr[2] = atan(gravity[1] / sqrt(gravity[0]*gravity[0] + gravity[2]*gravity[2]));

        // output various components for debugging
        //println("q:\t" + round(q[0]*100.0f)/100.0f + "\t" + round(q[1]*100.0f)/100.0f + "\t" + round(q[2]*100.0f)/100.0f + "\t" + round(q[3]*100.0f)/100.0f);
        //println("euler:\t" + euler[0]*180.0f/PI + "\t" + euler[1]*180.0f/PI + "\t" + euler[2]*180.0f/PI);
        //println("ypr:\t" + ypr[0]*180.0f/PI + "\t" + ypr[1]*180.0f/PI + "\t" + ypr[2]*180.0f/PI);
        //println("gravity:\t" + gravity[0] + "\t" + gravity[1] + "\t" + gravity[2]);
      }
    }
  }
}

void keyPressed() {
  if (key == 's' || key == 'S' || key == 'r' || key == 'R') {
    gameScreen = 1;
  }
}

void eyeDisplay() {
  fill(0);
  // translate everything to the middle of the viewport
  pushMatrix();
  translate(288, 356); 
  rotate(ypr[2]*2);
  ellipse(2, 2, 8, 8); 
  popMatrix(); 

  pushMatrix();
  translate(340, 349); 
  rotate(ypr[2]*2);
  ellipse(2, 2, 8, 8); 
  popMatrix();
}

void drawHealthBar() {
  rectMode(CORNER);

  if (health < 60) {
    fill(231, 76, 60);
  } else {
    fill(0);
  }

  rect(120, 20, healthBarWidth * (health/maxHealth), 5);
  textSize(18);
  text(int(health), 100, 30);
}

void increaseHealth() {
  steadyTime --;
  if (steadyTime == 0) {
    health += healthDecrease;
    if (health >= maxHealth) {
      health = maxHealth;
    }
    steadyTime = 10;
  }
}

void decreaseHealth() {
  health -= healthDecrease;
  if (health <= 0) {
    gameScreen = 2;
  }
}

void checkAccel() {
  if (startTime) {
    //if (abs(gravity[0]-0.0078)<0.2 && abs(gravity[1]+0.1483)<0.2 && abs(gravity[2]-0.989)<0.2) {
    //  decreaseHealth();
    //  println("Health decreased");
    //}


    total = total - readings[readIndex];
    readings[readIndex] = gravity[1];
    total = total + readings[readIndex];
    readIndex ++;

    if (readIndex >= numReadings) {
      readIndex = 0;
    }

    println("gravity:\t" + gravity[1] + "\t" + "average: \t" + average);

    average = total / numReadings;
    if (abs(average-reference)<0.12) {
      decreaseHealth();
      println("Health decreased\t" + reference);
    } else {
      increaseHealth();
    }
  }
}
