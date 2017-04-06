using UnityEngine;
using System.Collections;

public class spawnObj : MonoBehaviour {

    public GameObject obj;

    private float timer = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //If the timer has passed 10 seconds spawn an object defined in the unity inspector
	    if(timer >= 10) {
            GameObject enemey = Instantiate(obj, this.transform.position, this.transform.rotation) as GameObject;
            timer = 0;
        }
        else
        {
            timer += 1 * Time.deltaTime;
        }
	}
}
