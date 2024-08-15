using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashController : MonoBehaviour
{
    public int cash;    // This is the amount of cash the player will have.
    public GameObject[] towers;     //An array of all the available towers
    public GameObject selectedTower;

    // Start is called before the first frame update
    void Start()
    {
        cash = 200;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
