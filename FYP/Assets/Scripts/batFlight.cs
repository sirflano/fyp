using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batFlight : MonoBehaviour {

    //Can assign the target object and speed in inspector
    public GameObject target;
    public float speed;

    private GameObject curRoom;
	// Use this for initialization
	void Start () {
        //set target to be the player
        target = GameObject.FindGameObjectWithTag("enemyTarget");
	}
	
	// Update is called once per frame
	void Update () {
        //look at and fly toward the player
        transform.LookAt(target.transform);
        transform.position += transform.forward * speed * Time.deltaTime;

        //If the player has moved past the room this object is from, destroy the object
        if(target.transform.parent.gameObject.GetComponent<playerMove>().getCurrentRoom() != curRoom)
        {
            Destroy(gameObject);
        }
	}

    public void setCurRoom(GameObject _curRoom)
    {
        curRoom = _curRoom;
    }
}
