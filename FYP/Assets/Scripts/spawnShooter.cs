using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnShooter : MonoBehaviour {

    public GameObject shooter;
    public GameObject stalk;

    private GameObject instShooter;
    // Use this for initialization
    void Start () {
        //create a spinner shooter object and set it's parent to be this object
        instShooter = Instantiate(shooter, transform.position, transform.rotation);
        instShooter.transform.parent = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
        //If the spinning shooter is destroy, update this objects status
		if(instShooter == null)
        {
            stalk.GetComponent<trackStatus>().setIsDestroyed(true);
        }
	}
}
