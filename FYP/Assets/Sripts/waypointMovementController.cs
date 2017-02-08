using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypointMovementController : MonoBehaviour
{

    private GameObject player;
    private bool active = false;
    public GameObject target;
    public bool fight;
    public float fightTimer;
    public bool turnPlayer;
    private bool playerTurned = false;
    private float timeFaught = 0;
    // Use this for initialization
    void Start()
    {

        player = GameObject.Find("PlayerMover");
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if(turnPlayer || player.GetComponent<playerMove>().getTurning())
            {   
                if(!player.GetComponent<playerMove>().getTurning())
                {
                    player.GetComponent<playerMove>().turnToTarget(transform.rotation);
                }
                turnPlayer = false;
                playerTurned = true;
            }
            
            else
            {
                if (fight)
                {
                    if (timeFaught >= fightTimer)
                    {
                        timeFaught = 0;
                        fight = false;
                    }
                    else
                    {
                        timeFaught += Time.deltaTime;
                    }
                }
                else
                {
                    if(target != null)
                    {
                        player.GetComponent<playerMove>().moveToTarget(target);
                    }
                    else
                    {
                        player.GetComponent<playerMove>().endMovement();
                    }
                    active = false;
                }
            }
        }


    }

    private void OnTriggerEnter()
    {
        Debug.Log("Collided");
        if(playerTurned)
        {
            turnPlayer = true;
            playerTurned = false;
        }
        timeFaught = 0;
        active = true;
    }

}
