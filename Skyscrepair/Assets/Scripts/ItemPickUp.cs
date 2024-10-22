﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickUp : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string playerWalkEvent = "";
    FMOD.Studio.EventInstance playerWalk;

    [FMODUnity.EventRef]
    public string playerLiftEvent = "";

    [FMODUnity.EventRef]
    public string playerDropEvent = "";

    [FMODUnity.EventRef]
    public string itemPickUpEvent = "";
    FMOD.Studio.EventInstance itemPickUp;

    public int playerNumber;
    GameObject otherPlayer;
    GameObject[] players;

    public Animator animator;
    private ItemEffects effects;
    //point at which to hold the item
    private GameObject player;
    private GameObject holdingPoint;
    private  GameObject arm;
    //maximum distance from an item you must be to pick it up
    public float maxDist = 1f;

    //what if anything, is the player holding
    public bool pickingUpItem = false;
    public bool holdingItem = false;
    public GameObject heldItem;
    
    //current position vs target position
    private Vector3 armPosition = new Vector3(0, 0, 0);
    private Vector3 targetArmPosition = new Vector3(0, 0, 0);

    //defaults for holding or not holding an item
    private Vector3 armDefaultPosition = new Vector3(0, 0, 0);
    private Vector3 armHoldingPosition = new Vector3(0, 0, 90);
    private Vector3 armDroppingPosition = new Vector3(0, 0, 40);
    public float armSpeed;
    public bool droppingItem = false;

    //debugging variable
    //public float nearest;

    Coroutine armMovement;
    public bool snapToArm;

    void Start()
    {
        holdingItem = false;
        //effects = GameObject.Find("Effects").GetComponent<ItemEffects>();
        player = gameObject;
        arm = gameObject.transform.GetChild(0).gameObject;
        holdingPoint = arm.transform.GetChild(0).gameObject;
        arm.transform.localEulerAngles = armDefaultPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
           ItemButtonAction();
        }

        if (Mathf.Abs(Input.GetAxis("Horizontal"))>0.1f)
        {
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }

        //just outputs distance to closest item if you want
        //nearest = Mathf.Infinity;
        //foreach(var obj in GameObject.FindGameObjectsWithTag("item"))
        //{
        //    if (Vector2.Distance(obj.transform.position, gameObject.transform.position) < nearest)
        //    {
        //        nearest = Vector2.Distance(obj.transform.position, gameObject.transform.position);
        //    }
        //}
    }

    private void OnDestroy()
    {
        playerWalk.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        playerWalk.release();

        itemPickUp.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        itemPickUp.release();
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
        Vector3 startPosition = arm.transform.localEulerAngles;
        while(arm.transform.localEulerAngles != targetArmPosition)
        {
            Debug.Log("I'm trying to move my arm to the desired position... I am player: " + gameObject.name);
            //Debug.Log(Vector3.Lerp(startPosition, targetArmPosition, (Time.time - startTime) * armSpeed));
            arm.transform.localEulerAngles = Vector3.Lerp(startPosition, targetArmPosition, (Time.time - startTime) * armSpeed);
            yield return new WaitForEndOfFrame();
            //putting this after the waitforend of frame should hopefully stop as many items getting stuck
            if (arm.transform.localEulerAngles.z < armDroppingPosition.z)
            {
                //doesnt matter if this is in the moving arm upwards part because its an irrelevant bool in this case
                droppingItem = false;
            }
        }
        droppingItem = false;
        animator.SetFloat("dropping", 1f);
    }

    void PickUp()
    {
        GameObject closestItem = null;
        float minDist = Mathf.Infinity;

        GameObject closestToOtherPlayer = null;
        float minDistOtherPlayer = Mathf.Infinity;

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

            //if (otherPlayer != null)
            //{
            //    var distance2 = Vector2.Distance(obj.transform.position, otherPlayer.transform.position);
            //    if(distance2 < minDistOtherPlayer && distance2 < maxDist)
            //    {
            //        closestToOtherPlayer = obj;
            //        minDistOtherPlayer = distance2;
            //    }
            //}
        }
        //if there was an item in range pick up the closest one, otherwise do nothing
        if (closestItem != null)
        {
            //if(!(closestItem==closestToOtherPlayer && minDist > minDistOtherPlayer && otherPlayer.GetComponent<ItemPickUp>().)
            StartCoroutine(PickUpOverTime(closestItem));
            
        }
    }

    IEnumerator PickUpOverTime(GameObject item)
    {
        animator.SetBool("holding", true);
        FMODUnity.RuntimeManager.PlayOneShot(playerLiftEvent);
        pickingUpItem = true;
        yield return new WaitForSeconds(.5f);
        PickUpItem(item);
    }

    void PickUpItem(GameObject item)
    {
        //set holdingItem true and update the held item
        holdingItem = true;
        heldItem = item;
        heldItem.gameObject.tag = "heldItem"; //change the tag to make it inaccessible to the other player

        if (snapToArm)
        {
            item.transform.position = holdingPoint.transform.position;
        }
        //attach item to the holding point and disable the rigidbody
        try
        {
            item.transform.SetParent(holdingPoint.transform);
            item.GetComponent<Rigidbody2D>().simulated = false;
        }
        catch
        {
            //innapropriate bug fixes for the win :) 
        }
        //effects.ChangeColor(item.gameObject, new Color(0, 250, 250), 1);

        //move the arm
        targetArmPosition = armHoldingPosition;
        if (armMovement != null)
        {
            StopCoroutine(armMovement);
        }
        armMovement = StartCoroutine(LerpArm());

        pickingUpItem = false;
        
    }

    void Drop()
    {
        if (!droppingItem)
        {
            animator.SetBool("holding", false);
            StartCoroutine(DropItemTransition());
        }
        
    }

    IEnumerator DropItemTransition()
    {
        //use this bool to stop the player trying to 'double drop'
        droppingItem = true;
        animator.SetFloat("dropping", -1f);

        //play Audio player drog grunt & droped object clatter
        FMODUnity.RuntimeManager.PlayOneShot(playerDropEvent);
        try
        {
            itemPickUp = FMODUnity.RuntimeManager.CreateInstance(itemPickUpEvent);
            itemPickUp.setParameterValue("Material", (float)heldItem.GetComponent<objectRepair>().material);
            itemPickUp.start();
            itemPickUp.release();
        }
        catch
        {
            //booooooooooooooo
        }

        //start arm movement
        targetArmPosition = armDefaultPosition;
        if (armMovement != null)
        {
            StopCoroutine(armMovement);
        }
        armMovement = StartCoroutine(LerpArm());

        //waits for the arm to reach the position where you want to drop the item
        while (droppingItem)
        {
            yield return new WaitForEndOfFrame();
        }
        //drop whichever item is currently being held;
        heldItem.transform.SetParent(null);
        try
        {
            Rigidbody2D itemRB = heldItem.GetComponent<Rigidbody2D>();
            itemRB.simulated = true;
            itemRB.velocity = new Vector3(player.transform.eulerAngles.y == 180 ? -5 : 5, -20);
        }
        catch
        {
            Debug.Log("I somehow lost the rigidbody? rip? " + heldItem.name);
        }
       
        heldItem.gameObject.tag = "item";

        holdingItem = false;
        heldItem = null;

       
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (holdingItem)
        {
            Drop();
        }
        else if(!pickingUpItem)
        {
            PickUp();
        }
    }

    public void playFootstepAudio()
    {
        playerWalk = FMODUnity.RuntimeManager.CreateInstance(playerWalkEvent);
        playerWalk.setParameterValue("Surface", 1.5f);
        playerWalk.start();
        playerWalk.release();
    }
}
