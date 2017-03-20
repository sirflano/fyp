using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour {

    public GameObject batSpawnerZone;
    public GameObject standingShooterZone;
    public GameObject spinningShooterZone;

    public GameObject batSpawner;
    public GameObject standingShooter;
    public GameObject spinningShooter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void spawnEnemies(float difficultyPoints)
    {
        while(difficultyPoints > 0)
        {
            float choice = Random.Range(0, 4);
            if(choice < 1 && difficultyPoints >= 2)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                spawnPos = batSpawnerZone.transform.TransformPoint(spawnPos * 0.5f);
                Instantiate(batSpawner, spawnPos, transform.rotation);
                difficultyPoints -= 2;
            }
            else if(choice < 2)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                spawnPos = standingShooterZone.transform.TransformPoint(spawnPos * 0.5f);
                Instantiate(standingShooter, spawnPos, transform.rotation);
                difficultyPoints -= 1;
            }
            else if(choice < 3)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                spawnPos = spinningShooterZone.transform.TransformPoint(spawnPos * 0.5f);
                Instantiate(spinningShooter, spawnPos, transform.rotation);
                difficultyPoints -= 1;
            }
        }
    }
}
