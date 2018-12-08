using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerGame : MonoBehaviour {

    Scene currentScene;
	int sceneIndex;

    //access the GameManager object
    private GameObject arduinoInput;
    GameController gameController;

    //Arduino Input Variables
	private GameObject timerVariable;
	UIManager uimanager;
	float countDown;
    int proceedButton;
    int resetButton;
    int ovenTemp;
    int keyPad;
    int appleSliceCounter;
    int crustReady;
    int creamRotaryEncoder;
    int broilRotaryEncoder;
    int gameOvenTemp;

    public GameObject[] steps;
    public int OvenTempProperty{
        get{return gameOvenTemp;}
    }

    bool tempIsGood;
	bool proceedCounter = false;
    int prevproceedButton;


    void Start()
    {
        arduinoInput = GameObject.Find("GameManagement");
        gameController = arduinoInput.GetComponent<GameController>();
		timerVariable = GameObject.Find("UIManager");
		uimanager = timerVariable.GetComponent<UIManager>();;

        currentScene = SceneManager.GetActiveScene ();
        sceneIndex = currentScene.buildIndex;

        prevproceedButton = 1;

       
        proceedButton = 1;
        resetButton = 1;
        ovenTemp = 1;
        keyPad =0;
        appleSliceCounter = 0;
        crustReady = 0;
        creamRotaryEncoder = 0;
        broilRotaryEncoder = 0;

        steps [1].SetActive(false);
		steps [2].SetActive(false);
		steps [3].SetActive(false);
		steps [4].SetActive(false);
		steps [5].SetActive(false);
		steps [6].SetActive(false);
		steps [7].SetActive(false);
		steps [8].SetActive(false);
		steps [9].SetActive(false);
		steps [10].SetActive(false);
		steps [11].SetActive(false);
		steps [12].SetActive(false);
		steps [13].SetActive(false);
		steps [14].SetActive(false);
       
    }

    void LateUpdate()
    {
        //Assign buttonPress to the Property variable from the GameController Script 
        proceedButton = gameController.ButtonPressAcessor;
        resetButton = gameController.ResetButtonAccessor;
        ovenTemp = gameController.OvenTempAccessor;
        keyPad = gameController.KeyPadAccessor;
        appleSliceCounter = gameController.AppleSliceCounterAccessor;
        crustReady = gameController.CrustReadyAccessor;
        creamRotaryEncoder = gameController.CreamRotaryEncoderAccesor;
        broilRotaryEncoder = gameController.BroilRotaryEncoderAccessor;
		countDown = uimanager.CountDownAccessor;
         
        //keyboard input test
        //  if(Input.GetKey(KeyCode.Space)){
        //      proceedButton = 0;
        //  }else{proceedButton = 1;}

        // if(Input.GetKeyDown(KeyCode.Keypad2)){
        //     resetButton = 0;
        // }else{resetButton = 1;}

        // if(Input.GetKey("space")){
        //     ovenTemp ++;
        //     Debug.Log("ovenTemp= " + ovenTemp);
        // }else if(ovenTemp >= 0){ovenTemp--;}
        // if(Input.GetKeyDown(KeyCode.Keypad4)){
        //    keyPad = 1;
        // }
        // if(Input.GetKeyDown(KeyCode.Keypad5)){
        //    appleSliceCounter++;
        //    Debug.Log ("apples= " + appleSliceCounter);
        // }
        // if(Input.GetKeyDown(KeyCode.Keypad6)){
        //    crustReady = 1;
        // }
        //  if(Input.GetKey(KeyCode.Keypad7)){
        //    creamRotaryEncoder++;
        // }
        // if(Input.GetKey(KeyCode.Keypad8)){
        //    broilRotaryEncoder++;
        // }
       if (proceedButton == 0 && prevproceedButton != proceedButton)
        {
            proceedCounter = true;
        } else {
            proceedCounter = false;
        }

        if (ovenTemp == 0)
        { gameOvenTemp+= 2; }
        else if (ovenTemp >= 0)
        { gameOvenTemp-= 1; }

        if (gameOvenTemp >= 250 && gameOvenTemp <=450)
         {
            tempIsGood = true;
         }else{tempIsGood = false;}

        //Debug.Log("Temp is good? " + tempIsGood);

        //GameScene
         if(steps[0].activeSelf && gameOvenTemp >= 5 )
         {
             steps[0].SetActive(false);
             steps[1].SetActive(true);
             Debug.Log("over 5");
         }
         else if(steps[1].activeSelf && proceedCounter)
         {
             steps[1].SetActive(false);
             steps[2].SetActive(true);
			 proceedCounter = false;
			 return;
         }
		 else if(steps[2].activeSelf && proceedCounter)
         {
             steps[2].SetActive(false);
             steps[3].SetActive(true);
			 proceedCounter = false;
			 return;
         }
         else if(steps[3].activeSelf && keyPad == 1)
         {
             steps[3].SetActive(false);
             steps[4].SetActive(true);
         }
         else if(steps[4].activeSelf && proceedCounter)
         {
             steps[4].SetActive(false);
             steps[5].SetActive(true);
			 proceedCounter = false;
			 return;
         }
         else if(steps[5].activeSelf && proceedCounter)
         {
             steps[5].SetActive(false);
             steps[6].SetActive(true);
			 proceedCounter = false;
			 return;
         }
         else if(steps[6].activeSelf && appleSliceCounter >=6)
         {
             steps[6].SetActive(false);
             steps[7].SetActive(true);
         }
         else if(steps[7].activeSelf && proceedCounter)
         {
             steps[7].SetActive(false);
             steps[8].SetActive(true);
			 proceedCounter = false;
			 return;
         }
          else if(steps[8].activeSelf && proceedCounter)
         {
             steps[8].SetActive(false);
             steps[9].SetActive(true);
			 proceedCounter = false;
			 return;
         }
         else if(steps[9].activeSelf && crustReady == 9)
         {
            if(tempIsGood)
            {
             steps[9].SetActive(false);
             steps[10].SetActive(true);
            }else{SceneManager.LoadScene(sceneIndex = 2, LoadSceneMode.Single);}
         }
         else if(steps[10].activeSelf && proceedCounter)
         {
             steps[10].SetActive(false);
             steps[11].SetActive(true);
			 proceedCounter = false;
			 return;
         }
         else if(steps[11].activeSelf && creamRotaryEncoder >= 20)
         {
			  if(tempIsGood)
			  {
				steps[11].SetActive(false);
            	steps[12].SetActive(true);
			  }else{SceneManager.LoadScene(sceneIndex = 2, LoadSceneMode.Single);}
             
         }
         else if(steps[12].activeSelf && proceedCounter)
         {
             steps[12].SetActive(false);
             steps[13].SetActive(true);
			 proceedCounter = false;
			 return;
         }
         else if(steps[13].activeSelf && broilRotaryEncoder >= 20)
         {
             steps[13].SetActive(false);
             steps[14].SetActive(true);
         }
          else if(steps[14].activeSelf && proceedCounter)
         {
             SceneManager.LoadScene(sceneIndex = 3, LoadSceneMode.Single);
			 proceedCounter = false;
			 return;
         }
         else if( resetButton == 0 )
         {
            SceneManager.LoadScene(sceneIndex = 0, LoadSceneMode.Single);

             proceedButton = 1;
             resetButton = 1;
             ovenTemp = 1;
             keyPad =0;
             appleSliceCounter = 0;
             crustReady = 0;
             creamRotaryEncoder = 0;
             broilRotaryEncoder = 0;
         }

		 if(countDown<=0){
			 SceneManager.LoadScene(sceneIndex = 4, LoadSceneMode.Single);
		 }

		 

		  if(Input.GetKeyDown(KeyCode.Escape)){
             Application.Quit();
			  Debug.Log("Quit");
         }

            prevproceedButton = proceedButton;


         Debug.Log(sceneIndex);
        
   } 
}