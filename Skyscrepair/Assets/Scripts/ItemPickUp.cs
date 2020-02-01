using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    //point at which to hold the item
    private GameObject player;
    public GameObject holdingPoint;
    public GameObject arm;
    //maximum distance from an item you must be to pick it up
    public float maxDist = 1f;

    private bool holdingItem = false;
    private GameObject heldItem;

    //time since item was dropped or picked up
    private float timeSinceLastInteraction;
    public float armSpeed;
    //current position vs target position
    private Vector3 armPosition = new Vector3(0, 0, 0);
    private Vector3 targetArmPosition = new Vector3(0, 0, 0);

    //defaults for holding or not holding an item
    private Vector3 armDefaultPosition = new Vector3(0, 0, 0);
    private Vector3 armHoldingPosition = new Vector3(0, 0, 90);

    public bool snapToArm;

    void Start()
    {
        player = GameObject.Find("Player");   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (holdingItem)
            {
                Drop();
            }
            else
            {
                PickUp();
            }
            
        }
        if(armPosition!= targetArmPosition)
        {
            LerpArm();
        }
    }

    void LerpArm()
    {
        //rotates the arm to the holding position or the default position based on itemHeld;
        arm.transform.eulerAngles = Vector3.Lerp(armPosition, targetArmPosition, (Time.time - timeSinceLastInteraction)*armSpeed);

        //once the rotation is finished stop
        if(arm.transform.localEulerAngles == targetArmPosition)
        {
            armPosition = targetArmPosition;
        }
    }

    void PickUp()
    {
        GameObject closestItem = null;
        float minDist = Mathf.Infinity;

        foreach(var obj in GameObject.FindGameObjectsWithTag("item"))
        {
            var distanceToObject = Vector2.Distance(obj.transform.position, player.transform.position);
            //if you're close enough to the item to pick it up, compare it against the item closest to you
            if (distanceToObject < maxDist)
            {
                if(distanceToObject < minDist)
                {
                    minDist = distanceToObject;
                    closestItem = obj;
                }
            }
        }
        //if there was an item in range pick up the closest one, otherwise do nothing
        if (closestItem != null)
        {
            PickUpItem(closestItem);
        }
    }

    void PickUpItem(GameObject item)
    {
        //set holdingItem true and update the held item
        holdingItem = true;
        heldItem = item;

        if (snapToArm)
        {
            item.transform.position = holdingPoint.transform.position;
        }
        //attach item to the holding point and disable the rigidbody
        item.transform.SetParent(holdingPoint.transform);
        item.GetComponent<Rigidbody2D>().simulated = false;

        //move the arm
        targetArmPosition = armHoldingPosition;
        timeSinceLastInteraction = Time.time;

    }

    void Drop()
    {
        //drop whichever item is currently being held;
        heldItem.transform.SetParent(null);
        heldItem.GetComponent<Rigidbody2D>().simulated = true;

        targetArmPosition = armDefaultPosition;
        timeSinceLastInteraction = Time.time;
  
        holdingItem = false;
        heldItem = null;
    }
    
}
