class Block{
  int posx;
  int posy;
  boolean collide;
  int power;
  color blockColor;
  
  Block(int x_, int y_){
    posx = x_;
    posy = y_;
    collide = false;
    power = 0; 
  }
  
  void update(){
    if (collide){
      blockColor = color(255);
    } else {
      blockColor = color(0);
    }
  }
  
  void display(){
    fill(blockColor);
    rect(posx, posy, 20, 20);
  }
}
