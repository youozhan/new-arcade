class Person {
  int posx;
  int posy;
  boolean goal;
  float distance;
  boolean close;
  boolean special;

  Person(int x_, int y_) {
    posx = x_;
    posy = y_;
  }

  void update() {
    posx = 120 + 20 * sensor0;
    posy = 160 + 20 * sensor1;
  //  if (key == 'd' || keyCode == RIGHT) {
  //    posx += 20;
  //  }

  //  if (key == 'a' || keyCode == LEFT ) {
  //    posx -= 20;
  //  }
  //  if (key == 'w' || keyCode == UP) {
  //    posy -= 20;
  //  }
  //  if (key == 's' || keyCode == DOWN) {
  //    posy += 20;
  //  }
  }
}
