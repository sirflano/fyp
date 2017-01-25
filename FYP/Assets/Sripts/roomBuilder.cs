using UnityEngine;
using System.Collections;

public class roomBuilder : MonoBehaviour {

    public GameObject floorTile;
    public GameObject wallTile;
    public GameObject doorTile;
    public float floorheight;
    public float roofHeight;
    public float minLength;
    public float maxLength;
    public float minWidth;
    public float maxWidth;
    public float settleCycles;
    public float minFilledPercent;
    public float minDoorDist;

    public struct room
    {
        public tile[,] tiles;
        public Vector2 doorIn;
        public Vector2 doorOut;
        public Vector2 doorInSide;
        public Vector2 doorOutSide;

        public room(int _length, int _width) 
        {
            tiles = new tile[_length, _width];
            doorIn = new Vector2(0, 0);
            doorOut = new Vector2(0, 0);
            doorInSide = new Vector2(0, 0);
            doorOutSide = new Vector2(0, 0);
        }
    }
    public struct tile
    {
        public int tiletype;
        public GameObject floorTile;
        public GameObject roofTile;
        public GameObject wallTile;
        public Texture wallTexture;
        public Texture floorTexture;
        public Texture roofTexture;
    }

	// Use this for initialization
	void Start () {
        //build((int)initWidth, (int)initLength);
        room toBeBuilt = new room((int)Random.Range(minLength, maxLength), (int)Random.Range(minWidth, maxWidth));
        toBeBuilt.tiles = generateCave(toBeBuilt);
        //toBeBuilt.tiles = generateRoom(toBeBuilt);
        build(toBeBuilt);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public tile[,] generateRoom(room toBeBuilt)
    {
        tile[,] tiles = toBeBuilt.tiles;
        for(int i = 0; i < tiles.GetLength(0); i++)
        {
            for(int j = 0; j < tiles.GetLength(1); j++)
            {
                tiles[i, j].tiletype = 1;
            }
        }
        addDoors(toBeBuilt);
        return tiles;
    }

    public tile[,] generateCave(room toBeBuilt)
    {
        tile[,] tiles = toBeBuilt.tiles;
        setUpGrid(tiles);
        int counter = 0;
        while(counter < settleCycles)
        {
        int totalFilledTiles = 0;
            for( int j = 0; j < tiles.GetLength(0); j++)
            {
                for(int k=0; k < tiles.GetLength(1); k++)
                {
                    int surroundingFilledTiles = 0;
                    
                    if(j-1>=0 && tiles[j-1,k].tiletype ==1)
                    {
                        surroundingFilledTiles += 1;
                    }
                    if (j + 1 < tiles.GetLength(0) && tiles[j + 1, k].tiletype == 1)
                    {
                        surroundingFilledTiles += 1;
                    }
                    if (j - 1 >= 0 && k - 1 >= 0 && tiles[j - 1, k-1].tiletype == 1)
                    {
                        surroundingFilledTiles += 1;
                    }
                    if (j - 1 >= 0 && k + 1 < tiles.GetLength(1) && tiles[j - 1, k+1].tiletype == 1)
                    {
                        surroundingFilledTiles += 1;
                    }
                    if (j + 1 < tiles.GetLength(0) && k-1 >= 0 && tiles[j + 1, k-1].tiletype == 1)
                    {
                        surroundingFilledTiles += 1;
                    }
                    if (j + 1 < tiles.GetLength(0) && k + 1 < tiles.GetLength(1) && tiles[j + 1, k+1].tiletype == 1)
                    {
                        surroundingFilledTiles += 1;
                    }
                    if (k + 1 < tiles.GetLength(1) && tiles[j, k+1].tiletype == 1)
                    {
                        surroundingFilledTiles += 1;
                    }
                    if (k - 1 >= 0 && tiles[j, k-1].tiletype == 1)
                    {
                        surroundingFilledTiles += 1;
                    }

                    if(surroundingFilledTiles <= 3)
                    {
                        tiles[j, k].tiletype = 0;
                    }
                    
                    else if (surroundingFilledTiles > 4)
                    {
                        tiles[j, k].tiletype = 1;
                    }

                    if (tiles[j, k].tiletype == 1)
                    {
                        totalFilledTiles += 1;
                    }
                    
                    /*if ((Random.Range(0, 100) > 90) && tiles[i, j].tiletype == 0 && ((i + 1 < tiles.GetLength(0) && tiles[i + 1, j].tiletype == 1) || (i - 1 >= 0 && tiles[i - 1, j].tiletype == 1) || (j - 1 >= 0 && tiles[i, j - 1].tiletype == 1) || (j + 1 < tiles.GetLength(1) && tiles[i, j + 1].tiletype == 1)))
                    {
                        tiles[i, j].tiletype = 2;
                    }*/
                }
                
            }
            if(counter == settleCycles-1)
            {
                //Debug.Log("trying to restart");
                if (totalFilledTiles < ((tiles.GetLength(0) * tiles.GetLength(1)) / 100) * minFilledPercent)
                {
                    Debug.Log("Retrying Generation");
                    setUpGrid(tiles);
                    totalFilledTiles = 0;
                    counter = 0;
                }
                else
                {
                    addDoors(toBeBuilt);
                    return tiles;
                }
            }
            else
            {
                totalFilledTiles = 0;
                counter++;
            }
            //counter++;
            //Debug.Log(counter);
        }
        
            return tiles;
    }

    private void setUpGrid(tile[,] tiles)
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
    }

