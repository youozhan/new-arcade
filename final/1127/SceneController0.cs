using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController0 : MonoBehaviour
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
            SceneManager.LoadScene("Instructions", LoadSceneMode.Single);
        }

        if (GameObject.Find("GameManagement").GetComponent<GameController0>().proceed)
        {
            SceneManager.LoadScene("Instructions", LoadSceneMode.Single);
        }
    }
}