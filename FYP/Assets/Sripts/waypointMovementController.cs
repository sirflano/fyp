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
    private bool completed = false;
    // Use this for initialization
    void Start()
    {

        player = GameObject.Find("PlayerMover");
    }

    // Update is called once per frame
    void Update()
    {
        if (turnPlayer)//|| player.GetComponent<playerMove>().getTurning())
        {
            player.GetComponent<playerMove>().turnToTarget(transform.rotation);
            /*if (!player.GetComponent<playerMove>().getTurning())
            {
                player.GetComponent<playerMove>().turnToTarget(transform.rotation);
            }*/
            turnPlayer = false;
            playerTurned = true;
        }
        if (active)
        {
            
            
            //else
            //{
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
                    completed = true;
                }
           // }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collided");
        if(other.gameObject.layer == 11)
        {
            if (!completed)
            {
                if(playerTurned)
                {
                    turnPlayer = true;
                    playerTurned = false;
                }
                timeFaught = 0;
                active = true;
            }
        }
    }

}
