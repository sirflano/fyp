using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnShooter : MonoBehaviour {

    public GameObject shooter;
    public GameObject stalk;

    private GameObject instShooter;
    // Use this for initialization
    void Start () {
        instShooter = Instantiate(shooter, transform.position, transform.rotation);
        instShooter.transform.parent = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(instShooter == null)
        {
            stalk.GetComponent<trackStatus>().setIsDestroyed(true);
        }
	}
}
