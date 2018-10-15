PImage img;

void setup() {
  size(600, 800);
  img = loadImage("Asset_1.png");
}
 
void draw() {
  background(255);
  imageMode(CENTER);
  image(img, width/2, height/2);
  noStroke();
  float pupilX1 = map(mouseX, 0, width, 280, 290);
  float pupilY1 = map(mouseY, 0, height, 354, 358);
  float pupilX2 = map(mouseX, 0, width, 340, 350);
  float pupilY2 = map(mouseY, 0, height, 345, 348);
  fill(0);
  if(mousePressed)                   
  fill(0);
  ellipseMode(CENTER);
  ellipse(pupilX1, pupilY1, 8, 11); 
  ellipse(pupilX2, pupilY2, 8, 11); 
  
  }
