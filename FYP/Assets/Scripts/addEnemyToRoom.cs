﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addEnemyToRoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Add enemy with this script attached to parent rooms list of enemies
        transform.parent.GetComponent<enemySpawner>().addEnemy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
