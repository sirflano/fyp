using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batSpawner : MonoBehaviour {

    public GameObject bat;
    private GameObject target;
    public float cooldown;
    private float maxDist = 100;
    private float curdown;

	// Use this for initialization
	void Start () {
        //reset cooldown, target player
        curdown = cooldown;
        target = GameObject.FindGameObjectWithTag("enemyTarget");
	}
	
	// Update is called once per frame
	void Update () {
        //if the target has been lost retarget player
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("enemyTarget");
        }
        else
        {
            //If the cooldown has passed spawn a bat and have it target the player, then reset cooldown. else tick down the cooldown
		    if(target.transform.parent.gameObject.GetComponent<playerMove>().getCurrentRoom() == transform.root.gameObject && curdown <= 0)
            {
                curdown = cooldown;
                GameObject curBat = Instantiate(bat, transform.position, transform.rotation);
                curBat.GetComponent<batFlight>().setCurRoom(transform.root.gameObject);
                curBat.GetComponent<batFlight>().target = target;
            
            }
            curdown -= Time.deltaTime;
        }
	}
}
