// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using System.IO.Ports;
// using System.Threading;

// public class PlayerStart : MonoBehaviour
// {

//     //public int playerNum = 1;
//     public float thrust = 50f;
//     public Rigidbody rb1;
//     public Rigidbody rb2;


//     // Add connection from Arduino
//     private SerialPort serialPort = null;
//     private String portName = "/dev/cu.usbmodem14641";
//     private int baudRate = 9600;
//     private int readTimeOut = 100;

//     private string serialInput;
//     bool programActive = true;
//     Thread thread;

//     private int btn1 = 0;
//     private int btn2 = 0;

//     void Start()
//     {

//         try{
//             serialPort = new SerialPort();
//             serialPort.PortName = portName;
//             serialPort.BaudRate = baudRate;
//             serialPort.ReadTimeout = readTimeOut;
//             serialPort.Open();
//         }

//         catch(Exception e){
//             Debug.Log(e.Message);
//         }

//         thread = new Thread(new ThreadStart(ProcessData));
//         thread.Start();
//     }

//     void FixedUpdate()
//     {
//         if (serialInput != null){
//             string[] strEul = serialInput.Split(',');
//             if(strEul.Length > 0){
//                 if(int.Parse(strEul[0])!= 0)
//                 {
//                     btn1 = 1;
//                     Debug.Log("Button 1 detected");
//                 } else {
//                     btn1 = 0;
//                 }

//                 if (int.Parse(strEul[1]) != 0){
//                     btn2 = 1;
//                     Debug.Log("Button 2 detected");
//                 } else {
//                     btn2 = 0;
//                 }
                
//                 //moveBalloon(btn1, btn2);
//             }
//         }
//     }

//     void ProcessData(){
//         Debug.Log("Thread:Start");
//         while(programActive){
//             try{
//                 serialInput = serialPort.ReadLine();
//             } catch(TimeoutException){
                
//             }
//         }
//         Debug.Log("Thread:Stop");
//     }

//     // void moveBalloon(int move1, int move2)
//     // {
//     //     if (move1 == 1 && hit1 != true)
//     //     {
//     //         //add force up to balloon
//     //         rb1.AddForce(transform.up * thrust);
//     //     }
//     //     else if (move1 == 0 || hit1 == true)
//     //     {
//     //         //stall with gravity for a sec and then push balloon down
//     //         StartCoroutine(WaitAndFall(1.5f, 1));
//     //     }
//     //     if (move2 == 1 && hit2 != true)
//     //     {
//     //         //add force up to balloon
//     //         rb2.AddForce(transform.up * thrust);
//     //     }
//     //     else if (move2 == 0 || hit2 == true)
//     //     {
//     //         //stall with gravity for a sec and then push balloon down
//     //         StartCoroutine(WaitAndFall(1.5f, 2));
//     //     }
//     // }
// }