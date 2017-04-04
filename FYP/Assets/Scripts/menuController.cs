using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuController : MonoBehaviour {

    private GameObject player;
    private bool paused = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("cubeMan");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")) {
            paused = !paused;
        }
        if (Input.GetKeyDown("a")) {
            player.GetComponent<gunPlacer>().leftHanded = !player.GetComponent<gunPlacer>().leftHanded;

        }
        if (paused)
        {
            player.GetComponent<gunPlacer>().setSelectionPaused(true);
        }
        else
        {
            player.GetComponent<gunPlacer>().setSelectionPaused(false);
        }
    }
}
