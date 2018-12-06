using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {

	Scene currentScene;
	int sceneIndex;
	int resetButton;

	//access the GameManager object
    private GameObject arduinoInput;
    GameController gameController;


	void Start () {
		arduinoInput = GameObject.Find("GameManagement");
        gameController = arduinoInput.GetComponent<GameController>();

		currentScene = SceneManager.GetActiveScene ();
        sceneIndex = currentScene.buildIndex;
	}
	
	// Update is called once per frame
	void Update () {

        resetButton = gameController.ResetButtonAccessor;

        // if(Input.GetKeyDown(KeyCode.Keypad2)){
        //     resetButton = 0;
        // }else{resetButton = 1;}

		if( resetButton == 0 )
         {
            SceneManager.LoadScene(sceneIndex = 0, LoadSceneMode.Single);
         }

		  if(Input.GetKeyDown(KeyCode.Escape)){
             Application.Quit();

			 Debug.Log("Quit");
         }
	}
}
