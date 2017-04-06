using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using UnityEngine.UI;

public class gunController : MonoBehaviour {

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
    public bool gunCalibrated = false;
    private string calibarionString = "";

    public bool Connected = false;
    public string portName = "COM3";

    public volatile float pot;
    public volatile bool button;
    public Text displayString;
    public Image bullets;
    public Image bullets2;
    public GameObject player;
    public Quaternion currentAngle;

    private float maxBullets = 6;
    private float curBullets;

    float scale = 1.0f;

    public AudioClip lazer;
    private AudioSource source;
    public AudioClip reload;

	// Use this for initialization
	void Start () {
        //reset bullets and initialize the audio player
        source = gameObject.GetComponent<AudioSource>();
        curBullets = maxBullets;

        //This code was provided to me, It runs the serial monitor reader on a seperate thread
        LookForController();
        // create the thread
        runThread = true;
        Thread ThreadForController = new Thread(new ThreadStart(ThreadWorker));
        ThreadForController.Start();
    }
	
	// Update is called once per frame
	void Update () {
        //update UI elements and cooldown
        displayString.text = calibarionString;
        bullets.fillAmount = curBullets / maxBullets;
        bullets2.fillAmount = curBullets / maxBullets;
        curdown = curdown - 1 * Time.deltaTime;

        //If the player is able to fire, spawn a bullet object, update the remaining bullets, reset cooldown, play a sound clip, and send a message to the Arduino to run the servo motor
        if (button && curdown <= 0)
        {
            if(curBullets > 0)
            {
                source.PlayOneShot(lazer, Random.Range(0.5f, 1));
                Instantiate(bul, transform.position, transform.rotation);
                curdown = cooldown;
                curBullets -= 1;
                controller.Write("1");
            }
            button = false;
        }

        //update the guns orientation if it's been calibrated, otherwise pause the game
        if(gunCalibrated)
        {
            Time.timeScale = 1;
            transform.localRotation = Quaternion.Euler(-x, -y, -z);
            currentAngle = transform.localRotation;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    //This function was provided for me, it takes the string read from the serial port, I wrote what it does with this string
    void ProcessMessage(string message)
    {
        //split the string and check if it is configured
        string[] decoded = message.Split(':');
        if (decoded[1] == "NotConfigured")
        {
            gunCalibrated = false;
            calibarionString = message;
        }
        else
        {
            //If the gun has been freshly calibrated set the rotation offset around the Y to match the current orientation
            if(!gunCalibrated)
            {
                gunCalibrated = true;
                initY = float.Parse(decoded[0]);
            }
            //blank the calibration string as the gun has been calibrated and update the orientation variables
            calibarionString = "";
            x = initX - float.Parse(decoded[2]);
            y = (initY - float.Parse(decoded[0]) - 180) % 360;
            z = initZ - float.Parse(decoded[1]);
            
            //If the string has a fourth element attempt to fire the gun
            if (decoded.Length > 3)
            {
                button = true;
            }
        }
    }

    //this function was provided for me. It reads in strings from the serial port and passes them to the processMessage function
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

    //this function was provided for me. It closes the thread when closing the program
    void OnApplicationQuit()
    {
        controller.Close();
        runThread = false;
    }

    //this function was provided for me. It checks each of the serial ports to find an arduino controller
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

    //If the gun collides with the reload zone, reload the gun
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
        {
            if(curBullets < maxBullets)
            {
                source.PlayOneShot(reload, 1);
                curBullets = maxBullets;
            }
        }
    }
}
