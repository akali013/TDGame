using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemies;
    public int enemyCount = 1;
    public bool canEndWave = true;

    private Queue<GameObject> enemyQueue; // Holds the enemies to be spawned in a wave.
    private int enemyCooldown = 1;      // Enemies are separated by one second.
    private int waveIntermission = 5;     //Each wave is separated by five seconds.
    private GameObject enemySpawn;
    private int waveNumber;
    private GameManager gameManagerScript;
    private Vector3 spawnPos;


    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        waveNumber = gameManagerScript.waveNumber;
        enemySpawn = GameObject.Find("EnemySpawn");
        spawnPos = enemySpawn.gameObject.transform.position;
        enemyQueue = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the all the enemies in a wave are defeated,
        if (canEndWave && gameManagerScript.isGameActive)   
        {
            canEndWave = false;
            waveNumber++;
            gameManagerScript.waveNumber++;
            StartCoroutine(BeginWave());  // start the next wave
        }
        // enemyCount may be set to true in the Move script upon
        // the last enemy being destroyed.
    }



    IEnumerator BeginWave()
    {
        // All enemies are stored in an array called enemies.
        GameObject normalEnemy = enemies[0];
        GameObject fastEnemy = enemies[1];
        GameObject slowEnemy = enemies[2];
        GameObject bossEnemy = enemies[3];

        yield return new WaitForSeconds(waveIntermission);

        switch (waveNumber)
        {
            case 1:
                queueEnemies(normalEnemy, 2);
                enemyCount = 2;
                StartCoroutine(SpawnEnemies());
                break;

            case 2:
                queueEnemies(normalEnemy, 3);
                queueEnemies(fastEnemy, 1);
                enemyCount = 4;
                StartCoroutine(SpawnEnemies());
                break;

            case 3:
                queueEnemies(fastEnemy, 3);
                queueEnemies(slowEnemy, 5);
                enemyCount = 8;
                StartCoroutine(SpawnEnemies());
                break;

            case 4:
                queueEnemies(slowEnemy, 5);
                queueEnemies(normalEnemy, 6);
                enemyCount = 11;
                StartCoroutine(SpawnEnemies());
                break;

            case 5:
                queueEnemies(normalEnemy, 7);
                queueEnemies(slowEnemy, 2);
                queueEnemies(fastEnemy, 5);
                queueEnemies(bossEnemy, 1);
                enemyCount = 15;
                StartCoroutine(SpawnEnemies());
                break;

            default:
                // The player wins the game if they survive all five waves.
                gameManagerScript.WinGame();        
                break;
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (enemyQueue.TryPeek(out GameObject result))   // Checks if there are still enemies to be spawned
        {
            GameObject currentEnemy = enemyQueue.Dequeue();
            Instantiate(currentEnemy, spawnPos, currentEnemy.transform.rotation);
            yield return new WaitForSeconds(enemyCooldown);
        }
    }

 
    private void queueEnemies(GameObject enemy, int numberOfTimes)
    {
        for (int i = 0; i < numberOfTimes; i++)
            enemyQueue.Enqueue(enemy);
    }



}
