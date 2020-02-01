using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeColor(GameObject obj, Color color, float colorChangeTime=0)
    {
        SpriteRenderer spr;
        try
        {
            spr = obj.GetComponent<SpriteRenderer>();
        }
        catch
        {
            Debug.Log("No Sprite Renderer");
            return;
        }
        StartCoroutine(ColorTransition(colorChangeTime, spr, color));
    }

    //transition the colour over the given time
    IEnumerator ColorTransition(float time,SpriteRenderer spr,Color color)
    {
        Color startColor = spr.color;
        var startTime = Time.time;
        while (Time.time - startTime < time)
        {
            spr.color = Color.Lerp(startColor, color, (Time.time-startTime)/time);
            yield return new WaitForEndOfFrame();
        }
        spr.color = color;
    }
}
