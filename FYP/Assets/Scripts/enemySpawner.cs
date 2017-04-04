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

    public List<GameObject> enemies;
    private bool cleared = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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

    public bool isRoomCleared()
    {
        return cleared;
    }

    public void spawnEnemies(float difficultyPoints)
    {
        while(difficultyPoints > 0)
        {
            float choice = Random.Range(0, 4);
            //Debug.Log(choice);
            if(choice < 1 && difficultyPoints >= 2)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                spawnPos = batSpawnerZone.transform.TransformPoint(spawnPos * 0.5f);
                if(checkSpawnPos(spawnPos))
                {
                    GameObject curEnemy = Instantiate(batSpawner, spawnPos, transform.rotation);
                    curEnemy.transform.parent = transform;
                    enemies.Add(curEnemy);
                    difficultyPoints -= 2;
                }
            }
            else if(choice < 2)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
                spawnPos = standingShooterZone.transform.TransformPoint(spawnPos * 0.5f);
                if(checkSpawnPos(spawnPos))
                {
                    GameObject curEnemy = Instantiate(standingShooter, spawnPos, transform.rotation);
                    curEnemy.transform.parent = transform;
                    enemies.Add(curEnemy);
                    difficultyPoints -= 1;
                }
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
                if(checkSpawnPos(spawnPos))
                {
                    GameObject curEnemy = Instantiate(spinningShooter, spawnPos, spawnRot);
                    curEnemy.transform.parent = transform;
                    enemies.Add(curEnemy);
                    difficultyPoints -= 1;
                }
            }
        }
    }

    public void addEnemy(GameObject _enemy)
    {
        enemies.Add(_enemy);
    }

    private bool checkSpawnPos(Vector3 spawnPos)
    {
        return true;
        //return Physics.OverlapSphere(spawnPos, 1).Length < 3;
    }
}
