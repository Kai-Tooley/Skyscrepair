using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool isActive = false;
    public Transform elevatorExit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
