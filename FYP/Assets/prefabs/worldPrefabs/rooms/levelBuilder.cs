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
        initLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void clearLevel()
    {
        if(curRooms.Count >= 1)
        {
            for(int i = curRooms.Count -1; i >= 0; i--)
            {
                Object.Destroy(curRooms[i]);
            }
        }
        if (tempRooms.Count >= 1)
        {
            for (int i = tempRooms.Count - 1; i >= 0; i--)
            {
                Object.Destroy(tempRooms[i]);
            }
        }
    }

    private void initLevel()
    {
        curRooms = new List<GameObject>();
        tempRooms = new List<GameObject>();
        curRooms.Add(Instantiate(startRoom, new Vector3(0, 0, 0), new Quaternion()));
        Debug.Log(curRooms.Count);
        curOutDoor = curRooms[0].GetComponent<roomController>().getOutDoorPos();
        curOutDoorRot = curRooms[0].GetComponent<roomController>().getOutDoorRot();
        //Debug.Log("outDoorPos:" + curOutDoor + " outDoorRot:" + curOutDoorRot);
        StartCoroutine(trackRooms());
        GameObject player = GameObject.Find("playerMover");
        player.transform.position = curRooms[0].GetComponent<roomController>().getFirstWatpoint().transform.position;
    }

    IEnumerator trackRooms()
    {
        while(true)
        {
            int completedRooms = 0;
            
            if (!spawnRoom && tempRooms.Count >= 1)
            {
                for(int i = 0; i < tempRooms.Count; i++) {
                    curRooms.Add(tempRooms[i]);
                    //Object.Destroy(tempRooms[i]);
                    tempRooms.Remove(tempRooms[i]);
                    curOutDoor = tempOutDoor;
                    curOutDoorRot = tempOutDoorRot;
                }
            }
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
            else if(curRooms.Count<=3)
            {
                spawnRoom = true;
            }
            if(spawnRoom)
            {
                if(Random.Range(0,4) > 2)
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
        //yield return null;
    }


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
        tempOutDoor = curRooms[curRooms.Count - 1].GetComponent<roomController>().getOutDoorPos();
        tempOutDoorRot = curRooms[curRooms.Count - 1].GetComponent<roomController>().getOutDoorRot();
        int roomToSpawn = Random.Range(0, rooms.Length);
        Vector3 posRelToInDoor = rooms[roomToSpawn].GetComponent<roomController>().getInDoorPos();
        posRelToInDoor = Quaternion.Inverse(rooms[roomToSpawn].GetComponent<roomController>().getInDoorRot()) * posRelToInDoor;
        tempOutDoor = Quaternion.Inverse(tempOutDoorRot) * tempOutDoor;
        Quaternion nextRoomRot = curRooms[curRooms.Count - 1].GetComponent<roomController>().transform.rotation * tempOutDoorRot;
        Vector3 spawnPos = (nextRoomRot * (-posRelToInDoor + tempOutDoor)) + curRooms[curRooms.Count - 1].GetComponent<roomController>().getRoomPos();
        tempRooms.Add(Instantiate(rooms[roomToSpawn], spawnPos, nextRoomRot));
        curRooms[curRooms.Count - 1].GetComponent<roomController>().setNextWaypoint(tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getFirstWatpoint());
        tempRooms[tempRooms.Count - 1].GetComponent<enemySpawner>().spawnEnemies(enemiesToSpawn);
        curOutDoor = tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getOutDoorPos();
        curOutDoorRot = tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getOutDoorRot();
        enemiesToSpawn += 2;
        spawnRoom = false;
    }
    private void placeCorridor()
    {
        tempOutDoor = curRooms[curRooms.Count - 1].GetComponent<roomController>().getOutDoorPos();
        tempOutDoorRot = curRooms[curRooms.Count - 1].GetComponent<roomController>().getOutDoorRot();
        int roomToSpawn = Random.Range(0, corridors.Length);
        Vector3 posRelToInDoor = corridors[roomToSpawn].GetComponent<roomController>().getInDoorPos();
        posRelToInDoor = Quaternion.Inverse(corridors[roomToSpawn].GetComponent<roomController>().getInDoorRot()) * posRelToInDoor;
        tempOutDoor = Quaternion.Inverse(tempOutDoorRot) * tempOutDoor;
        Quaternion nextRoomRot = curRooms[curRooms.Count - 1].GetComponent<roomController>().transform.rotation * tempOutDoorRot;
        Vector3 spawnPos = (nextRoomRot * (-posRelToInDoor + tempOutDoor)) + curRooms[curRooms.Count - 1].GetComponent<roomController>().getRoomPos();
        tempRooms.Add(Instantiate(corridors[roomToSpawn], spawnPos, nextRoomRot));
        curRooms[curRooms.Count - 1].GetComponent<roomController>().setNextWaypoint(tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getFirstWatpoint());
        curOutDoor = tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getOutDoorPos();
        curOutDoorRot = tempRooms[tempRooms.Count - 1].GetComponent<roomController>().getOutDoorRot();
        spawnRoom = false;
    }
}
