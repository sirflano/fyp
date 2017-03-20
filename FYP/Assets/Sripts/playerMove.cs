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
        Object.DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (turning)
        {
            if (Quaternion.Angle(transform.rotation, targetRot) >= 1.0)
            {
                //Debug.Log("Turn Remaining:" + Quaternion.Angle(transform.rotation, targetRot));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = targetRot;
                turning = false;
            }
        }
        
        else if (moving)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= (speed * Time.deltaTime))
            {
                transform.position += target.transform.position - transform.position;
                moving = false;
            }
            else
            {
                //Debug.Log("Distance:" + Vector3.Distance(transform.position, target.transform.position));
                transform.position += movement * speed * Time.deltaTime;
            }


        }
    }

    public bool getTurning()
    {
        return turning;
    }

    public void turnToTarget(Quaternion _targetRot)
    {
        turning = true;
        targetRot = _targetRot;
    }

    public void moveToTarget(GameObject _target)
    {
        Debug.Log("moveToTargetCalled");
        movement = _target.transform.position - transform.position;
        movement = Vector3.Normalize(movement);
        target = _target;
        moving = true;
    }

    public void endMovement()
    {
        target = null;
        turning = false;
        moving = false;
    }

    private void OnLevelWasLoaded(int level)
    {
        //turning = false;
        moving = false;
        transform.rotation = new Quaternion(0, 0, 0, 1);
        transform.position = new Vector3(0, 0, 0);
    }
}