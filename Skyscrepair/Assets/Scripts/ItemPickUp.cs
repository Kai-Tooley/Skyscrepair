using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickUp : MonoBehaviour
{
    //point at which to hold the item
    private GameObject player;
    public GameObject holdingPoint;
    public GameObject arm;
    //maximum distance from an item you must be to pick it up
    public float maxDist = 1f;

    //what if anything, is the player holding
    private bool holdingItem = false;
    private GameObject heldItem;
    
    //current position vs target position
    private Vector3 armPosition = new Vector3(0, 0, 0);
    private Vector3 targetArmPosition = new Vector3(0, 0, 0);

    //defaults for holding or not holding an item
    private Vector3 armDefaultPosition = new Vector3(0, 0, 0);
    private Vector3 armHoldingPosition = new Vector3(0, 0, 90);
    public float armSpeed;

    Coroutine armMovement;
    public bool snapToArm;

    void Start()
    {
        player = GameObject.Find("Player");   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ItemButtonAction();
        }
    }

    void ItemButtonAction()
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

    IEnumerator LerpArm()
    {
        float startTime = Time.time;
        Vector3 startPosition = arm.transform.eulerAngles;
        while(arm.transform.eulerAngles != targetArmPosition)
        {
            arm.transform.eulerAngles = Vector3.Lerp(startPosition, targetArmPosition, (Time.time - startTime) * armSpeed);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("End of lerparm");
    }

    void PickUp()
    {
        Debug.Log("Attempting to pick up item");
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
        else
        {
            Debug.Log("Closest item was " + minDist);
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
        if (armMovement != null)
        {
            StopCoroutine(armMovement);
        }
        armMovement = StartCoroutine(LerpArm());
        

    }

    void Drop()
    {
        //drop whichever item is currently being held;
        heldItem.transform.SetParent(null);
        heldItem.GetComponent<Rigidbody2D>().simulated = true;

        targetArmPosition = armDefaultPosition;
        if (armMovement != null)
        {
            StopCoroutine(armMovement);
        }
        armMovement = StartCoroutine(LerpArm());

        holdingItem = false;
        heldItem = null;
    }

    public void OnGrab(InputAction.CallbackContext context)
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
}
