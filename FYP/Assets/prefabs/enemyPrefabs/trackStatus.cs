using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackStatus : MonoBehaviour {

    private bool isDesctoyed = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setIsDestroyed(bool status)
    {
        isDesctoyed = status;
    }

    public bool isDestroyed()
    {
        return isDesctoyed;
    }
}
