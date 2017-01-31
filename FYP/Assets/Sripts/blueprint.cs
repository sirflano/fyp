using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueprint : MonoBehaviour {

    public struct room
    {
        public tile[,] tiles;
        public Vector2 doorIn;
        public Vector2 doorOut;
        public Vector2 doorInSide;
        public Vector2 doorOutSide;
        public float floorHeight;
        public float roofHeight;

        public room(int _length, int _width, int _floorheight)
        {
            tiles = new tile[_length, _width];
            doorIn = new Vector2(0, 0);
            doorOut = new Vector2(0, 0);
            doorInSide = new Vector2(0, 0);
            doorOutSide = new Vector2(0, 0);
            floorHeight = _floorheight;
            roofHeight = _floorheight + 2;
        }
    }
    public struct tile
    {
        public int tiletype;
        public GameObject floorTile;
        public GameObject roofTile;
        public GameObject wallTile;
        public GameObject doorTile;
        public Texture wallTexture;
        public Texture floorTexture;
        public Texture roofTexture;
    }
}
