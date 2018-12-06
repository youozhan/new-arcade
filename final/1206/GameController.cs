using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class GameController : MonoBehaviour {

    //access the serial port the Arduino is communicating with
    //private SerialPort stream = new SerialPort("COM5", 9600);
    private SerialPort stream = new SerialPort("/dev/cu.usbmodem141401", 9600);
    // private SerialPort stream = new SerialPort("/dev/cu.usbmodem1461", 9600);

    //declare variable for the button input
    //create the public property for the scenemanager script to access

    private int proceedButton;
	public int ButtonPressAcessor{
		get{return proceedButton;}
	}

    private int resetButton;
    public int ResetButtonAccessor
    {
        get { return resetButton; }
    }

    private int ovenTemp;
    public int OvenTempAccessor
    {
        get { return ovenTemp; }
    }

    private int keyPad;
    public int KeyPadAccessor
    {
        get { return keyPad; }
    }

    private int appleSliceCounter;
    public int AppleSliceCounterAccessor
    {
        get { return appleSliceCounter; }
    }

    private int crustReady;
    public int CrustReadyAccessor
    {
        get { return crustReady; }
    }


    private int creamRotaryEncoder;
    public int CreamRotaryEncoderAccesor
    {
        get { return creamRotaryEncoder; }
    }

    private int broilRotaryEncoder;
    public int BroilRotaryEncoderAccessor
    {
        get { return broilRotaryEncoder; }
    }

    private static GameController gameContollerInstance;

	 void Awake(){
		DontDestroyOnLoad (this);
			
		if (gameContollerInstance == null) {
			gameContollerInstance = this;
		} else {
			Destroy(gameObject);
		}
	 }

	void Start () {
		//start reading the arduino serial
		stream.Open();
		stream.ReadTimeout = 20;
		StartCoroutine(readString());

	}

	//read the serial stream
	IEnumerator readString() {

		while (true) {

			if (stream.IsOpen) {

				try
				{
					//save the serial data as a string
					string input = stream.ReadLine();
					string[] values = input.Split(',');
					//save the string data is an int that will be used in the property above
    	     	    proceedButton = int.Parse(values[0]);
                 resetButton = int.Parse(values[1]);
                 ovenTemp = int.Parse(values[2]);
                 keyPad = int.Parse(values[3]);
                 appleSliceCounter = int.Parse(values[4]);
                 crustReady = int.Parse(values[5]);
                 creamRotaryEncoder = int.Parse(values[6]);
                 broilRotaryEncoder = int.Parse(values[7]);

                    Debug.Log(proceedButton + ", " + resetButton + ", " + ovenTemp + ", " + keyPad + ", " + appleSliceCounter + ", " +crustReady + ", " + creamRotaryEncoder + ", " + broilRotaryEncoder + ".");

                }
                catch (System.Exception) {
				}
			}
			yield return null;
		}
	}
}
