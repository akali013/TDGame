using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This script is in both the ground and range circles so that the player can place towers there.
public class PlayerController : MonoBehaviour
{
    public bool isTowerSelected = false;
    public int towerIndex;

    private CashController cashScript;
    private Vector3 mousePos;
    private Vector3 worldPosition;
    private GameManager gameManagerScript;
        
    // Start is called before the first frame update
    void Start()
    {
        cashScript = GameObject.Find("Player").GetComponent<CashController>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // To get the mouse's position in the scene view rather than its pixels
        mousePos = Input.mousePosition;
        mousePos.z = 10;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        SelectTower();
    }

    private void SelectTower ()
    {
        // Press 1 to select tower 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            towerIndex = 1;
            isTowerSelected = true;
        }

        // Press 2 to select tower 2
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            towerIndex = 2;
            isTowerSelected = true;
        }

        // Press Q to cancel tower selection
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isTowerSelected = false;
            towerIndex = 0;
        } 
         
        cashScript.selectedTower = cashScript.towers[towerIndex];
    }

    private void OnMouseDown()
    {
        if (isTowerSelected && gameManagerScript.isGameActive)
        {
            TowerController selectedTowerScript = cashScript.selectedTower.GetComponent<TowerController>();
            int selectedTowerCost = selectedTowerScript.cost;

            if (cashScript.cash >= selectedTowerCost)
            {
                cashScript.cash -= selectedTowerCost;
                // Spawn tower at mouse's position
                Instantiate(cashScript.selectedTower.gameObject, worldPosition, cashScript.selectedTower.transform.rotation); 
            }
            isTowerSelected = false; // Cancel tower selection if the player has insufficient cash.
        }
    }
}
