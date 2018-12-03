using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController1 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("Step1", LoadSceneMode.Single);
        }

        if (GameObject.Find("GameManagement").GetComponent<GameController1>().proceed)
        {
            SceneManager.LoadScene("Step1", LoadSceneMode.Single);
        }
    }
}