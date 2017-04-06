using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelBuilder : MonoBehaviour {

    public GameObject[] rooms;
    public GameObject[] corridors;

    public GameObject startRoom;

    public List<GameObject> curRooms;
    private bool spawnRoom;
    public List<GameObject> tempRooms;
    private Vector3 curOutDoor;
    private Vector3 tempOutDoor;
    private Quaternion curOutDoorRot;
    private Quaternion tempOutDoorRot;
    public float enemiesToSpawn = 3;


	// Use this for initialization
	void Start () {
        //Create the tutorial starting room
        initLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void initLevel()
    {
        //Initialise the room lists
        curRooms = new List<GameObject>();
        tempRooms = new List<GameObject>();

        //spawn the start room at 0,0,0 and add it to the array of current rooms
        curRooms.Add(Instantiate(startRoom, new Vector3(0, 0, 0), new Quaternion()));
        
        //set the outDoor position and rotation used for generating the position of the next room to be the out door of the current room
        curOutDoor = curRooms[0].GetComponent<roomController>().getOutDoorPos();
        curOutDoorRot = curRooms[0].GetComponent<roomController>().getOutDoorRot();

        //start the coroutine to control the level generation process
        StartCoroutine(trackRooms());

        //Initialize the player and move them to the first waypoint
        GameObject player = GameObject.Find("playerMover");
        player.transform.position = curRooms[0].GetComponent<roomController>().getFirstWatpoint().transform.position;
    }

    IEnumerator trackRooms()
    {
        while(true)
        {
            //If there are any rooms still in the temp array since last frame add them to the current room array and update the number of enemies to spawn in the next room
            if (!spawnRoom && tempRooms.Count >= 1)
            {
                for(int i = 0; i < tempRooms.Count; i++) {
                    curRooms.Add(tempRooms[i]);
                    enemiesToSpawn += 2;
                    tempRooms.Remove(tempRooms[i]);
                    curOutDoor = tempOutDoor;
                    curOutDoorRot = tempOutDoorRot;
                }
            }

            //check the total number of rooms completed, If three or more have been completed destroy the oldest room
            int completedRooms = 0;
            for (int i = 0; i < curRooms.Count; i++)
            {
                if(curRooms[i].GetComponent<roomController>().isCompleted())
                {
                    completedRooms += 1;
                }
            }
            if(completedRooms >= 3)
            {
                Object.Destroy(curRooms[0]);
                curRooms.Remove(curRooms[0]);
                spawnRoom = true;
            }

            //If there are less than four rooms in the current room array spawn a new room
            else if(curRooms.Count<=3)
            {
                spawnRoom = true;
            }
            if(spawnRoom)
            {
                //Randomly decide between spawning a room or a corridor
                if(Random.Range(0,4) > 3)
                {
                    placeCorridor();
                }
                else
                {
                    placeRoom();
                }
            }
            yield return null;
        }
    }

    //This function wipes the temp array, this is called by room objects if they collide with one another
    public void restartGen()
    {
        for(int i = 0; i < tempRooms.Count; i++)
        {
            Object.Destroy(tempRooms[i]);
        }
        tempRooms.Clear();
    }

    private void placeRoom()
    {
        //set the out door position and rotation to match the out door of the last room in the current room array
        tempOutDoor = curRooms[curRooms.Count - 1].GetComponent<roomController>().getOutDoorPos();
        tempOutDoorRot = curRooms[curRooms.Count - 1].GetComponent<roomController>().getOutDoorRot();

        //generate which room to spawn and get its in door position
        int roomToSpawn = Random.Range(0, rooms.Length);
        Vector3 posRelToInDoor = rooms[roomToSpawn].GetComponent<roomController>().getInDoorPos();

        //multiply the door position by the inverse quaternion of their rotations to turn them into straight line distances
        posRelToInDoor = Quaternion.Inverse(rooms[roomToSpawn].GetComponent<roomController>().getInDoorRot()) * posRelToInDoor;
        tempOutDoor = Quaternion.Inverse(tempOutDoorRot) * tempOutDoor;

        //Get the rotation between the current last room and the next room to spawn
        Quaternion nextRoomRot = curRooms[curRooms.Count - 1].GetComponent<roomController>().transform.rotation * tempOutDoorRot;

        //Generate the spawn position of the next room by multiplying the desired rotation between the last and next rooms by the sum of their distances. Add this to the current last room position
        Vector3 spawnPos = (nextRoomRot * (-posRelToInDoor + tempOutDoor)) + curRooms[curRooms.Count - 1].GetComponent<roomController>().getRoomPos();

        //Spawn the new room and connect its waypoints to the current waypoints. Generate enemies in the new room
        tempRooms.Add(Instantiate(rooms[roomToSpawn], spawnPos, nextRoomRot));
        curRooms[curRooms.Count - 1].GetComponent<roomController>().setNextWaypoint(tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getFirstWatpoint());
        tempRooms[tempRooms.Count - 1].GetComponent<enemySpawner>().spawnEnemies(enemiesToSpawn);

        //update the out door positions and flip the spawnRoom boolean to false as it will be flipped to true if it's conditions are met
        curOutDoor = tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getOutDoorPos();
        curOutDoorRot = tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getOutDoorRot();
        spawnRoom = false;
    }
    private void placeCorridor()
    {
        //set the out door position and rotation to match the out door of the last room in the current room array
        tempOutDoor = curRooms[curRooms.Count - 1].GetComponent<roomController>().getOutDoorPos();
        tempOutDoorRot = curRooms[curRooms.Count - 1].GetComponent<roomController>().getOutDoorRot();

        //generate which room to spawn and get its in door position
        int roomToSpawn = Random.Range(0, corridors.Length);
        Vector3 posRelToInDoor = corridors[roomToSpawn].GetComponent<roomController>().getInDoorPos();

        //multiply the door position by the inverse quaternion of their rotations to turn them into straight line distances
        posRelToInDoor = Quaternion.Inverse(corridors[roomToSpawn].GetComponent<roomController>().getInDoorRot()) * posRelToInDoor;
        tempOutDoor = Quaternion.Inverse(tempOutDoorRot) * tempOutDoor;

        //Get the rotation between the current last room and the next room to spawn
        Quaternion nextRoomRot = curRooms[curRooms.Count - 1].GetComponent<roomController>().transform.rotation * tempOutDoorRot;

        //Generate the spawn position of the next room by multiplying the desired rotation between the last and next rooms by the sum of their distances. Add this to the current last room position
        Vector3 spawnPos = (nextRoomRot * (-posRelToInDoor + tempOutDoor)) + curRooms[curRooms.Count - 1].GetComponent<roomController>().getRoomPos();

        //Spawn the new room and connect its waypoints to the current waypoints. Generate enemies in the new room
        tempRooms.Add(Instantiate(corridors[roomToSpawn], spawnPos, nextRoomRot));

        //Spawn the new room and connect its waypoints to the current waypoints.
        curRooms[curRooms.Count - 1].GetComponent<roomController>().setNextWaypoint(tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getFirstWatpoint());

        //update the out door positions and flip the spawnRoom boolean to false as it will be flipped to true if it's conditions are met
        curOutDoor = tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getOutDoorPos();
        curOutDoorRot = tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getOutDoorRot();
        spawnRoom = false;
    }
}
