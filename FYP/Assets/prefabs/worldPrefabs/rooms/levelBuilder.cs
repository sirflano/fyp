using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelBuilder : MonoBehaviour {

    public GameObject[] rooms;
    public GameObject[] corridors;

    public GameObject startRoom;

    private List<GameObject> curRooms;

    private List<GameObject> tempRooms;


	// Use this for initialization
	void Start () {
        curRooms = new List<GameObject>();
        tempRooms = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void restartGen()
    {
        for(int i = 0; i < tempRooms.Count; i++)
        {
            Object.Destroy(tempRooms[i]);
        }
        tempRooms.Clear();
    }
}
