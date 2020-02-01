using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool isActive = false;
    public Transform elevatorExit;
    public List<objectRepair> itemsToBeRepaired;

    public List<bool> areItemsRepaired;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        foreach (objectRepair item in itemsToBeRepaired)
        {
            areItemsRepaired.Add(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (objectRepair item in itemsToBeRepaired)
        {
            if (item.repaired)
            {
                areItemsRepaired[itemsToBeRepaired.IndexOf(item)] = true;
            }
            if (areItemsRepaired.Contains(!false))
            {
                isActive = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() && isActive)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject item in players)
            {
                item.transform.position = elevatorExit.position;
            }
        }
    }
}
