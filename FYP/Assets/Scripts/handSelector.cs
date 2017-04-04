using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handSelector : MonoBehaviour {

    public bool leftHanded;

    private GameObject gun;
	// Use this for initialization
	void Start () {
        gun = GameObject.FindGameObjectWithTag("cubeMan");
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(Time.timeScale);
        if (other.gameObject.layer == 11 && !gun.GetComponent<gunPlacer>().getSelectionPaused())
        {
            gun.GetComponent<gunPlacer>().leftHanded = leftHanded;
            gun.GetComponent<gunPlacer>().handSelected = true;
        }
    }
}
