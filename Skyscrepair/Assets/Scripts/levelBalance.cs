using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelBalance : MonoBehaviour
{
    public int level;
    public float decay_rate;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float x = 1 / (level * decay_rate);
        gameObject.transform.localScale = new Vector3(x,1,1);
    }
}
