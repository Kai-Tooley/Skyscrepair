using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Tooltip("This is used to see if the elevator is active")]
    public bool isActive = false;
    [Tooltip("This is pointing to the exit on the next level")]
    public Transform elevatorExit;

    [Tooltip("This is the array of objects that need to be put together")]
    public List<objectRepair> itemsToBeRepaired;

    public List<bool> areItemsRepaired;

    public float camStep = 7f;

    public GameObject elevatorObj;

    Light elevatorLight;
    GameObject cameraMain;


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
        elevatorLight = GetComponentInChildren<Light>();
        elevatorLight.color = Color.red;
        cameraMain = GameObject.FindGameObjectWithTag("MainCamera");
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

        if (isActive)
        {
            elevatorLight.color = Color.green;
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
                GameObject.Find("TiltController").GetComponent<TowerTilt>().IncreaseLevel();
            }

            elevatorObj.transform.Translate(Vector3.up * 8);
            //cameraMain.transform.Translate(Vector3.up * camStep);
        }
    }
}
