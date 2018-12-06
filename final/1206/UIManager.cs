using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	private GameObject sceneManagerGameInput;
	SceneManagerGame scenemanagerGame;
	int ovenTemp;

	public Slider ovenTempSlider;
	public Text timer;

	private float countDown;
	public float CountDownAccessor{
		get{return countDown;}
	}


	// Use this for initialization
	void Start () {
		sceneManagerGameInput = GameObject.Find("ScreenLoader");
		scenemanagerGame = sceneManagerGameInput.GetComponent<SceneManagerGame>();
		countDown = 120;
	}
	
	// Update is called once per frame
	void Update () {
		ovenTemp = scenemanagerGame.OvenTempProperty;
		ovenTempSlider.value = ovenTemp;

		countDown -= Time.deltaTime;

		timer.text = countDown.ToString("F");

		Debug.Log(countDown);

	}
}
