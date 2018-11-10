PFont font;

Block[] blocks = new Block[50];
Person person1;
Person person2;

int x1 = int(random(0, 4));
int y1 = int(random(0, 4));
int x2 = int(random(0, 4));
int y2 = int(random(0, 4));

void setup() {
  size(720, 480);
  background(255);
  font = createFont("Futura-Medium", 28);
  textFont(font);
  textAlign(LEFT);
  textSize(32);


  int index = 0;

  for (int i = 0; i < 5; i++) {
    for ( int k = 0; k < 5; k++) { 
      blocks[index] = new Block(120 + 20 * i, 160 + 20 * k);
      blocks[25 + index] = new Block(500 + 20 * i, 160 + 20 * k);
      index ++;
    }
  }

  person1 = new Person(120, 160);
  person2 = new Person(500, 160);
}

void draw() {
  background(255);
  stroke(0);
  line(width/2, 0, width/2, height);
  treasure();

  for (int  i = 0; i < 50; i ++) {
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

  //person1.update();
  //person2.update();

  //println (person1.posx + " " + person1.posy + " " + person2.posx + " " + person2. posy);
}

void keyReleased() {
  person1.update();
  person2.update();
}

void treasure() {
  //int x1 = int(random(0, 4));
  //int y1 = int(random(0, 4));
  //int x2 = int(random(0, 4));
  //int y2 = int(random(0, 4));
  println(x1 + " " + y1 + " " + x2 + " " + y2);

  if (person1.posx == 120 + 20 * x1 && person1.posy == 160 + 20 * y1) {
    person1.goal = true;
    println("person1 wins");
    text("Player 1 wins", 40, 40);
  }

  if (person2.posx == 500 + 20 * x2 && person2.posy == 160 + 20 * y2) {
    person1.goal = true;
    println("person2 wins");
    text("Player 2 wins", 40, 40);
  }
}
