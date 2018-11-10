class Person {
  int posx;
  int posy;
  boolean goal;

  Person(int x_, int y_) {
    posx = x_;
    posy = y_;
  }

  void update() {
    if (key == 'd') {
      posx += 20;
    }

    if (key == 'a') {
      posx -= 20;
    }
    if (key == 'w') {
      posy -= 20;
    }
    if (key == 's') {
      posy += 20;
    }
  }
}
