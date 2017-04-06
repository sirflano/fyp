using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAfterX : MonoBehaviour {

    public float time;
    private float curTime = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //count how long object has been alive, if it's been alive longer than the time defined in the inspector destroy the object, usually applied to particle systems etc.
        curTime += Time.deltaTime;
        if(curTime >= time)
        {
            Destroy(gameObject);
        }
	}
}
