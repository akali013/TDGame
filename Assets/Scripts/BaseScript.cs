using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    public int maxHealth = 200;
    public int baseHealth;
    private Move enemyScript;
    private GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        baseHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (baseHealth <= 0)
        {
            // End the game when the base's health is zero.
            gameManagerScript.GameOver();       
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Any enemies that reach the base will weaken the base by its health
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyScript = other.gameObject.GetComponent<Move>();
            int enemyHealth = enemyScript.health;
            baseHealth -= enemyHealth;
        }

    }
}
