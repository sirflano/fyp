using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueprintGenerator : MonoBehaviour {

    public float minLength;
    public float maxLength;
    public float minWidth;
    public float maxWidth;
    public float floorHeight;
    public float settleCycles;
    public float minFilledPercent;
    public float minDoorDist;

    public GameObject roomType;

    

    // Use this for initialization
    void Start()
    {
        Debug.Log("Starting generation");
        blueprint.room toBeBuilt = new blueprint.room((int)Random.Range(minLength, maxLength), (int)Random.Range(minWidth, maxWidth), (int)floorHeight);
        Debug.Log("room Initialized");
        if (Random.Range(0, 2) < 1)
        {
            Debug.Log("cave chosen");
            toBeBuilt = generateCave(toBeBuilt);
        }
        else
        {
            Debug.Log("room chosen");
            toBeBuilt = generateRoom(toBeBuilt);
        }
        Debug.Log("Room generated");
        //GetComponent<roomBuilder>().build(toBeBuilt);
        GameObject room = Instantiate(roomType, transform.position, transform.rotation) as GameObject;
        room.GetComponent<roomBuilder>().build(toBeBuilt);
        Debug.Log("Room Built");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public blueprint.room generateRoom(blueprint.room toBeBuilt)
    {
        blueprint.tile[,] tiles = toBeBuilt.tiles;
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                tiles[i, j].tiletype = 1;
            }
        }
        Debug.Log("tiles laid");
        toBeBuilt = addDoors(toBeBuilt);
        Debug.Log("doors added");
        return toBeBuilt;
    }

    public blueprint.room generateCave(blueprint.room toBeBuilt)
    {
        blueprint.tile[,] tiles = new blueprint.tile[toBeBuilt.tiles.GetLength(0), toBeBuilt.tiles.GetLength(1)];
        tiles = setUpGrid(toBeBuilt.tiles);
        //System.Array.Copy(tiles, toBeBuilt.tiles, tiles.GetLength(0) * tiles.GetLength(1));
        //tile[,] tiles = toBeBuilt.tiles;
        //int totalFilledTiles = setUpGrid(tiles);

        int counter = 0;
        while (counter < settleCycles)
        {
            System.Array.Copy(toBeBuilt.tiles, tiles, toBeBuilt.tiles.GetLength(0) * toBeBuilt.tiles.GetLength(1));
            int totalFilledTiles = 0;
            for (int j = 0; j < tiles.GetLength(0); j++)
            {
                for (int k = 0; k < tiles.GetLength(1); k++)
                {
                    int surroundingFilledTiles = checkTiles(tiles, j, k);

                    if (surroundingFilledTiles <= 3 && tiles[j, k].tiletype != 0)
                    {
                        tiles[j, k].tiletype = 0;
                        //totalFilledTiles -= 1;
                    }
                    else if (surroundingFilledTiles > 4 && tiles[j, k].tiletype != 1)
                    {
                        tiles[j, k].tiletype = 1;
                        //totalFilledTiles += 1;
                    }

                    if (tiles[j, k].tiletype == 1 || tiles[j, k].tiletype == 2)
                    {
                        totalFilledTiles += 1;
                    }
                }
            }
            System.Array.Copy(tiles, toBeBuilt.tiles, tiles.GetLength(0) * tiles.GetLength(1));
            counter++;
            if (counter == settleCycles)
            {
                if (totalFilledTiles < ((tiles.GetLength(0) * tiles.GetLength(1)) / 100) * minFilledPercent)
                {
                    Debug.Log("Retrying Generation, Current Filled Tiles:" + totalFilledTiles + " out of a required:" + ((tiles.GetLength(0) * tiles.GetLength(1)) / 100) * (minFilledPercent));
                    toBeBuilt.tiles = setUpGrid(toBeBuilt.tiles);
                    counter = 0;
                }
                else
                {
                    Debug.Log("Total filled tiles:" + totalFilledTiles + " out of a possible:" + tiles.GetLength(0) * tiles.GetLength(1));
                    toBeBuilt = addDoors(toBeBuilt);
                    return toBeBuilt;
                }
            }
            else
            {
                totalFilledTiles = 0;
            }
        }
        return toBeBuilt;
    }

    private blueprint.tile[,] setUpGrid(blueprint.tile[,] tiles)
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                tiles[i, j].tiletype = 0;

                if (Random.Range(0, 100) <= 50.0f)
                {
                    tiles[i, j].tiletype = 1;
                }

            }
        }
        return tiles;
    }

    private int checkTiles(blueprint.tile[,] tiles, int j, int k)
    {
        int surroundingFilledTiles = 0;
        if (j - 1 >= 0 && tiles[j - 1, k].tiletype == 1)
        {
            surroundingFilledTiles += 1;
        }
        if (j + 1 < tiles.GetLength(0) && tiles[j + 1, k].tiletype == 1)
        {
            surroundingFilledTiles += 1;
        }
        if (j - 1 >= 0 && k - 1 >= 0 && tiles[j - 1, k - 1].tiletype == 1)
        {
            surroundingFilledTiles += 1;
        }
        if (j - 1 >= 0 && k + 1 < tiles.GetLength(1) && tiles[j - 1, k + 1].tiletype == 1)
        {
            surroundingFilledTiles += 1;
        }
        if (j + 1 < tiles.GetLength(0) && k - 1 >= 0 && tiles[j + 1, k - 1].tiletype == 1)
        {
            surroundingFilledTiles += 1;
        }
        if (j + 1 < tiles.GetLength(0) && k + 1 < tiles.GetLength(1) && tiles[j + 1, k + 1].tiletype == 1)
        {
            surroundingFilledTiles += 1;
        }
        if (k + 1 < tiles.GetLength(1) && tiles[j, k + 1].tiletype == 1)
        {
            surroundingFilledTiles += 1;
        }
        if (k - 1 >= 0 && tiles[j, k - 1].tiletype == 1)
        {
            surroundingFilledTiles += 1;
        }

        return surroundingFilledTiles;
    }
    private blueprint.room addTexture(blueprint.room toBeBuilt)
    {
        return toBeBuilt;
    }

    private blueprint.room addDoors(blueprint.room toBeBuilt)
    {
        Vector2 door1Path = new Vector2(Random.Range(0, toBeBuilt.tiles.GetLength(0)), (int)Random.Range(0, toBeBuilt.tiles.GetLength(1)));
        Vector2 door2Path = new Vector2(Random.Range(0, toBeBuilt.tiles.GetLength(0)), (int)Random.Range(0, toBeBuilt.tiles.GetLength(1)));
        bool doorInSet = false;
        bool doorOutSet = false;
        bool door1x = Random.Range(0, 2) >= 1;
        bool door2x = Random.Range(0, 2) >= 1;
        Debug.Log("Variables set");
        while (!doorInSet)
        {
            if (door1x)
            {
                Debug.Log("door in x:" + door1Path.y + " Out of a possible:" + toBeBuilt.tiles.GetLength(1));
                if (Random.Range(0, 2) <= 1)
                {
                    toBeBuilt.doorInSide = new Vector2(1, 0);
                    for (int i = 0; i < toBeBuilt.tiles.GetLength(0); i++)
                    {
                        if (toBeBuilt.tiles[i, (int)door1Path.y].tiletype == 2)
                        {
                            Debug.Log("hit door, breaking");
                            break;
                        }
                        if (toBeBuilt.tiles[i, (int)door1Path.y].tiletype == 1)
                        {
                            toBeBuilt.tiles[i, (int)door1Path.y].tiletype = 2;
                            toBeBuilt.doorIn = new Vector2(i, (int)door1Path.y);
                            doorInSet = true;
                            break;
                        }
                    }
                }
                else
                {
                    toBeBuilt.doorInSide = new Vector2(-1, 0);
                    for (int i = toBeBuilt.tiles.GetLength(0) - 1; i >= 0; i--)
                    {
                        if (toBeBuilt.tiles[i, (int)door1Path.y].tiletype == 2)
                        {
                            Debug.Log("hit door, breaking");
                            break;
                        }
                        if (toBeBuilt.tiles[i, (int)door1Path.y].tiletype == 1)
                        {
                            toBeBuilt.tiles[i, (int)door1Path.y].tiletype = 2;
                            toBeBuilt.doorIn = new Vector2(i, (int)door1Path.y);
                            doorInSet = true;
                            break;
                        }
                    }
                }

            }
            else
            {
                Debug.Log("door in y:" + door1Path.x + " out of a possible:" + toBeBuilt.tiles.GetLength(0));
                if (Random.Range(0, 2) <= 1)
                {
                    toBeBuilt.doorInSide = new Vector2(0, 1);
                    for (int i = 0; i < toBeBuilt.tiles.GetLength(1); i++)
                    {
                        if (toBeBuilt.tiles[(int)door1Path.x, i].tiletype == 2)
                        {
                            Debug.Log("hit door, breaking");
                            break;
                        }
                        if (toBeBuilt.tiles[(int)door1Path.x, i].tiletype == 1)
                        {
                            toBeBuilt.tiles[(int)door1Path.x, i].tiletype = 2;
                            toBeBuilt.doorIn = new Vector2((int)door1Path.x, i);
                            doorInSet = true;
                            break;
                        }
                    }
                }
                else
                {
                    toBeBuilt.doorInSide = new Vector2(0, -1);
                    for (int i = toBeBuilt.tiles.GetLength(1) - 1; i >= 0; i--)
                    {
                        if (toBeBuilt.tiles[(int)door1Path.x, i].tiletype == 2)
                        {
                            Debug.Log("hit door, breaking");
                            break;
                        }
                        if (toBeBuilt.tiles[(int)door1Path.x, i].tiletype == 1)
                        {
                            toBeBuilt.tiles[(int)door1Path.x, i].tiletype = 2;
                            toBeBuilt.doorIn = new Vector2((int)door1Path.x, i);
                            doorInSet = true;
                            break;
                        }
                    }
                }
            }
            door1Path = new Vector2(Random.Range(0, toBeBuilt.tiles.GetLength(0)), (int)Random.Range(0, toBeBuilt.tiles.GetLength(1)));
            door1x = Random.Range(0, 2) >= 1;
        }
        Debug.Log("Door1Set");
        while (!doorOutSet)
        {
            if (door2x)
            {
                Debug.Log("Door out y:" + door2Path.x);
                if (Random.Range(0, 2) <= 1)
                {
                    toBeBuilt.doorOutSide = new Vector2(0, 1);
                    for (int i = 0; i < toBeBuilt.tiles.GetLength(1); i++)
                    {
                        if (toBeBuilt.tiles[(int)door2Path.x, i].tiletype == 2)
                        {
                            Debug.Log("hit door, breaking");
                            break;
                        }
                        if (toBeBuilt.tiles[(int)door2Path.x, i].tiletype == 1)
                        {
                            toBeBuilt.tiles[(int)door2Path.x, i].tiletype = 2;
                            toBeBuilt.doorOut = new Vector2((int)door2Path.x, i);
                            doorOutSet = true;
                            break;
                        }
                    }
                }
                else
                {
                    toBeBuilt.doorOutSide = new Vector2(0, -1);
                    for (int i = toBeBuilt.tiles.GetLength(1) - 1; i >= 0; i--)
                    {
                        if (toBeBuilt.tiles[(int)door2Path.x, i].tiletype == 2)
                        {
                            Debug.Log("hit door, breaking");
                            break;
                        }
                        if (toBeBuilt.tiles[(int)door2Path.x, i].tiletype == 1)
                        {
                            toBeBuilt.tiles[(int)door2Path.x, i].tiletype = 2;
                            toBeBuilt.doorOut = new Vector2((int)door2Path.x, i);
                            doorOutSet = true;
                            break;
                        }
                    }
                }

            }
            else
            {
                Debug.Log("door out y:" + door2Path.y);
                if (Random.Range(0, 2) <= 1)
                {
                    toBeBuilt.doorOutSide = new Vector2(1, 0);
                    for (int i = 0; i < toBeBuilt.tiles.GetLength(0); i++)
                    {
                        if (toBeBuilt.tiles[i, (int)door2Path.y].tiletype == 2)
                        {
                            Debug.Log("hit door, breaking");
                            break;
                        }
                        if (toBeBuilt.tiles[i, (int)door2Path.y].tiletype == 1)
                        {
                            toBeBuilt.tiles[i, (int)door2Path.y].tiletype = 2;
                            toBeBuilt.doorOut = new Vector2(i, (int)door2Path.y);
                            doorOutSet = true;
                            break;
                        }

                    }
                }
                else
                {
                    toBeBuilt.doorOutSide = new Vector2(-1, 0);
                    for (int i = toBeBuilt.tiles.GetLength(0) - 1; i >= 0; i--)
                    {
                        if (toBeBuilt.tiles[i, (int)door2Path.y].tiletype == 2)
                        {
                            Debug.Log("hit door, breaking");
                            break;
                        }
                        if (toBeBuilt.tiles[i, (int)door2Path.y].tiletype == 1)
                        {
                            toBeBuilt.tiles[i, (int)door2Path.y].tiletype = 2;
                            toBeBuilt.doorOut = new Vector2(i, (int)door2Path.y);
                            doorOutSet = true;
                            break;
                        }

                    }
                }

            }
            door2Path = new Vector2(Random.Range(0, toBeBuilt.tiles.GetLength(0)), (int)Random.Range(0, toBeBuilt.tiles.GetLength(1)));
            door2x = Random.Range(0, 2) >= 1;
        }
        Debug.Log("Door1Set");
        float distance = Vector2.Distance(toBeBuilt.doorIn, toBeBuilt.doorOut);
        if (distance < Mathf.Min(minDoorDist, Mathf.Min(toBeBuilt.tiles.GetLength(0), toBeBuilt.tiles.GetLength(0))))
        {
            toBeBuilt.tiles[(int)toBeBuilt.doorIn.x, (int)toBeBuilt.doorIn.y].tiletype = 1;
            toBeBuilt.tiles[(int)toBeBuilt.doorOut.x, (int)toBeBuilt.doorOut.y].tiletype = 1;
            toBeBuilt.doorIn = toBeBuilt.doorInSide = toBeBuilt.doorOut = toBeBuilt.doorOutSide = new Vector2(0, 0);
            toBeBuilt.doorOut = new Vector2(0, 0);
            toBeBuilt.doorIn = new Vector2(0, 0);
            Debug.Log("doors too close:" + Vector2.Distance(door1Path, door2Path));
            toBeBuilt = addDoors(toBeBuilt);
        }

        return toBeBuilt;
    }
}
