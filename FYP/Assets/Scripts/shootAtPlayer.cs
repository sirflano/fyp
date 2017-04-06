using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootAtPlayer : MonoBehaviour {

    public GameObject bul;
    private GameObject target;
    //public float maxBats;
    public float cooldown;
    private float curdown;

    // Use this for initialization
    void Start()
    {
        //initialise the cooldown and target variables
        curdown = cooldown;
        target = GameObject.FindGameObjectWithTag("enemyTarget");
    }

    // Update is called once per frame
    void Update()
    {
        //If the cooldown has passed instatiate a bullet object and reset the cooldown
        if (target.transform.parent.gameObject.GetComponent<playerMove>().getCurrentRoom() == transform.root.gameObject && curdown <= 0)
        {
            curdown = cooldown;
            Instantiate(bul, transform.position, transform.rotation);
        }
        curdown -= Time.deltaTime;
    }
}