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
        gun = GameObject.FindGameObjectWithTag("gun");
        cubeMan = GameObject.FindGameObjectWithTag("cubeMan");
        cubeMan.GetComponent<handsTest>().setHandSelected(false);
        configured = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(gun.GetComponent<shooty>().gunCalibrated && !configured)
        {
            leftHand = Instantiate(leftHand, leftHand.transform.position, leftHand.transform.rotation);
            rightHand = Instantiate(rightHand, rightHand.transform.position, rightHand.transform.rotation);
            configured = true;
        }

        if(cubeMan.GetComponent<handsTest>().handSelected)
        {
            Object.Destroy(leftHand);
            Object.Destroy(rightHand);
        }
	}

    private void OnLevelWasLoaded(int level)
    {
        
    }
}
