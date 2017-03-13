using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class healthTracker : MonoBehaviour {
    public Image healthBar;
    public float maxHealth;
    private float curHealth;
	// Use this for initialization
	void Start () {
        curHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        healthBar.fillAmount = curHealth / maxHealth;

        if(curHealth <= 0)
        {
            SceneManager.LoadScene("badEnd");
            curHealth = maxHealth;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            curHealth -= 1;
        }
    }
}