    private void addTexture(tile[,] tiles)
    {

    }

    private void addDoors(room toBeBuilt)
    {
        
        Vector2 door1Path = new Vector2(Random.Range(0, toBeBuilt.tiles.GetLength(0)-1), (int)Random.Range(0, toBeBuilt.tiles.GetLength(1)-1));
        Vector2 door2Path = new Vector2(Random.Range(0, toBeBuilt.tiles.GetLength(0)-1), (int)Random.Range(0, toBeBuilt.tiles.GetLength(1)-1));
        bool doorInSet = false;
        bool doorOutSet = false;
        bool door1x = Random.Range(0, 1) > 0.5f;
        bool door2x = Random.Range(0, 1) > 0.5f;

        while (!doorInSet)
        {
            if (door1x)
            {
                if(Random.Range(0,1) > 0.5f)
                {
                    toBeBuilt.doorInSide = new Vector2(1, 0);
                    for (int i = 0; i < toBeBuilt.tiles.GetLength(0); i++)
                    {
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
                    for (int i = toBeBuilt.tiles.GetLength(0)-1; i >= 0; i--)
                    {
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
                if(Random.Range(0,1) > 0.5f)
                {
                    toBeBuilt.doorInSide = new Vector2(0, 1);
                    for (int i = 0; i < toBeBuilt.tiles.GetLength(1); i++)
                    {
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
                    for (int i = toBeBuilt.tiles.GetLength(1)-1; i >=0; i--)
                    {
                        toBeBuilt.doorInSide = new Vector2(0, -1);
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
        }
        while (!doorOutSet)
        {
            if (door2x)
            {
                if(Random.Range(0,1) > 0.5f)
                {
                    toBeBuilt.doorOutSide = new Vector2(1, 0);
                    for (int i = 0; i < toBeBuilt.tiles.GetLength(1); i++)
                    {
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
                    toBeBuilt.doorOutSide = new Vector2(-1, 0);
                    for (int i = toBeBuilt.tiles.GetLength(1)-1; i >=0; i--)
                    {
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
                if(Random.Range(0,1) > 0.5f)
                {
                    toBeBuilt.doorOutSide = new Vector2(0, 1);
                    for (int i = 0; i < toBeBuilt.tiles.GetLength(0); i++)
                    {

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
                    toBeBuilt.doorOutSide = new Vector2(0, -1);
                    for (int i = toBeBuilt.tiles.GetLength(1)-1; i >=0; i--)
                    {

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
        }
        if(Vector2.Distance(door1Path, door2Path) < minDoorDist) {
            toBeBuilt.tiles[(int)door1Path.x, (int)door1Path.y].tiletype = 1;
            toBeBuilt.tiles[(int)door2Path.x, (int)door2Path.y].tiletype = 1;
            door1Path = new Vector2(0, 0);
            door2Path = new Vector2(0, 0);
            addDoors(toBeBuilt);
        }
    }

    public void build(room toBeBuilt)
    {
        tile[,] tiles = toBeBuilt.tiles;
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if(tiles[i,j].tiletype == 1)
                {
                    Vector3 floorPos = new Vector3(transform.position.x + i, floorheight, transform.position.z + j);
                    Vector3 roofPos = new Vector3(transform.position.x + i, roofHeight, transform.position.z + j);
                    tiles[i, j].floorTile = Instantiate(floorTile, floorPos, transform.rotation) as GameObject;
                    tiles[i, j].roofTile = Instantiate(floorTile, roofPos, transform.rotation) as GameObject;

                    checkWalls(tiles, i, j);
                }
                else if(tiles[i,j].tiletype == 2)
                {
                    checkDoors(toBeBuilt, i, j);
                }
                
                
            }
        }
    }

    public void checkWalls(tile[,] tiles, int i, int j)
    {
       /* if(tiles[i,j].tiletype == 2 && (i-1<0 ||  tiles [i-1, j].tiletype == 0))
        {
            Vector3 wallPos = new Vector3(transform.position.x + i - 0.5f, roofHeight / 2, transform.position.z + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.left) * Quaternion.AngleAxis(90, Vector3.forward);
            tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
        }*/
        if ((i - 1 >= 0 && tiles[i - 1, j].tiletype == 0) || i-1 < 0)
        {
            Vector3 wallPos = new Vector3(transform.position.x + i - 0.5f, roofHeight / 2, transform.position.z + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.left) * Quaternion.AngleAxis(90, Vector3.forward);
            tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
        }
        if ((i + 1 < tiles.GetLength(0) && tiles[i + 1, j].tiletype == 0) || i + 1 >= tiles.GetLength(0))
        {
            Vector3 wallPos = new Vector3(transform.position.x + 0.5f + i, roofHeight / 2, transform.position.z + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.forward);
            tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
        }
        
        if ((j - 1 >= 0 && tiles[i, j - 1].tiletype == 0) || j-1 < 0)
        {
            Vector3 wallPos = new Vector3(transform.position.x + i, roofHeight / 2, transform.position.z + j - 0.5f);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
            tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
        }
        if ((j + 1 < tiles.GetLength(1) && tiles[i, j + 1].tiletype == 0) || j+1 >= tiles.GetLength(1))
        {
            Vector3 wallPos = new Vector3(transform.position.x + i, roofHeight / 2, transform.position.z + 0.5f + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
            tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
        }
    }

    public void checkDoors(room toBeBuilt, int i, int j)
    {
        tile[,] tiles = toBeBuilt.tiles;
        if ((i - 1 < 0 || tiles[i - 1, j].tiletype == 0))
        {
            if((toBeBuilt.doorIn.x == i && toBeBuilt.doorIn.y == j && toBeBuilt.doorInSide.x == -1)|| (toBeBuilt.doorOut.x == i && toBeBuilt.doorOut.y == j && toBeBuilt.doorOutSide.x == -1))
            {
                Vector3 wallPos = new Vector3(transform.position.x +i - 0.5f, roofHeight / 2, transform.position.z + j);
                Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.forward);
                Vector3 floorPos = new Vector3(transform.position.x + i, 0, transform.position.z + j);
                Vector3 roofPos = new Vector3(transform.position.x + i, roofHeight, transform.position.z + j);
            
                tiles[i, j].wallTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
                tiles[i, j].floorTile = Instantiate(floorTile, floorPos, transform.rotation) as GameObject;
                tiles[i, j].roofTile = Instantiate(floorTile, roofPos, transform.rotation) as GameObject;
            }
            else
            {
                Vector3 wallPos = new Vector3(transform.position.x + i - 0.5f, roofHeight / 2, transform.position.z + j);
                Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.left) * Quaternion.AngleAxis(90, Vector3.forward);
                tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
            }
        }
        if((i+1 >= tiles.GetLength(0) || tiles[i+1, j].tiletype == 0)) 
        {
            if ((toBeBuilt.doorIn.x == i && toBeBuilt.doorIn.y == j && toBeBuilt.doorInSide.x == 1) || (toBeBuilt.doorOut.x == i && toBeBuilt.doorOut.y == j && toBeBuilt.doorOutSide.x == 1))
            {
            Vector3 wallPos = new Vector3(transform.position.x + 0.5f + i, roofHeight / 2, transform.position.z + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.forward);
            Vector3 floorPos = new Vector3(transform.position.x + i, 0, transform.position.z + j);
            Vector3 roofPos = new Vector3(transform.position.x + i, roofHeight, transform.position.z + j);

            tiles[i, j].wallTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
            tiles[i, j].floorTile = Instantiate(floorTile, floorPos, transform.rotation) as GameObject;
            tiles[i, j].roofTile = Instantiate(floorTile, roofPos, transform.rotation) as GameObject;
            }
            else
            {
                Vector3 wallPos = new Vector3(transform.position.x + 0.5f + i, roofHeight / 2, transform.position.z + j);
                Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.forward);
                tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
            }
        }
        if((j-1 < 0 || tiles[i,j-1].tiletype == 0))
        {
            if ((toBeBuilt.doorIn.x == i && toBeBuilt.doorIn.y == j && toBeBuilt.doorInSide.y == -1) || (toBeBuilt.doorOut.x == i && toBeBuilt.doorOut.y == j && toBeBuilt.doorOutSide.y == -1))
            {
            Vector3 wallPos = new Vector3(transform.position.x + i, roofHeight / 2, transform.position.z + j - 0.5f);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
            Vector3 floorPos = new Vector3(transform.position.x + i, 0, transform.position.z + j);
            Vector3 roofPos = new Vector3(transform.position.x + i, roofHeight, transform.position.z + j);

            tiles[i, j].wallTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
            tiles[i, j].floorTile = Instantiate(floorTile, floorPos, transform.rotation) as GameObject;
            tiles[i, j].roofTile = Instantiate(floorTile, roofPos, transform.rotation) as GameObject;

            }
            else
            {
                Vector3 wallPos = new Vector3(transform.position.x + i, roofHeight / 2, transform.position.z + j - 0.5f);
                Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
                tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
            }
        }
        if((j+1 >= tiles.GetLength(1) || tiles[i,j+1].tiletype==0))
        {
            if ((toBeBuilt.doorIn.x == i && toBeBuilt.doorIn.y == j && toBeBuilt.doorInSide.y == 1) || (toBeBuilt.doorOut.x == i && toBeBuilt.doorOut.y == j && toBeBuilt.doorOutSide.y == 1))
            {
            Vector3 wallPos = new Vector3(transform.position.x + i, roofHeight / 2, transform.position.z + 0.5f + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
            Vector3 floorPos = new Vector3(transform.position.x + i, 0, transform.position.z + j);
            Vector3 roofPos = new Vector3(transform.position.x + i, roofHeight, transform.position.z + j);

            tiles[i, j].wallTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
            tiles[i, j].floorTile = Instantiate(floorTile, floorPos, transform.rotation) as GameObject;
            tiles[i, j].roofTile = Instantiate(floorTile, roofPos, transform.rotation) as GameObject;
            }
            else
            {
                Vector3 wallPos = new Vector3(transform.position.x + i, roofHeight / 2, transform.position.z + 0.5f + j);
                Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
                tiles[i, j].floorTile = Instantiate(wallTile, wallPos, wallRot) as GameObject;
            }
        }
    }
}
