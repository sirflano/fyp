using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtTarget : MonoBehaviour {

    private GameObject target;
	// Use this for initialization
	void Start () {
        //Initialize the target
        target = GameObject.FindGameObjectWithTag("enemyTarget");
	}
	
	// Update is called once per frame
	void Update () {
        //If the target is currently moving through this objects parent room, look at target
        if(target.transform.parent.gameObject.GetComponent<playerMove>().getCurrentRoom() == transform.root.gameObject)
        transform.LookAt(target.transform);
	}
}
