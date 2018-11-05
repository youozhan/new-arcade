using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowEnemy : MonoBehaviour {
	private FlyingDebris throwEnemy;
    void Start() {
        GameObject flyingEnemies = GameObject.FindGameObjectWithTag("Satellite");
		throwEnemy = flyingEnemies.GetComponent<FlyingDebris>();
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.tag+" "+this.tag);
        if(this.gameObject.name == "Trigger1"){
            if(other.gameObject.CompareTag("Player2")) {
                //do stuff for blue player (player2)
                throwEnemy.canThrow = true;
                throwEnemy.thrust = 30;
                throwEnemy.capsuleName = "Capsule1";
            }
        }
        if(this.gameObject.name == "Trigger2"){
            if(other.gameObject.CompareTag("Player1")) {
                //do stuff for red player (player1)
                throwEnemy.canThrow = true;
                throwEnemy.thrust = -30;
                throwEnemy.capsuleName = "Capsule2";
            }
        }
    }
}
