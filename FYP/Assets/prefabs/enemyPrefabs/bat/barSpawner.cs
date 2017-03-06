using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barSpawner : MonoBehaviour {

    public GameObject bat;
    public GameObject target;
    //public float maxBats;
    public float cooldown;

    private float curdown;

	// Use this for initialization
	void Start () {
        curdown = cooldown;
	}
	
	// Update is called once per frame
	void Update () {
		if(curdown <= 0)
        {
            GameObject curBat = Instantiate(bat, transform.position, transform.rotation);
            curBat.GetComponent<batFlight>().target = target;
            curdown = cooldown;
        }
        curdown -= Time.deltaTime;
	}
}
