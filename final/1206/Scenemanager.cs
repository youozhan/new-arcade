using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenemanager : MonoBehaviour
{
    Scene currentScene;
	int sceneIndex;

    bool proceedCounter = false;

    //access the GameManager object
    private GameObject arduinoInput;
    GameController gameController;

    //Arduino Input Variables
    public GameObject[] steps;

    int proceedButton;

    int prevproceedButton;

    int resetButton;
    public int scenIndexProperty{
        get{return sceneIndex;}
    }


    void Start()
    {
        arduinoInput = GameObject.Find("GameManagement");
        gameController = arduinoInput.GetComponent<GameController>();
    
        currentScene = SceneManager.GetActiveScene ();
        sceneIndex = currentScene.buildIndex;
       
        proceedButton = 1;
        resetButton = 1;
        prevproceedButton = 1;

        steps[1].SetActive(false);
        steps[2].SetActive(false);
        steps[3].SetActive(false);

       
    }

    void LateUpdate()
    {
        //Assign buttonPress to the Property variable from the GameController Script 
        proceedButton = gameController.ButtonPressAcessor;
        resetButton = gameController.ResetButtonAccessor;
        Debug.Log("current " + proceedButton + " prev " + prevproceedButton);
         
        //keyboard input test
        //  if(Input.GetKeyUp(KeyCode.Space)){
        //      proceedButton = 0;
        //  }else{proceedButton = 1;}

        // if(Input.GetKeyDown(KeyCode.Return)){
        //     resetButton = 0;
        // }else{resetButton = 1;}

        //Intro Scene

        if (proceedButton == 0 && prevproceedButton != proceedButton)
        {
            proceedCounter = true;
        } else {
            proceedCounter = false;
        }

         if(steps[0].activeSelf && proceedCounter)
         {
             steps[0].SetActive(false);
             steps[1].SetActive(true);
             Debug.Log("Test connection");
             proceedCounter = false;
            //  return;
         }
         if(steps[1].activeSelf && proceedCounter)
         {
             steps[1].SetActive(false);
             steps[2].SetActive(true);
             proceedCounter = false;
            //  return;
         }
         if(steps[2].activeSelf && proceedCounter)
         {
             steps[2].SetActive(false);
             steps[3].SetActive(true);
             proceedCounter = false;
            //  return;
         }
         if(steps[3].activeSelf && proceedCounter)
         {
             SceneManager.LoadScene(sceneIndex = 1, LoadSceneMode.Single);
             proceedCounter = false;
             Debug.Log("Image 3");
         }

         if( resetButton == 0 )
         {
            SceneManager.LoadScene(sceneIndex = 0, LoadSceneMode.Single);

             proceedButton = 1;
             resetButton = 1;
         }

         if(Input.GetKey(KeyCode.Escape)){
             Application.Quit();
              Debug.Log("Quit");
         }

        //  if(proceedButton == 0){
        //     proceedCounter = false;
        // }

         prevproceedButton = proceedButton;

         Debug.Log(sceneIndex);

        //  if(proceedCounter == 0) {
        //      proceedCounter = 10;
        //  }
        
   } 
}