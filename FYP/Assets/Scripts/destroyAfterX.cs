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
        curTime += Time.deltaTime;
        if(curTime >= time)
        {
            Destroy(gameObject);
        }
	}
}
