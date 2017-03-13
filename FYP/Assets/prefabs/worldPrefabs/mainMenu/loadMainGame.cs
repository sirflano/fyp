using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadMainGame : MonoBehaviour {
    private GameObject gun;
	// Use this for initialization
	void Start () {
        gun = GameObject.FindGameObjectWithTag("gun");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        /*if(other.gameObject.layer == 9 && gun.GetComponent<shooty>().gunCalibrated)
        {
            //Application.LoadLevel("demo");
            SceneManager.LoadScene("demo");
        }*/
        SceneManager.LoadScene("gameplayTest");
    }
}
