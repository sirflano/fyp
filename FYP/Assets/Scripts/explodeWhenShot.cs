using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeWhenShot : MonoBehaviour {
    public ParticleSystem ps;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //If the attached GameObject collides with the players bullets, destroy it and spawn a large particle system
        if(other.gameObject.layer == 9)
        {
            Instantiate(ps, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
