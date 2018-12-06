using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenemanager : MonoBehaviour
{
    Scene currentScene;
	int sceneIndex;

    //access the GameManager object
    private GameObject arduinoInput;
    GameController gameController;

    //Arduino Input Variables
    public GameObject[] steps;

    int proceedButton;
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

        steps[1].SetActive(false);
        steps[2].SetActive(false);
        steps[3].SetActive(false);

       
    }

    void Update()
    {
        //Assign buttonPress to the Property variable from the GameController Script 
        proceedButton = gameController.ButtonPressAcessor;
        resetButton = gameController.ResetButtonAccessor;


         
        //keyboard input test
         if(Input.GetKeyUp(KeyCode.Space)){
             proceedButton = 0;
         }else{proceedButton = 1;}

        if(Input.GetKeyDown(KeyCode.Return)){
            resetButton = 0;
        }else{resetButton = 1;}


        //Intro Scene
         if(steps[0].activeSelf && proceedButton == 0 )
         {
             steps[0].SetActive(false);
             steps[1].SetActive(true);
             return;
         }
         if(steps[1].activeSelf && proceedButton == 0 )
         {
             steps[1].SetActive(false);
             steps[2].SetActive(true);
             return;
         }
         if(steps[2].activeSelf && proceedButton == 0 )
         {
             steps[2].SetActive(false);
             steps[3].SetActive(true);
             return;
         }
         if(steps[3].activeSelf && proceedButton == 0 )
         {
             SceneManager.LoadScene(sceneIndex = 1, LoadSceneMode.Single);
             Debug.Log("Image 3");
         }

         else if( resetButton == 0 )
         {
            SceneManager.LoadScene(sceneIndex = 0, LoadSceneMode.Single);

             proceedButton = 1;
             resetButton = 1;
         }

         if(Input.GetKey(KeyCode.Escape)){
             Application.Quit();
              Debug.Log("Quit");
         }

         Debug.Log(sceneIndex);
        
   } 
}