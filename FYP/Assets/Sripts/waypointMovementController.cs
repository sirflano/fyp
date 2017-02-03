using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypointMovementController : MonoBehaviour {

    private GameObject player;
    private bool active = false;
    public GameObject target;
    public bool fight;
    public float fightTimer;
    public bool turnPlayer;
    private float timeFaught = 0;
	// Use this for initialization
	void Start () {

         player = GameObject.Find("PlayerMover");
}
	
	// Update is called once per frame
	void Update () {
        if(active)
        {
            if (fight)
            {
                if (timeFaught >= fightTimer)
                {
                    fight = false;
                }
                else
                {
                    timeFaught += Time.deltaTime;
                }
            }
            else
            {
                player.GetComponent<playerMove>().moveToTarget(target);
                active = false;
            }
        }
        
		
	}

    private void OnTriggerEnter()
    {
        Debug.Log("Collided");
        active = true;
    }

}
