using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.IO.Ports;

public class PlayerController : MonoBehaviour {

    public float speed;
    private Rigidbody rb;
    private int count;
    public Text countText;
    public Text winText;

    private SerialPort serialPort = null;
    private String portName = "/dev/cu.usbmodem14641";
    private int baudRate =  9600;             
    private int readTimeOut = 100;

    private string serialInput;
    bool programActive = true;
    Thread thread;

	private void Start()
	{
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountTest();
        winText.text = "";

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

	private void FixedUpdate()
	{
        if(serialInput != null){
            string[] strEul = serialInput.Split(',');
            if (strEul.Length > 0)
            {
                if (int.Parse(strEul[0]) != 0)
                {
                    speed = speed + 10;
                    Debug.Log("Button 1 detected");
                } else if (int.Parse(strEul[1]) != 0) 
                {
                    speed = speed - 10;
                    Debug.Log("Button 2 detected");
                }
            }

        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
	}

	private void OnTriggerEnter(Collider other)
	{
        if(other.gameObject.CompareTag("Pick Up")){
            other.gameObject.SetActive(false);
            count++;
            SetCountTest();
        }
	}

    private void SetCountTest(){
        countText.text = "Count: " + count.ToString();
        if(count>=8){
            winText.text = "You win!";
        }
    }

	public void OnDisable()
	{
        programActive = false;
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
	}
}
