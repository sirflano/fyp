﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootAtPlayer : MonoBehaviour {

    public GameObject bul;
    private GameObject target;
    //public float maxBats;
    public float cooldown;
    private float maxDist = 100;
    private float curdown;

    // Use this for initialization
    void Start()
    {
        curdown = cooldown;
        target = GameObject.FindGameObjectWithTag("enemyTarget");
    }

    // Update is called once per frame
    void Update()
    {
        if (curdown <= 0 && Vector3.Distance(transform.position, target.transform.position) <= maxDist)
        {
            curdown = cooldown;
            Instantiate(bul, transform.position, transform.rotation);
        }
        curdown -= Time.deltaTime;
    }
}