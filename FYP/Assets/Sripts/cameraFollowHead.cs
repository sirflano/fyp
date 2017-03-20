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
        Vector3 tempPos = head.transform.position;
        transform.position = tempPos;
	}
}
