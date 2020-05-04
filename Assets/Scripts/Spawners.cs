using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : MonoBehaviour
{
    // variables
    [SerializeField] private int numberOfEnemiesToSpawn = 5;
    [SerializeField] private float timeBetweenSpawns = 5f;
    [SerializeField] private bool isOn = true;

    //cached references
    [SerializeField] private GameObject enemyPrefab;

    //cached variables
    private int numberOfEnemiesSpawned = 0;
    private bool playerInRange = false;
    private float timer = 0f;
    private float randomTimer;


    private void Start()
    {
        randomTimer = timeBetweenSpawns;
    }

    void Update()
    {
        if(timer >= randomTimer)
        {
            SpawnEnemyHandler();
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }

    }

    private void SpawnEnemies()
    {
        if(numberOfEnemiesSpawned <= numberOfEnemiesToSpawn)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity) as GameObject;
            numberOfEnemiesSpawned++;
            enemy.transform.parent = transform;
            RandomizeTimer();
        }
    }

    private void SpawnEnemyHandler()
    {
        if(!playerInRange && isOn)
        {
            SpawnEnemies();
        }
    }

    private void RandomizeTimer()
    {
        randomTimer = Random.Range(1f, timeBetweenSpawns);
    }

    public void EnemyIsKilled()
    {
        numberOfEnemiesSpawned--;
    }

    public void PlayerIsInRange(bool inRange)
    {
        playerInRange = inRange;
    }
}
