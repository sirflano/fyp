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
        //Debug.Log("camera position x:" + Camera.main.transform.position.x + " y:" + Camera.main.transform.position.y+" z:"+Camera.main.transform.position.z);
        //Debug.Log("head position x:" + head.transform.position + " y:" + head.transform.position.y + " z:" + head.transform.position.z);
        transform.position = tempPos;
	}
}
