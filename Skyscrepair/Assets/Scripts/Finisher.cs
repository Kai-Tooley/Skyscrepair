using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finisher : MonoBehaviour
{
    public GameObject fireworks;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        fireworks.SetActive(true);
    }
}
