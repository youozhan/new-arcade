using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;

public class PlayerMove_copy : MonoBehaviour
{

    //public int playerNum = 1;
    public float thrust = 50f;
    public Rigidbody rb1;
    public Rigidbody rb2;
    //public bool hit1 = false;
    //public bool hit2 = false;
    //SerialPort sp = new SerialPort("COM3", 9600);


    // Add connection from Arduino
    private SerialPort serialPort = null;
    private String portName = "/dev/cu.usbmodem14641";
    private int baudRate = 9600;
    private int readTimeOut = 100;

    private string serialInput;
    bool programActive = true;
    Thread thread;

    void Start()
    {
        //sp.Open();
        //sp.ReadTimeout = 1;

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

    void Update()
    {
        if (serialInput != null){
            string[] strEul = serialInput.Split(',');
            if(strEul.Length > 0){
                if(int.Parse(strEul[0])!= 0)
                {
                    rb1.AddForce(transform.up * thrust);
                    Debug.Log("Button 1 detected");
                } else {
                    StartCoroutine(WaitAndFall(1.5f, 1));
                }

                if (int.Parse(strEul[1]) != 0){
                    rb2.AddForce(transform.up * thrust);
                    Debug.Log("Button 2 detected");
                } else {
                    StartCoroutine(WaitAndFall(1.5f, 2));
                }
            }
        }

        //if (sp.IsOpen)
        //{
        //    try
        //    {
        //        moveBalloon(sp.ReadByte(), sp.ReadByte());
        //        print(sp.ReadByte());
        //    }
        //    catch (System.Exception)
        //    {
        //    }
        //}

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

    //void moveBalloon(int move1, int move2)
    //{
    //    if (move1 == 1 && hit1 != true)
    //    {
    //        //add force up to balloon
    //        rb1.AddForce(transform.up * thrust);
    //    }
    //    else if (move1 == 0 || hit1 == true)
    //    {
    //        //stall with gravity for a sec and then push balloon down
    //        StartCoroutine(WaitAndFall(1.5f, 1));
    //    }
    //    if (move2 == 1 && hit2 != true)
    //    {
    //        //add force up to balloon
    //        rb2.AddForce(transform.up * thrust);
    //    }
    //    else if (move2 == 0 || hit2 == true)
    //    {
    //        //stall with gravity for a sec and then push balloon down
    //        StartCoroutine(WaitAndFall(1.5f, 2));
    //    }
    //}
}