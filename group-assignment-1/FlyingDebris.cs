using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDebris : MonoBehaviour {
	private PlayerMove hitBalloon;

	public float speed = .5f; //how fast it moves
    public float distance = 2f; //how far right it moves
    private float startPosition; //needs to get back to start

	public bool noLoop = false; //there is a oscillating unless true
	public bool canThrow = false;
	public int thrust = 0;
	public string capsuleName;

    private Rigidbody rb; //kid rigidbody 
	void Start () {
		GameObject playerControl = GameObject.FindGameObjectWithTag("Control");
		hitBalloon = playerControl.GetComponent<PlayerMove>();

		startPosition = transform.position.x;
        rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
		if (!noLoop) {
        	Vector2 newPosition = this.transform.position; //temp variable
        	newPosition.x = Mathf.SmoothStep(startPosition, startPosition + distance, Mathf.PingPong(Time.time * speed, 1)); //assign
        	this.transform.position = newPosition; //new calculated position
		} else if(canThrow) {
			throwIt(thrust);
			canThrow = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player1")) {
			//stop player from flying
			hitBalloon.hit1 = true;
			Debug.Log("player 1 hit");
		}
		if(other.CompareTag("Player2")) {
			//stop player from flying
			hitBalloon.hit2 = true;
			Debug.Log("player 2 hit");
		}
	}

	IEnumerator KillEnemy() {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

	void throwIt(float throwForce) {
		rb.AddForce(transform.right * throwForce);
	}
}

	//instantiate and can start to move in screen from -4 going left/4 going right
	//if reaches -35/35 destroy object/send force back to where it started

