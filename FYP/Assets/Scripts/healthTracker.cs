using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class healthTracker : MonoBehaviour {
    public Image healthBarLeft;
    public Image healthBarRight;
    public float maxHealth;
    private float curHealth;
	// Use this for initialization
	void Start () {
        //Initialise current health
        curHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        //update both health bars to match the current health. I use two bars next to each other shrinking in opposite directions to get a healthbar which shrinks toward its centre
        healthBarLeft.fillAmount = curHealth / maxHealth;
        healthBarRight.fillAmount = curHealth / maxHealth;

        //If the player dies load the menu scene
        if(curHealth <= 0)
        {
            SceneManager.LoadScene("subsequentMenu");
            curHealth = maxHealth;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        //If an enemy object colldies with the player reduce their health by one
        if(other.gameObject.layer == 10)
        {
            curHealth -= 1;
        }
    }
}
