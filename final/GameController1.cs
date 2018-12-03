using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.IO.Ports;

public class GameController1 : MonoBehaviour
{

    private SerialPort serialPort = null;
    private String portName = "/dev/cu.usbmodem141301";
    private int baudRate = 9600;
    private int readTimeOut = 100;

    private string serialInput;
    bool programActive = true;
    Thread thread;

    public Boolean proceed = false;
    public Boolean restart = false;

    // Use this for initialization
    void Start()
    {
        try
        {
            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.ReadTimeout = readTimeOut;
            serialPort.Open();
        }

        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        thread = new Thread(new ThreadStart(ProcessData));
        thread.Start();
    }

    void ProcessData()
    {
        Debug.Log("Thread:Start");
        while (programActive)
        {
            try
            {
                serialInput = serialPort.ReadLine();
            }
            catch (TimeoutException)
            {

            }
        }
        Debug.Log("Thread:Stop");
    }

    // Update is called once per frame
    void Update()
    {
        if (serialInput != null)
        {
            string[] strEul = serialInput.Split(',');
            if (strEul.Length > 0)
            {
                //Debug.Log(strEul[0] + " " + strEul[1]);
                if (int.Parse(strEul[0]) != 0)
                {
                    Debug.Log("Button 1 detected");
                    proceed = true;
                }
                else if (int.Parse(strEul[1]) != 0)
                {
                    Debug.Log("Button 2 detected");
                    restart = true;
                }
            }
            else
            {
                proceed = false;
                restart = false;
            }

        }
    }

    public void OnDisable()
    {
        programActive = false;
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}
