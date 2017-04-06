using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{

    public float speed = 1;
    public float turnSpeed = 1;
    private Vector3 movement;
    public GameObject target;
    private bool moving = false;
    private bool turning = false;
    private Quaternion targetRot;
    // Use this for initialization
    void Start()
    {
        //prevent the player object from being destroyed between scenes
        Object.DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //If the waypoint has set the player to turn, turn the player toward the target rotation
        if (turning)
        {
            if (Quaternion.Angle(transform.rotation, targetRot) >= 1.0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
            }
            else
            {
                //If the rotation is close enough to the target rotation set the rotation to match the target rotation and stop turning
                transform.rotation = targetRot;
                turning = false;
            }
        }
        
        //If the waypoint has set the player to move, move the player toward the target position
        else if (moving)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= (speed * Time.deltaTime))
            {
                //If the player is close enough to the target position set the position to the target position and stop moving
                transform.position += target.transform.position - transform.position;
                moving = false;
            }
            else
            {
                transform.position += movement * speed * Time.deltaTime;
            }


        }
    }

    //Return true if the player is current turning, used by the waypoint movement controller to allow it to wait until the player is done turning before calling moveToTarget
    public bool getTurning()
    {
        return turning;
    }

    //Used by the waypoint movement controller to turn the player
    public void turnToTarget(Quaternion _targetRot)
    {
        targetRot = _targetRot;
        turning = true;
    }

    //Used by the waypoint movement controller to move the player
    public void moveToTarget(GameObject _target)
    {
        movement = _target.transform.position - transform.position;
        movement = Vector3.Normalize(movement);
        target = _target;
        moving = true;
    }

    //This is used to stop the player
    public void endMovement()
    {
        turning = false;
        moving = false;
    }

    //Used by enemies to check the players current room
    public GameObject getCurrentRoom()
    {
        return target.transform.parent.gameObject;
    }

    //upon loading a new level reinitialise variables
    private void OnLevelWasLoaded(int level)
    {
        moving = false;
        transform.rotation = new Quaternion(0, 0, 0, 1);
        transform.position = new Vector3(4.5f, -23, 0);
    }
}