using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handSelectionController : MonoBehaviour {

    private GameObject gun;
    private GameObject cubeMan;
    public GameObject leftHand;
    public GameObject rightHand;

    private bool configured = false;
	// Use this for initialization
	void Start () {
        //initialise the gun, cubeman and configured varibles, start the hand selection process
        gun = GameObject.FindGameObjectWithTag("gun");
        cubeMan = GameObject.FindGameObjectWithTag("cubeMan");
        cubeMan.GetComponent<gunPlacer>().setHandSelected(false);
        configured = false;
    }
	
	// Update is called once per frame
	void Update () {
        //If the gun has been calibrated and the hand selection buttons have not been created create them
		if(gun.GetComponent<gunController>().gunCalibrated && !configured)
        {
            leftHand = Instantiate(leftHand, leftHand.transform.position, leftHand.transform.rotation);
            rightHand = Instantiate(rightHand, rightHand.transform.position, rightHand.transform.rotation);
            configured = true;
        }

        //If the player has selected their preferred hand destroy the hand selecton objects
        if(cubeMan.GetComponent<gunPlacer>().handSelected)
        {
            Object.Destroy(leftHand);
            Object.Destroy(rightHand);
        }
	}
}
