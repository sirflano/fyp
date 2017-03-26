using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addToRoomEnemies : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.parent.GetComponent<enemySpawner>().addEnemy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
