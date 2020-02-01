using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTilt : MonoBehaviour
{
    public GameObject tiltObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var targetPostiion = -tiltObject.transform.eulerAngles;
        targetPostiion.y = 180;
        transform.eulerAngles = targetPostiion;
    }
}
