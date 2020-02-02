using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTilt : MonoBehaviour
{
    int level = 0;
    public GameObject[] players;
    public GameObject[] tiltObjects;
    int numberOfPlayers;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        numberOfPlayers = players.Length;
    }

    public void IncreaseLevel()
    {
        level++;
    }

    // Update is called once per frame
    void Update()
    {
        TiltByPlayer();
    }

    void TiltByPlayer()
    {
        float[] xPositions = new float[numberOfPlayers];
        float[] weights = new float[numberOfPlayers];
        float weightedAverage = 0;
        float totalWeight = 0;
        for(int i=0;i<numberOfPlayers;i++)
        {
            xPositions[i] = players[i].transform.position.x;
            weights[i] = players[i].GetComponent<PlayerController>().weight;
            totalWeight += weights[i];
        }
        for (int i = 0; i < numberOfPlayers; i++)
        {
            weightedAverage += xPositions[i] * weights[i]/totalWeight;
        }
        Debug.Log(weightedAverage);
        float tiltAngle = -weightedAverage;
        var targetPostiion = transform.eulerAngles;
        targetPostiion.z = tiltAngle;
        transform.eulerAngles = targetPostiion;
    }

    void TiltByObject()
    {
        var targetPostiion = tiltObjects[level].transform.eulerAngles;
        targetPostiion.y = 0;
        transform.eulerAngles = targetPostiion;
    }
}
