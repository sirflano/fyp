﻿using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using UnityEngine.UI;

public class shooty : MonoBehaviour {

    public GameObject bul;
    public float cooldown;
    private float curdown;
    private SerialPort controller;
    private string messageFromController;
    private bool runThread = true;
    private float x;
    private float y;
    private float z;
    private float initX = 0;
    private float initY = 0;
    private float initZ = 0;
    private bool gunCalibrated = true;
    private string calibarionString = "";
    //private Quaternion handRot;

    public bool Connected = false;
    public string portName = "COM3";

    public volatile float pot;
    public volatile bool button;
    public Text displayString;
    public GameObject player;
    public Quaternion currentAngle;
    

    float scale = 1.0f;
	// Use this for initialization
	void Start () {
        LookForController();

        // create the thread
        runThread = true;
        Thread ThreadForController = new Thread(new ThreadStart(ThreadWorker));
        ThreadForController.Start();
    }
	
	// Update is called once per frame
	void Update () {
        displayString.text = calibarionString;
        curdown = curdown - 1 * Time.deltaTime;
        if (button && curdown <= 0)
        {
            
            GameObject bullet = Instantiate(bul, transform.position, transform.rotation) as GameObject;
            curdown = cooldown;
            button = false;
        }
        else if(Input.GetKeyDown("space") && curdown <=0)
        {
            GameObject bullet = Instantiate(bul, transform.position, transform.rotation) as GameObject;
            curdown = cooldown;
        }
        else if(Input.GetKeyDown("space") || button) {
            Debug.Log("Fire!");
        }
        if(gunCalibrated)
        {
            Time.timeScale = 1;
            //transform.localRotation = Quaternion.Euler(-x, y, z);
            transform.localRotation = Quaternion.Euler(-x, -y, -z);
            currentAngle = transform.localRotation;
           // Debug.Log("GunRotation:" + transform.localRotation);
        }
        else
        {
            Time.timeScale = 0;
        }
        
        //transform.rotation = Quaternion.Euler(x, y, z);
    }
    void ProcessMessage(string message)
    {
        string[] decoded = message.Split(':');
        //float value = float.Parse(decoded[1]);
        /*switch (decoded[0])
        {
            case "H":
                Debug.Log("FIRE");
                button = true;
                break;
            case "L":
                button = false;
                break;
        }*/
        //print(message);
            //Debug.Log(message);
        if(decoded[1] == "NotConfigured")
        {
            gunCalibrated = false;
            calibarionString = message;
            Debug.Log(message);
        }
        else
        {
            if(!gunCalibrated)
            {
                gunCalibrated = true;
                
                //handRot = player.GetComponent<CubemanController>().handsRot;
                //if(initX == 0 && initY == 0 && initZ == 0)
                //{
                    initY = float.Parse(decoded[1]);
                    initX = float.Parse(decoded[2]);
                    initZ = float.Parse(decoded[3]);
                //}
                //else
                //{
                //    initY = y;
                //    initX = x;
                //    initZ = z;
                //}
            }
            Debug.Log(message);
            calibarionString = "";
            /*if(initX - float.Parse(decoded[2]) > 180.0f || initX - float.Parse(decoded[2]) < -180)
            {
                x = initX - (float.Parse(decoded[2]) - 180.0f);
            }
            else
            {
                x = initX - float.Parse(decoded[2]);
            }
            if (initZ - float.Parse(decoded[3]) > 180.0f || initZ - float.Parse(decoded[3]) < -180)
            {
                z = initZ - (float.Parse(decoded[3]) - 180.0f);
            }
            else
            {
                z = initZ - float.Parse(decoded[3]);
            }
            if (initY - float.Parse(decoded[1]) > 180.0f || initY - float.Parse(decoded[1]) < -180)
            {
                y = initY - (float.Parse(decoded[1]) - 180.0f);
            }
            else
            {
                y = initY - float.Parse(decoded[1]);
            }*/

            x = initX - float.Parse(decoded[2]);
            y = initY - float.Parse(decoded[0]);
            z = initZ - float.Parse(decoded[1]);
            if (decoded.Length > 3)
            {
                button = true;
            }
        }

        //transform.rotation = Quaternion.Euler(float.Parse(decoded[1]), float.Parse(decoded[2]), float.Parse(decoded[3]));
        /*
        print(message);
        switch(message)
        {
            case "H":
                button = true;
                break;
            case "L":
                button = false;
                break;
        }*/

    }

    void ThreadWorker()
    {
        while (runThread)
        {
            if (controller != null && controller.IsOpen)
            {
                try
                {
                    messageFromController = controller.ReadLine();
                    ProcessMessage(messageFromController);
                }
                catch (System.Exception) { }
            }
            else
            {
                Thread.Sleep(50);
            }
        }
    }


    void OnApplicationQuit()
    {
        controller.Close();
        runThread = false;
    }

    public void LookForController()
    {
        string[] ports = SerialPort.GetPortNames();
        Debug.Log(ports.Length);

        if (ports.Length == 0)
        {
            Debug.Log("No controller detected");
        }
        else
        {
            Debug.Log("Ports: " + string.Join(", ", ports));
            portName = "\\\\.\\" + ports[ports.Length - 1];
            Debug.Log("Port Name: " + portName);
            Connected = true;

            // check the default port
            controller = new SerialPort(portName, 9600);
            controller.ReadTimeout = 100;
            controller.Open();
        }
    }
}
