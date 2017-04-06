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
    private float timeFaught = 0;
    public bool completed = false;
    // Use this for initialization
    void Start()
    {
        //Initialise the player
        player = GameObject.Find("PlayerMover");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //Once activated send the player the commands specificed in the inspector window of this object
            if (active)
            {
                //turn the player before stopping them to fight before moving them
                if (turnPlayer)
                {
                    player.GetComponent<playerMove>().turnToTarget(transform.rotation);
                    turnPlayer = false;
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
                        player.GetComponent<playerMove>().moveToTarget(target);
                    }
                    else
                    {
                        player.GetComponent<playerMove>().endMovement();
                    }
                    active = false;
                    completed = true;
                }
            }

        }
    }

    public bool isCompleted()
    {
        return completed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //When the waypoint collides with the player activate it
        if(other.gameObject.layer == 11)
        {
            if (!completed)
            {
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
