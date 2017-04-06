using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeOnWalls : MonoBehaviour {

    public ParticleSystem pop;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //If the attached GameObject collides with a wall object destroy it and spawn a small particle system
        if(other.gameObject.layer == 8)
        {
            Instantiate(pop, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
