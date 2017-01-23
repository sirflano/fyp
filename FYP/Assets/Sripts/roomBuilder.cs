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


    public struct tile
    {
        public int tiletype;
        public GameObject floorTile;
        public GameObject roofTile;
        public Texture wallTexture;
        public Texture floorTexture;
        public Texture roofTexture;
    }

	// Use this for initialization
	void Start () {
        //build((int)initWidth, (int)initLength);
        tile[,] tiles = generateCave((int)Random.Range(minLength, maxLength), (int)Random.Range(minWidth, maxWidth));
        //tile[,] tiles = generateRoom((int)Random.Range(minLength, maxLength), (int)Random.Range(minWidth, maxWidth));
        build(tiles);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public tile[,] generateRoom(int length, int width)
    {
        tile[,] tiles = new tile[length, width];
        for(int i = 0; i < length; i++)
        {
            for(int j = 0; j < width; j++)
            {
                tiles[i, j].tiletype = 1;
            }
        }
        return tiles;
    }

    public tile[,] generateCave(int length, int width)
    {
        tile[,] tiles = new tile[length, width];
        int totalFilledTiles = 0;
        setUpGrid(tiles);
        int counter = 0;
        while(counter <= settleCycles)
        {
                    int surroundingFilledTiles = 0;
            for( int j = 0; j < tiles.GetLength(0); j++)
            {
                for(int k=0; k < tiles.GetLength(1); k++)
                {
                    
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

                    if(surroundingFilledTiles <= 2)
                    {
                        tiles[j, k].tiletype = 0;
                    }
                    
                    if (surroundingFilledTiles >= 3 && Random.Range(0, 100) > 90 && surroundingFilledTiles <8)
                    {
                        tiles[j, k].tiletype = 0;
                    }
                    else if (surroundingFilledTiles > 5)
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
            if(counter == settleCycles)
            {
                if (totalFilledTiles < ((tiles.GetLength(0) * tiles.GetLength(1)) / 100) * minFilledPercent)
                {
                    Debug.Log("Retrying Generation");
                    setUpGrid(tiles);
                    totalFilledTiles = 0;
                    counter = 0;
                }
            }
            else
            {
                totalFilledTiles = 0;
                counter++;
            }
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

                if (Random.Range(0, 100) <= 75.0f)
                {
                    tiles[i, j].tiletype = 1;
                }

            }
        }
    }

    public void build(tile[,] tiles)
    {
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
                    checkDoors(tiles, i, j);
                }
                
                
            }
        }
    }

    public void checkWalls(tile[,] tiles, int i, int j)
    {
        
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

    public void checkDoors(tile[,] tiles, int i, int j)
    {
        if (i - 1 >= 0 && tiles[i - 1, j].tiletype == 2)
        {
            Vector3 wallPos = new Vector3(transform.position.x +i - 0.5f, roofHeight / 2, transform.position.z + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.forward);
            tiles[i, j].floorTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
        }
        if (i + 1 < tiles.GetLength(0) && tiles[i + 1, j].tiletype == 2)
        {
            Vector3 wallPos = new Vector3(transform.position.x + 0.5f + i, roofHeight / 2, transform.position.z + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.forward);
            tiles[i, j].floorTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
        }
        if (j - 1 >= 0 && tiles[i, j - 1].tiletype == 2)
        {
            Vector3 wallPos = new Vector3(transform.position.x + i, roofHeight / 2, transform.position.z +j - 0.5f);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
            tiles[i, j].floorTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
        }
        if (j + 1 < tiles.GetLength(1) && tiles[i, j + 1].tiletype == 2)
        {
            Vector3 wallPos = new Vector3(transform.position.x + i, roofHeight / 2, transform.position.z + 0.5f + j);
            Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
            tiles[i, j].floorTile = Instantiate(doorTile, wallPos, wallRot) as GameObject;
        }
    }
    /*public void build(int width, int length)
    {
        GameObject[,] floorTiles = new GameObject[width,length];
        GameObject[,] roofTiles = new GameObject[width, length];
        GameObject[,] wallTiles = new GameObject[width, length];
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < length; j++)
            {
                Vector3 floorPos = new Vector3(transform.position.x + i, floorheight, transform.position.z + j);
                Vector3 roofPos = new Vector3(transform.position.x + i, roofHeight, transform.position.z + j);
                floorTiles[i, j] = Instantiate(floorTile, floorPos, transform.rotation) as GameObject;
                roofTiles[i, j] = Instantiate(floorTile, roofPos, transform.rotation) as GameObject;
                if (i == 0)
                {
                    Vector3 wallPos = new Vector3(transform.position.x -0.5f, roofHeight/2, transform.position.z + j);
                    Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.left) * Quaternion.AngleAxis(90, Vector3.forward);
                    wallTiles[i,j] = Instantiate(wallTile, wallPos, wallRot) as GameObject;
                }
                if (i == width-1)
                {
                    Vector3 wallPos = new Vector3(transform.position.x + 0.5f + i, roofHeight / 2, transform.position.z + j);
                    Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(90, Vector3.forward);
                    wallTiles[i, j] = Instantiate(wallTile, wallPos, wallRot) as GameObject;
                }
                if (j == 0)
                {
                    Vector3 wallPos = new Vector3(transform.position.x +i, roofHeight / 2, transform.position.z - 0.5f);
                    Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up) ;
                    wallTiles[i, j] = Instantiate(wallTile, wallPos, wallRot) as GameObject;
                }
                if (j == length-1)
                {
                    Vector3 wallPos = new Vector3(transform.position.x + i, roofHeight / 2, transform.position.z + 0.5f + j);
                    Quaternion wallRot = Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(0, Vector3.up);
                    wallTiles[i, j] = Instantiate(wallTile, wallPos, wallRot) as GameObject;
                }
            }
        }
    }*/
}
