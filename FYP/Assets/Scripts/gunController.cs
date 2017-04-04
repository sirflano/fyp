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
        LookForController();
        source = gameObject.GetComponent<AudioSource>();

        // create the thread
        runThread = true;
        Thread ThreadForController = new Thread(new ThreadStart(ThreadWorker));
        ThreadForController.Start();
        curBullets = maxBullets;
    }
	
	// Update is called once per frame
	void Update () {
        displayString.text = calibarionString;
        bullets.fillAmount = curBullets / maxBullets;
        bullets2.fillAmount = curBullets / maxBullets;
        curdown = curdown - 1 * Time.deltaTime;
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
        else if(Input.GetKeyDown("space") && curdown <=0)
        {
            
            Instantiate(bul, transform.position, transform.rotation);
            curdown = cooldown;
        }
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
    void ProcessMessage(string message)
    {
        string[] decoded = message.Split(':');
        if (decoded[1] == "NotConfigured")
        {
            gunCalibrated = false;
            calibarionString = message;
        }
        else
        {
            if(!gunCalibrated)
            {
                gunCalibrated = true;
                initY = float.Parse(decoded[0]);
            }
            calibarionString = "";
            x = initX - float.Parse(decoded[2]);
            y = (initY - float.Parse(decoded[0]) - 180) % 360;
            z = initZ - float.Parse(decoded[1]);
            
            if (decoded.Length > 3)
            {
                button = true;
            }
        }
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
