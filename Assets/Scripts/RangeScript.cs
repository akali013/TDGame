using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangeScript : MonoBehaviour
{
    public GameObject tower;
    public bool isInRange = false;
    public LinkedList<GameObject> enemiesInRange;     // Each enemy is associated with their distance from the base
    public GameObject leadingEnemy;

    private CashController cashScript;
    private TowerController towerScript;
    private Move enemyScript;
    private Animator towerAnim;
    
    private bool canAttack = true;      // Debounce for the attack function
    private Vector3 basePos;
    private float minimumDistance = Mathf.Infinity;


    // Start is called before the first frame update
    void Start()
    {
        towerScript = tower.GetComponent<TowerController>();
        enemiesInRange = new LinkedList<GameObject>();
        basePos = GameObject.Find("Base").transform.position;
        cashScript = GameObject.Find("Player").GetComponent<CashController>();
        minimumDistance = Mathf.Infinity;
        towerAnim = tower.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        FindFirstEnemy();   // Finds a new leading enemy

        if (leadingEnemy != null && canAttack)
        {
            StartCoroutine(Attack());
            towerAnim.SetBool("Attacking", true);   // Play the shooting animation
            // The tower will always face the enemy its attacking.
            tower.transform.LookAt(leadingEnemy.transform.position, Vector3.back);  
        }     
        else if (!isInRange)
            towerAnim.SetBool("Attacking", false);  // Stop the shooting animation
    }

    private void OnTriggerEnter(Collider other)
    {
        // If an enemy is within a tower's range, add it to the list of enemies within range.
        if (other.gameObject.CompareTag("Enemy") && !enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.AddLast(other.gameObject);  
        }
    }

    // When the enemy leaves the range
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);        // The enemy is no longer in range.
            leadingEnemy = null;
        }
    }
    
    // Awards the player cash based on how much damage each tower does.
    IEnumerator Attack()
    {
        canAttack = false;
        enemyScript.health -= towerScript.damage;   
        cashScript.cash += towerScript.damage;             
        yield return new WaitForSeconds(towerScript.attackCooldown);
        canAttack = true;
    }

    private void FindFirstEnemy()
    {
        // Iterate through the linked list of enemies (enemiesInRange).
        for (LinkedListNode<GameObject> enemyNode = enemiesInRange.First; enemyNode != null; enemyNode = enemyNode.Next)
        {
            GameObject currentEnemy = enemyNode.Value;
            if (currentEnemy == null)   // In case the enemy is destroyed mid-calculation.
            {
                enemiesInRange.Remove(currentEnemy);
                continue;
            }
            float distance = GetDistanceFromBase(currentEnemy);
            // The enemy with the least distance is the leading enemy.
            if (distance < minimumDistance)
            {
                leadingEnemy = currentEnemy;
                minimumDistance = distance;
            }
        }

        if (leadingEnemy != null)
            enemyScript = leadingEnemy.GetComponent<Move>();
        minimumDistance = Mathf.Infinity;
    }

    // Find the distance between two points in the x-y plane.
    private float GetDistanceFromBase(GameObject enemy)
    {
        return Mathf.Sqrt(Mathf.Pow((basePos.y - enemy.transform.position.y), 2) + Mathf.Pow((basePos.x - enemy.transform.position.x), 2));
    }
}
