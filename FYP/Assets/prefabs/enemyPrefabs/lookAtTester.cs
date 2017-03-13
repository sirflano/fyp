using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtTester : MonoBehaviour {

    private GameObject target;
    private float maxDist = 100;
	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("enemyTarget");
	}
	
	// Update is called once per frame
	void Update () {
        if(Vector3.Distance(transform.position, target.transform.position) <= maxDist)
        transform.LookAt(target.transform);
	}
}
