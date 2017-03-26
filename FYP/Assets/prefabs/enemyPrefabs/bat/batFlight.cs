using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batFlight : MonoBehaviour {

    public GameObject target;
    public float speed;

    private GameObject curRoom;
	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("enemyTarget");
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target.transform);
        transform.position += transform.forward * speed * Time.deltaTime;

        if(target.transform.parent.gameObject.GetComponent<playerMove>().getCurrentRoom() != curRoom)
        {
            Destroy(gameObject);
        }
	}

    public void setCurRoom(GameObject _curRoom)
    {
        curRoom = _curRoom;
    }
}
