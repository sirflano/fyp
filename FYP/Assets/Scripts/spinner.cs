﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinner : MonoBehaviour {

    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //rotate around the X axis at a rate defined in the inspector
        transform.Rotate(new Vector3(speed * Time.deltaTime, 0, 0));
	}
}
