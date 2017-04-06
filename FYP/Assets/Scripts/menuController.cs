using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuController : MonoBehaviour {

    private GameObject player;
    private bool paused = false;
	// Use this for initialization
	void Start () {
        //Initialise the player
        player = GameObject.FindGameObjectWithTag("cubeMan");
	}
	
	// Update is called once per frame
	void Update () {
        //If I press space pause the game, if I press 'a' flip the players preferred hand. This is used during the menu scene to make switching from one player to the next easier
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
