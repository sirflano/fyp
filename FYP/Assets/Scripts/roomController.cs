using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomController : MonoBehaviour {


    public GameObject inDoor;
    public GameObject outDoor;

    public GameObject[] waypoints;

    private GameObject levelBuilder;
    private int completedWaypoints;
    public bool completed;
    private Vector3 outDoorPos;
    private Quaternion outDoorRot;
    private Vector3 inDoorPos;
    private Quaternion inDoorRot;
    private Bounds bounds;
	// Use this for initialization
	void Start () {
        //Initialise the levelBuilder and door varibles
        levelBuilder = GameObject.FindGameObjectWithTag("levelBuilder");
        completed = false;
        outDoorPos = outDoor.transform.position;
        outDoorRot = outDoor.transform.rotation;
        inDoorPos = inDoor.transform.localPosition;
        inDoorRot = inDoor.transform.localRotation;

        //Combine the bounds of all children objects to get the bounds of the room as a whole
        bounds = new Bounds();
        Renderer[] childrenBounds = GetComponentsInChildren<Renderer>();
        for(int i = 0; i < childrenBounds.Length; i++)
        {
            bounds.Encapsulate(childrenBounds[i].bounds);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Check each of the waypoints in the room to see if they player has already passed through them, If they have passed through them all update the completed boolean
        completedWaypoints = 0;
        outDoorPos = outDoor.transform.position;
        outDoorRot = outDoor.transform.rotation;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i].GetComponent<waypointMovementController>().isCompleted())
            {
                completedWaypoints += 1;
            }
        }
        if(completedWaypoints >= waypoints.Length)
        {
            completed = true;
        }
	}

    //These are functions for accessing the varibles in the room controller, they are used by the level builder when generating levels
    public Vector3 getInDoorPos()
    {
        return inDoor.transform.localPosition;
    }

    public Quaternion getInDoorRot()
    {
        return inDoor.transform.localRotation;
    }

    public Vector3 getOutDoorPos()
    {
        return outDoor.transform.localPosition;
    }

    public Vector3 getRoomPos()
    {
        return transform.position;
    }
    public Quaternion getOutDoorRot()
    {
        return outDoor.transform.localRotation;
    }

    public Bounds getBounds()
    {
        return bounds;
    }

    public bool isCompleted()
    {
        return completed;
    }

    public void setNextWaypoint(GameObject target)
    {
        waypoints[waypoints.Length - 1].GetComponent<waypointMovementController>().setTarget(target);
    }

    public GameObject getFirstWatpoint()
    {
        return waypoints[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the room collides with another room clear the temp room array and restart the generation process
        if(other.gameObject.layer == 8)
        {
            levelBuilder.GetComponent<levelBuilder>().restartGen();
        }
    }
}
