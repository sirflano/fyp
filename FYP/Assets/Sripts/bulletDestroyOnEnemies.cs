using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletDestroyOnEnemies : MonoBehaviour {

    public GameObject pop;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            Instantiate(pop, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
