using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    public GameObject winnerParticle1;
    public GameObject winnerParticle2;
    public GameObject winnerText1;
    public GameObject winnerText2;

    public bool p1Text = false;
    public TextMeshProUGUI h1Text;
    public TextMeshProUGUI h2Text;
    public bool winner = false;


    public float thrust1 = 25f;
    public float thrust2 = 25f;
    public GameObject playerOne;
    public GameObject playerTwo;
    private Rigidbody rb1;
    private Rigidbody rb2;

    public bool hit1 = false;
    public bool hit2 = false;


    // Add connection from Arduino
    private SerialPort serialPort = null;
    private String portName = "/dev/cu.usbmodem146401";
    private int baudRate = 9600;
    private int readTimeOut = 100;

    private string serialInput;
    bool programActive = true;
    Thread thread;

    private int btn1 = 0;
    private int btn2 = 0;

    void Start()
    {
        winnerParticle1.SetActive(false);
        winnerParticle2.SetActive(false);
        winnerText1.SetActive(false);
        winnerText2.SetActive(false);

        rb1 = playerOne.GetComponent<Rigidbody>();
        rb2 = playerTwo.GetComponent<Rigidbody>();

        try{
            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.ReadTimeout = readTimeOut;
            serialPort.Open();
        }

        catch(Exception e){
            Debug.Log(e.Message);
        }

        thread = new Thread(new ThreadStart(ProcessData));
        thread.Start();
    }

    void FixedUpdate()
    {
        if (serialInput != null){
            string[] strEul = serialInput.Split(',');
            if(strEul.Length > 0){
                if(int.Parse(strEul[0])!= 0)
                {
                    btn1 = 1;
                    Debug.Log("Button 1 detected");
                } else {
                    btn1 = 0;
                    StartCoroutine(WaitAndFall(1.5f, 1));
                }

                if (int.Parse(strEul[1]) != 0){
                    btn2 = 1;
                    Debug.Log("Button 2 detected");
                } else {
                    btn2 = 0;
                    StartCoroutine(WaitAndFall(1.5f, 2));
                }
                
                moveBalloon(btn1, btn2);
            }
        }

        HeightDisplay();
        CheckForWinner();
    }

    void ProcessData(){
        Debug.Log("Thread:Start");
        while(programActive){
            try{
                serialInput = serialPort.ReadLine();
            } catch(TimeoutException){
                
            }
        }
        Debug.Log("Thread:Stop");
    }

    IEnumerator WaitAndFall(float waitTime, int playerNum)
    {
        yield return new WaitForSeconds(waitTime);
        if (playerNum == 1)
        {
            rb1.AddForce(transform.up * -25f);
        }
        if (playerNum == 2)
        {
            rb2.AddForce(transform.up * -25f);
        }
    }

    void moveBalloon(int move1, int move2)
    {
        if (move1 == 1 && hit1 != true)
        {
            //add force up to balloon
            rb1.AddForce(transform.up * thrust1);
        }
        else if (move1 == 0 || hit1 == true)
        {
            //stall with gravity for a sec and then push balloon down
            StartCoroutine(WaitAndFall(1.5f, 1));
        }
        if (move2 == 1 && hit2 != true)
        {
            //add force up to balloon
            rb2.AddForce(transform.up * thrust2);
        }
        else if (move2 == 0 || hit2 == true)
        {
            //stall with gravity for a sec and then push balloon down
            StartCoroutine(WaitAndFall(1.5f, 2));
        }
    }

    void HeightDisplay() {
        h1Text.text = "height: " + ((int)Math.Round(playerOne.transform.position.y) + 1);
        h2Text.text = "height: " + ((int)Math.Round(playerTwo.transform.position.y) + 1);
    }

    void CheckForWinner() {
        if (((int)Math.Round(playerOne.transform.position.y) + 1) > 150 && !winner) {
            //player one wins
            Debug.Log("player one won");
            winnerParticle1.SetActive(true);
            winnerText1.SetActive(true);
            rb1.isKinematic = true;
            winner = true;
        }
        if (((int)Math.Round(playerTwo.transform.position.y) + 1) > 150 & !winner) {
            //player Two wins
            Debug.Log("player two won");
            winnerParticle2.SetActive(true);
            winnerText2.SetActive(true);
            rb2.isKinematic = true;
            winner = true;
        }
    }
}