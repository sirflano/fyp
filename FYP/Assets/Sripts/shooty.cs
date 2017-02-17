using UnityEngine;
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
    private float initX;
    private float initY;
    private float initZ;
    private bool gunCalibrated = true;
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
        
        if (button && curdown <= 0)
        {
            
            GameObject bullet = Instantiate(bul, transform.position, transform.rotation) as GameObject;
            curdown = cooldown;
        }
        else if(Input.GetKeyDown("space") && curdown <=0)
        {
            GameObject bullet = Instantiate(bul, transform.position, transform.rotation) as GameObject;
            curdown = cooldown;
        }
        if(gunCalibrated)
        {
            Time.timeScale = 1;
            //transform.localRotation = Quaternion.Euler(-x, y, z);
            transform.localRotation = Quaternion.Euler(x, -y, z);
            currentAngle = transform.localRotation;
           // Debug.Log("GunRotation:" + transform.localRotation);
            curdown -= 1 * Time.deltaTime;
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
        switch (decoded[0])
        {
            case "H":
                Debug.Log("FIRE");
                button = true;
                break;
            case "L":
                button = false;
                break;
        }
        //print(message);
            //Debug.Log(message);
        if(decoded[1] == "NotConfigured")
        {
            gunCalibrated = false;
            displayString.text = message;
            Debug.Log(message);
        }
        else
        {
            if(!gunCalibrated)
            {
                gunCalibrated = true;
                //handRot = player.GetComponent<CubemanController>().handsRot;
                initY = float.Parse(decoded[1]);
                initX = float.Parse(decoded[2]);
                initZ = float.Parse(decoded[3]);
            }
            displayString.text = "";
            y = initY - float.Parse(decoded[1]);
            x = initX - float.Parse(decoded[2]);
            z = initZ - float.Parse(decoded[3]);
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
