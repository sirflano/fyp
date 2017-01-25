using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowHead : MonoBehaviour {

    public GameObject head;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = head.transform.position;
	}
}
