using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnShooter : MonoBehaviour {

    public GameObject shooter;
	// Use this for initialization
	void Start () {
        GameObject instShooter = Instantiate(shooter, transform.position, transform.rotation);
        instShooter.transform.parent = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
