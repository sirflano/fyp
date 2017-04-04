using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtTarget : MonoBehaviour {

    private GameObject target;
    private float maxDist = 100;
	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("enemyTarget");
	}
	
	// Update is called once per frame
	void Update () {
        if(target.transform.parent.gameObject.GetComponent<playerMove>().getCurrentRoom() == transform.root.gameObject)
        transform.LookAt(target.transform);
	}
}
