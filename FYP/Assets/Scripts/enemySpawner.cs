using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour {

    //invisible boxes which define the bounds the enemies can spawn in
    public GameObject batSpawnerZone;
    public GameObject standingShooterZone;
    public GameObject spinningShooterZone;

    //enemy objects to spawn
    public GameObject batSpawner;
    public GameObject standingShooter;
    public GameObject spinningShooter;

    //a lift of enemies to track which ones have yet to be destroyed
    public List<GameObject> enemies;

    //A boolean to be used by 'fight' waypoints
    private bool cleared = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Certain enemy types are harmless but can spawn enemies the player can destroy, this checks if those enemies have been destroyed
        if (enemies.Count >= 1)
        {
            int destoyedEnemies = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null || enemies[i].GetComponent<trackStatus>().isDestroyed())
                {
                    destoyedEnemies += 1;
                }
            }
            if (destoyedEnemies == enemies.Count)
            {
                cleared = true;
            }
            else
            {
                cleared = false;
            }
        }
    }

    //used by 'fight' waypoints to check if the player has finished the eneimes in the room
    public bool isRoomCleared()
    {
        return cleared;
    }

    //takes a number of enemies to spawn and spawns enemies within their respective zones
    public void spawnEnemies(float difficultyPoints)
    {
        while(difficultyPoints > 0)
        {
            //randomly chose which enemy type to spawn
            float choice = Random.Range(0, 4);
            
            //create a 3d vector whoes components are random positons between -1 and 1, find this point in world space relative to the spawnZone
            //spawn enemy in this position, add it to the list of enemies and reduce it's cost from the difficultyPoints
            if(choice < 1 && difficultyPoints >= 2)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                spawnPos = batSpawnerZone.transform.TransformPoint(spawnPos * 0.5f);
                GameObject curEnemy = Instantiate(batSpawner, spawnPos, transform.rotation);
                curEnemy.transform.parent = transform;
                enemies.Add(curEnemy);
                difficultyPoints -= 2;
            }
            else if(choice < 2)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
                spawnPos = standingShooterZone.transform.TransformPoint(spawnPos * 0.5f);
                GameObject curEnemy = Instantiate(standingShooter, spawnPos, transform.rotation);
                curEnemy.transform.parent = transform;
                enemies.Add(curEnemy);
                difficultyPoints -= 1;
            }
            else if(choice < 3)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                Quaternion spawnRot = transform.rotation;
                if(spawnPos.y <= -0.5f)
                {
                    spawnRot = spawnRot * Quaternion.Euler(180, 0, 0);
                }
                spawnPos = spinningShooterZone.transform.TransformPoint(spawnPos * 0.5f);
                GameObject curEnemy = Instantiate(spinningShooter, spawnPos, spawnRot);
                curEnemy.transform.parent = transform;
                enemies.Add(curEnemy);
                difficultyPoints -= 1;
            }
        }
    }

    //Used by the addEnemyToRoom script to add enemies that were not spawned by this class to it's list
    public void addEnemy(GameObject _enemy)
    {
        enemies.Add(_enemy);
    }
}
