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
    private float initX = 0;
    private float initY = 0;
    private float initZ = 0;
    public bool gunCalibrated = false;
    private string calibarionString = "";
    //private Quaternion handRot;

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
            //transform.localRotation = Quaternion.Euler(-x, -y, -z);
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
        //Debug.Log(message);
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
        if (decoded[1] == "NotConfigured")
        {
            gunCalibrated = false;
            calibarionString = message;
            //Debug.Log(message);
        }
        else
        {
            if(!gunCalibrated)
            {
                gunCalibrated = true;
                
                //handRot = player.GetComponent<CubemanController>().handsRot;
                //if(initX == 0 && initY == 0 && initZ == 0)
                //{
                    initY = float.Parse(decoded[0]);
                    //initX = float.Parse(decoded[2]);
                    //initZ = float.Parse(decoded[3]);
                //}
                //else
                //{
                //    initY = y;
                //    initX = x;
                //    initZ = z;
                //}
            }
            //Debug.Log(message);
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
            y = (initY - float.Parse(decoded[0]) - 180) % 360;
            z = initZ - float.Parse(decoded[1]);
            //x = initX - float.Parse(decoded[2]) % 360;
            /*if(initX - float.Parse(decoded[2]) < 0)
            {
                x = 360 - initX - float.Parse(decoded[2]);
            }
            else
            {
                x = initX - float.Parse(decoded[2]);
            }
            x = float.Parse(decoded[2]);
            y = float.Parse(decoded[0]);
            z = float.Parse(decoded[1]);*/
            //calibarionString = "    "+initY+"    "+y.ToString();
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
