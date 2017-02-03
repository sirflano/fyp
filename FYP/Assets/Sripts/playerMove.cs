using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {

    public float speed = 1;
    private Vector3 movement;
    private GameObject target;
    private bool moving = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (moving)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= (speed * Time.deltaTime))
            {
                transform.position += target.transform.position - transform.position;

                moving = false;
            }
            else
            {
                Debug.Log("Distance:" + Vector3.Distance(transform.position, target.transform.position));
                transform.position += movement * speed * Time.deltaTime;
            }
        }
    }

    public void moveToTarget(GameObject _target)
    {
        Debug.Log("moveToTargetCalled");
        movement = _target.transform.position - transform.position;
        movement = Vector3.Normalize(movement);
        target = _target;
        moving = true;

    }
}
