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
    public bool completed = false;
    // Use this for initialization
    void Start()
    {

        player = GameObject.Find("PlayerMover");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            
            if (active)
            {
                // Debug.Log("Active is true");

                //else
                //{
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
                else if (fight)
                {
                    if (transform.parent.gameObject.GetComponent<enemySpawner>().isRoomCleared())
                    {
                        fight = false;
                    }
                }
                else
                {
                    if (target != null)
                    {
                        //Debug.Log("trying to call");
                        player.GetComponent<playerMove>().moveToTarget(target);
                        //Debug.Log("called");
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
    }

    public bool isCompleted()
    {
        return completed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collided");
        if(other.gameObject.layer == 11)
        {
            if (!completed)
            {
                //if(playerTurned)
                //{
                //    turnPlayer = true;
                //    playerTurned = false;
                //}
                timeFaught = 0;
                active = true;
            }
        }
    }

    public void setTarget(GameObject _target)
    {
        target = _target;
    }
}
