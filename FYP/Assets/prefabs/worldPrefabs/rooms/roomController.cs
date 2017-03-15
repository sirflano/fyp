using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomController : MonoBehaviour {


    public GameObject inDoor;
    public GameObject outDoor;

    public GameObject[] waypoints;

    private GameObject levelBuilder;
	// Use this for initialization
	void Start () {
        levelBuilder = GameObject.FindGameObjectWithTag("levelBuilder");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            levelBuilder.GetComponent<levelBuilder>().restartGen();
        }
    }
}
