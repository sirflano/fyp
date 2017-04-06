using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handSelector : MonoBehaviour {

    public bool leftHanded;

    private GameObject gun;
	// Use this for initialization
	void Start () {
        //Initialise the gun
        gun = GameObject.FindGameObjectWithTag("cubeMan");
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnTriggerEnter(Collider other)
    {
        //If this object collides with the players skeleton while selection is not paused set the preferred hand of the gun component to match the choice in the Unity Inspector
        if (other.gameObject.layer == 11 && !gun.GetComponent<gunPlacer>().getSelectionPaused())
        {
            gun.GetComponent<gunPlacer>().leftHanded = leftHanded;
            gun.GetComponent<gunPlacer>().handSelected = true;
        }
    }
}
