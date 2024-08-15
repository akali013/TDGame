using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public int cost;
    public int damage;
    public float attackCooldown = 1;
    public float range = 10;
    public int upgradeCost;
    public bool isUpgraded = false;


    private GameObject rangeCircle;
    private Renderer rangeRend;
    private GameManager gameManagerScript;
    

    // Start is called before the first frame update
    void Start()
    {
        rangeCircle = transform.GetChild(0).gameObject;             // GetChild(0) is the first index of children.
        rangeRend = rangeCircle.GetComponent<Renderer>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // If this tower is not the player's currently chosen tower
        if (gameManagerScript.chosenTower != this.gameObject)
            rangeRend.enabled = false;  // Make the range invisible
    }

   
    // When the tower is clicked
    private void OnMouseDown()
    {
        gameManagerScript.chosenTower = this.gameObject;
        gameManagerScript.ShowTowerRange(rangeRend);
    }
}
