using UnityEngine;
using System.Collections;

public class roomBuilder : MonoBehaviour {

    public GameObject floorTile;
    public GameObject wallTile;
    public GameObject doorTile;

	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void build(blueprint.room toBeBuilt)
    {
        blueprint.tile[,] tiles = toBeBuilt.tiles;
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if(tiles[i,j].tiletype == 1)
                {
                    Vector3 floorPos = new Vector3(transform.position.x + i, toBeBuilt.floorHeight, transform.position.z + j);
                    Vector3 roofPos = new Vector3(transform.position.x + i, toBeBuilt.roofHeight, transform.position.z + j);
                    tiles[i, j].floorTile = Instantiate(floorTile, floorPos, transform.rotation) as GameObject;
                    tiles[i, j].floorTile.transform.parent = transform;
                    tiles[i, j].roofTile = Instantiate(floorTile, roofPos, transform.rotation) as GameObject;
                    tiles[i, j].roofTile.transform.parent = transform;

                    checkWalls(toBeBuilt, i, j);
                }
                else if(tiles[i,j].tiletype == 2)
                {
                    checkDoors(toBeBuilt, i, j);
                }
                
                
            }
        }
    }

    public void checkWalls(blueprint.room toBeBuilt, int i, int j)
    {
        if ((i - 1 >= 0 && toBeBuilt.tiles[i - 1, j].tiletype == 0) || i-1 < 0)
        {
            Vector3 wallPos = new Vector3(transform.position.x + i - 0.5f, toBeBuilt.roofHeight -1, transform.position.z + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.left) * Quaternion.AngleAxis(90, Vector3.forward);
            toBeBuilt.tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
            toBeBuilt.tiles[i, j].floorTile.transform.parent = transform;

        }
        if ((i + 1 < toBeBuilt.tiles.GetLength(0) && toBeBuilt.tiles[i + 1, j].tiletype == 0) || i + 1 >= toBeBuilt.tiles.GetLength(0))
        {
            Vector3 wallPos = new Vector3(transform.position.x + 0.5f + i, toBeBuilt.roofHeight -1, transform.position.z + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.forward);
            toBeBuilt.tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
            toBeBuilt.tiles[i, j].floorTile.transform.parent = transform;
        }
        
        if ((j - 1 >= 0 && toBeBuilt.tiles[i, j - 1].tiletype == 0) || j-1 < 0)
        {
            Vector3 wallPos = new Vector3(transform.position.x + i, toBeBuilt.roofHeight -1, transform.position.z + j - 0.5f);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
            toBeBuilt.tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
            toBeBuilt.tiles[i, j].floorTile.transform.parent = transform;
        }
        if ((j + 1 < toBeBuilt.tiles.GetLength(1) && toBeBuilt.tiles[i, j + 1].tiletype == 0) || j+1 >= toBeBuilt.tiles.GetLength(1))
        {
            Vector3 wallPos = new Vector3(transform.position.x + i, toBeBuilt.roofHeight -1, transform.position.z + 0.5f + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
            toBeBuilt.tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
            toBeBuilt.tiles[i, j].floorTile.transform.parent = transform;
        }
    }

    public void checkDoors(blueprint.room toBeBuilt, int i, int j)
    {
        blueprint.tile[,] tiles = toBeBuilt.tiles;
        if ((i - 1 < 0 || tiles[i - 1, j].tiletype == 0))
        {
            if((toBeBuilt.doorIn.x == i && toBeBuilt.doorIn.y == j && toBeBuilt.doorInSide.x == 1)|| (toBeBuilt.doorOut.x == i && toBeBuilt.doorOut.y == j && toBeBuilt.doorOutSide.x == 1))
            {
                Vector3 wallPos = new Vector3(transform.position.x +i - 0.5f, toBeBuilt.roofHeight -1, transform.position.z + j);
                Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.forward);
                Vector3 floorPos = new Vector3(transform.position.x + i, toBeBuilt.floorHeight, transform.position.z + j);
                Vector3 roofPos = new Vector3(transform.position.x + i, toBeBuilt.roofHeight, transform.position.z + j);
            
                tiles[i, j].doorTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
                tiles[i, j].doorTile.transform.parent = transform;
                tiles[i, j].floorTile = Instantiate(floorTile, floorPos, transform.rotation) as GameObject;
                tiles[i, j].floorTile.transform.parent = transform;
                tiles[i, j].roofTile = Instantiate(floorTile, roofPos, transform.rotation) as GameObject;
                tiles[i, j].roofTile.transform.parent = transform;
            }
            else
            {
                Debug.Log("Door In x: "+toBeBuilt.doorIn.x+" y:"+toBeBuilt.doorIn.y+" Door Out x:"+toBeBuilt.doorOut.x+" y:"+toBeBuilt.doorOut.y+" CurrentPos x:"+i+" y:"+j);
                Debug.Log("DoorInSide x:" + toBeBuilt.doorInSide.x + " y:" + toBeBuilt.doorInSide.y + " DoorOutSide x:" + toBeBuilt.doorOutSide.x + " y:" + toBeBuilt.doorOutSide.y);
                Vector3 wallPos = new Vector3(transform.position.x + i - 0.5f, toBeBuilt.roofHeight -1, transform.position.z + j);
                Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.left) * Quaternion.AngleAxis(90, Vector3.forward);
                tiles[i, j].wallTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
                tiles[i, j].wallTile.transform.parent = transform;
            }
        }
        if((i+1 >= tiles.GetLength(0) || tiles[i+1, j].tiletype == 0)) 
        {
            if ((toBeBuilt.doorIn.x == i && toBeBuilt.doorIn.y == j && toBeBuilt.doorInSide.x == -1) || (toBeBuilt.doorOut.x == i && toBeBuilt.doorOut.y == j && toBeBuilt.doorOutSide.x == -1))
            {
            Vector3 wallPos = new Vector3(transform.position.x + 0.5f + i, toBeBuilt.roofHeight -1, transform.position.z + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.forward);
            Vector3 floorPos = new Vector3(transform.position.x + i, toBeBuilt.floorHeight, transform.position.z + j);
            Vector3 roofPos = new Vector3(transform.position.x + i, toBeBuilt.roofHeight, transform.position.z + j);

            tiles[i, j].doorTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
                tiles[i, j].doorTile.transform.parent = transform;
                tiles[i, j].floorTile = Instantiate(floorTile, floorPos, transform.rotation) as GameObject;
                tiles[i, j].floorTile.transform.parent = transform;
                tiles[i, j].roofTile = Instantiate(floorTile, roofPos, transform.rotation) as GameObject;
                tiles[i, j].roofTile.transform.parent = transform;
            }
            else
            {
                Debug.Log("Door In x: " + toBeBuilt.doorIn.x + " y:" + toBeBuilt.doorIn.y + " Door Out x:" + toBeBuilt.doorOut.x + " y:" + toBeBuilt.doorOut.y + " CurrentPos x:" + i + " y:" + j);
                Debug.Log("DoorInSide x:" + toBeBuilt.doorInSide.x + " y:" + toBeBuilt.doorInSide.y + " DoorOutSide x:" + toBeBuilt.doorOutSide.x + " y:" + toBeBuilt.doorOutSide.y);
                Vector3 wallPos = new Vector3(transform.position.x + 0.5f + i, toBeBuilt.roofHeight-1, transform.position.z + j);
                Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.forward);
                tiles[i, j].wallTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
                tiles[i, j].wallTile.transform.parent = transform;
            }
        }
        if((j-1 < 0 || tiles[i,j-1].tiletype == 0))
        {
            if ((toBeBuilt.doorIn.x == i && toBeBuilt.doorIn.y == j && toBeBuilt.doorInSide.y == 1) || (toBeBuilt.doorOut.x == i && toBeBuilt.doorOut.y == j && toBeBuilt.doorOutSide.y == 1))
            {
            Vector3 wallPos = new Vector3(transform.position.x + i, toBeBuilt.roofHeight -1, transform.position.z + j - 0.5f);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
            Vector3 floorPos = new Vector3(transform.position.x + i, toBeBuilt.floorHeight, transform.position.z + j);
            Vector3 roofPos = new Vector3(transform.position.x + i, toBeBuilt.roofHeight, transform.position.z + j);

            tiles[i, j].doorTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
                tiles[i, j].doorTile.transform.parent = transform;
                tiles[i, j].floorTile = Instantiate(floorTile, floorPos, transform.rotation) as GameObject;
                tiles[i, j].floorTile.transform.parent = transform;
                tiles[i, j].roofTile = Instantiate(floorTile, roofPos, transform.rotation) as GameObject;
                tiles[i, j].roofTile.transform.parent = transform;

            }
            else
            {
                Debug.Log("Door In x: " + toBeBuilt.doorIn.x + " y:" + toBeBuilt.doorIn.y + " Door Out x:" + toBeBuilt.doorOut.x + " y:" + toBeBuilt.doorOut.y + " CurrentPos x:" + i + " y:" + j);
                Debug.Log("DoorInSide x:" + toBeBuilt.doorInSide.x + " y:" + toBeBuilt.doorInSide.y + " DoorOutSide x:" + toBeBuilt.doorOutSide.x + " y:" + toBeBuilt.doorOutSide.y);
                Vector3 wallPos = new Vector3(transform.position.x + i, toBeBuilt.roofHeight -1, transform.position.z + j - 0.5f);
                Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
                tiles[i, j].wallTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
                tiles[i, j].wallTile.transform.parent = transform;
            }
        }
        if((j+1 >= tiles.GetLength(1) || tiles[i,j+1].tiletype==0))
        {
            if ((toBeBuilt.doorIn.x == i && toBeBuilt.doorIn.y == j && toBeBuilt.doorInSide.y == -1) || (toBeBuilt.doorOut.x == i && toBeBuilt.doorOut.y == j && toBeBuilt.doorOutSide.y == -1))
            {
            Vector3 wallPos = new Vector3(transform.position.x + i, toBeBuilt.roofHeight -1, transform.position.z + 0.5f + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
            Vector3 floorPos = new Vector3(transform.position.x + i, toBeBuilt.floorHeight, transform.position.z + j);
            Vector3 roofPos = new Vector3(transform.position.x + i, toBeBuilt.roofHeight, transform.position.z + j);

            tiles[i, j].doorTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
                tiles[i, j].doorTile.transform.parent = transform;
                tiles[i, j].floorTile = Instantiate(floorTile, floorPos, transform.rotation) as GameObject;
                tiles[i, j].floorTile.transform.parent = transform;
                tiles[i, j].roofTile = Instantiate(floorTile, roofPos, transform.rotation) as GameObject;
                tiles[i, j].roofTile.transform.parent = transform;
            }
            else
            {
                Debug.Log("Door In x: " + toBeBuilt.doorIn.x + " y:" + toBeBuilt.doorIn.y + " Door Out x:" + toBeBuilt.doorOut.x + " y:" + toBeBuilt.doorOut.y + " CurrentPos x:" + i + " y:" + j);
                Debug.Log("DoorInSide x:" + toBeBuilt.doorInSide.x + " y:" + toBeBuilt.doorInSide.y + " DoorOutSide x:" + toBeBuilt.doorOutSide.x + " y:" + toBeBuilt.doorOutSide.y);
                Vector3 wallPos = new Vector3(transform.position.x + i, toBeBuilt.roofHeight -1, transform.position.z + 0.5f + j);
                Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
                tiles[i, j].wallTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
                tiles[i, j].wallTile.transform.parent = transform;
            }
        }
    }
}
