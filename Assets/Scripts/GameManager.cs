using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameActive = false;    // Determines if the game is playing or not
    public int waveNumber = 1;      // Wave 1 is the first wave

    public TextMeshProUGUI cashText;
    public TextMeshProUGUI towerText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI baseText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI enemyText;
    public CanvasRenderer towerPanel;
    public Button restartButton;
    public Button playButton;
    public Button upgradeButton;

    public GameObject chosenTower;      // The currently chosen tower of the player.

    private CashController cashScript;
    private PlayerController playerScript;
    private BaseScript baseScript;
    private int cash;
    private GameObject tower;

    // Start is called before the first frame update
    void Start()
    {
        cashScript = GameObject.Find("Player").GetComponent<CashController>();
        playerScript = GameObject.Find("Ground").GetComponent<PlayerController>();
        baseScript = GameObject.Find("Base").GetComponent<BaseScript>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCash();
        UpdateTowerText();
        UpdateWaveText();
        UpdateBaseText();
        if (chosenTower != null)
            UpdateTowerInfoText();
    }

    // Executes whenever the player clicks the upgrade button when inspecting a tower.
    public void Upgrade()
    {
        TowerController chosenTowerScript = chosenTower.GetComponent<TowerController>();
        if ((cashScript.cash >= chosenTowerScript.upgradeCost) && !chosenTowerScript.isUpgraded) {
            if (chosenTower.gameObject.CompareTag("Tower 1")) {
                chosenTowerScript.damage = 30;
                chosenTowerScript.attackCooldown = 0.2f;
                chosenTowerScript.range = 15;
                // Resize the chosen tower's range circle to its upgraded size.
                chosenTower.transform.GetChild(0).localScale = new Vector3(chosenTowerScript.range, 0.3f, chosenTowerScript.range);     
            }
            else if (chosenTower.gameObject.CompareTag("Tower 2")) {
                chosenTowerScript.damage = 100;
                chosenTowerScript.attackCooldown = 0.7f;
                chosenTowerScript.range = 20;
                chosenTower.transform.GetChild(0).localScale = new Vector3(chosenTowerScript.range, 2, chosenTowerScript.range);
            }
            cashScript.cash -= chosenTowerScript.upgradeCost;
            chosenTowerScript.isUpgraded = true;
        }
    }

    // Shows or hides the range of the chosen tower
    // depending on if it's already showing.
    public void ShowTowerRange(Renderer rangeRend) {
        if (rangeRend.enabled) {
            rangeRend.enabled = false;
            ShowTowerInfo(false);
            chosenTower = null;
        }
        else {
            rangeRend.enabled = true;
            ShowTowerInfo(true);
        }
    }

    // Enables the GUI components of the chosen tower.
    private void ShowTowerInfo(bool setting)
    {
        infoText.gameObject.SetActive(setting);
        towerPanel.gameObject.SetActive(setting);
        upgradeButton.gameObject.SetActive(setting);
    }
    
    private void UpdateCash()
    {
        cash = cashScript.cash;
        cashText.text = "$" + cash;
    }

    private void UpdateTowerText()
    {
        tower = cashScript.selectedTower;

        if (tower != null && playerScript.isTowerSelected)
            towerText.text = "Selected Tower: " + tower.name;
        else
            towerText.text = "Selected Tower: None";
        
    }

    private void UpdateWaveText()
    {
        waveText.text = "Wave " + waveNumber;
    }

    private void UpdateBaseText()
    {
        baseText.text = "Base: " + baseScript.baseHealth + " / " + baseScript.maxHealth;
    }

    private void UpdateTowerInfoText()
    {
        TowerController chosenTowerScript = chosenTower.GetComponent<TowerController>();
        infoText.text = chosenTower.tag + "\n\nDamage: " + chosenTowerScript.damage + "\n\nAttack Rate: " + chosenTowerScript.attackCooldown + "\n\nRange: " + chosenTowerScript.range;
        if (chosenTowerScript.isUpgraded)
            upgradeText.text = "Upgraded!";
        else
            upgradeText.text = "Upgrade:\n$" + chosenTowerScript.upgradeCost;
    }

    // Executes when the play button is clicked
    public void StartGame()
    {
        isGameActive = true;    // Game is currently playing
        playButton.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        ShowGameTexts(true);  // Shows the player's GUI
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true); 
        ShowGameTexts(false);   // Hides the player's GUI
        isGameActive = false;   // Game is currently not running
    }

    public void WinGame()
    {
        // Shows the "You Won!" text
        winText.gameObject.SetActive(true);
    }

    // Runs when the restart button is clicked
    public void RestartGame()
    {
        // Reloads the scene that is currently active in the game (which is just the game itself)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     
    }


    private void ShowGameTexts(bool setting)
    {
        cashText.gameObject.SetActive(setting);
        waveText.gameObject.SetActive(setting);
        towerText.gameObject.SetActive(setting);
        baseText.gameObject.SetActive(setting);
    }

    // Activate all GUI components related to the enemy's health.
    public void ShowEnemyHealth(GameObject enemy)
    {
        int maxHealth = enemy.GetComponent<Move>().maxHealth;
        int health = enemy.GetComponent<Move>().health;
        enemyText.gameObject.SetActive(true);
        enemyText.text = "Enemy: " + health + " / " + maxHealth;
    }
}
