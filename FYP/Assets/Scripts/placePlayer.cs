using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placePlayer : MonoBehaviour {
    public GameObject waypoint;
	// Use this for initialization
	void Start () {
        
        GameObject player = GameObject.Find("PlayerMover");
        player.transform.position = waypoint.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
