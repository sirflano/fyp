using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barSpawner : MonoBehaviour {

    public GameObject bat;
    private GameObject target;
    //public float maxBats;
    public float cooldown;
    private float maxDist = 100;
    private float curdown;

	// Use this for initialization
	void Start () {
        curdown = cooldown;
        target = GameObject.FindGameObjectWithTag("enemyTarget");
	}
	
	// Update is called once per frame
	void Update () {
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("enemyTarget");
        }
        else
        {
		    if(curdown <= 0 && Vector3.Distance(transform.position, target.transform.position) <= maxDist)
            {
                curdown = cooldown;
                GameObject curBat = Instantiate(bat, transform.position, transform.rotation);
                curBat.GetComponent<batFlight>().target = target;
            
            }
            curdown -= Time.deltaTime;
        }
	}
}
