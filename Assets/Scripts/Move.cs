using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Move : MonoBehaviour
{
    public float speed = 5;
    public int maxHealth = 10;  // Since this script is only applied to enemies
    public int health;
    

    private SpawnManager smScript;
    private GameManager gmScript;
    private Vector3 basePos;
    
    
    // Start is called before the first frame update
    void Start()
    {
        smScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        gmScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        health = maxHealth;
        basePos = GameObject.Find("Base").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToBase();
        CheckHealth();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If enemy reaches the base
        if (other.gameObject.CompareTag("Base"))
        {
            smScript.enemyCount--;
            // If this enemy is the last enemy, the wave can be ended.
            if (smScript.enemyCount <= 0)      
                smScript.canEndWave = true;

            Destroy(gameObject);
        }
    }

    // Enemies always move torwards the base.
    private void MoveToBase()
    {
        transform.position = Vector3.MoveTowards(transform.position, basePos, speed * Time.deltaTime);
        transform.LookAt(basePos, Vector3.back);
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            smScript.enemyCount--;
            // If this enemy is the last enemy, the wave can be ended.
            if (smScript.enemyCount <= 0)
                smScript.canEndWave = true;

            Destroy(gameObject);
        }
    }

    // Show this enemy's health when the mouse is over it.
    private void OnMouseOver()
    {
        gmScript.ShowEnemyHealth(this.gameObject);
    }

    // Hide this enemy's health when the mouse leaves it.
    private void OnMouseExit()
    {
        gmScript.enemyText.gameObject.SetActive(false);
    }
}
