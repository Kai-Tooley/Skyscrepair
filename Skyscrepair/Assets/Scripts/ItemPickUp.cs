using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickUp : MonoBehaviour
{
    public Animator animator;
    private ItemEffects effects;
    //point at which to hold the item
    private GameObject player;
    private GameObject holdingPoint;
    private  GameObject arm;
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
    private Vector3 armDroppingPosition = new Vector3(0, 0, 40);
    public float armSpeed;
    private bool droppingItem = false;

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
            Debug.Log(Vector3.Lerp(startPosition, targetArmPosition, (Time.time - startTime) * armSpeed));
            arm.transform.localEulerAngles = Vector3.Lerp(startPosition, targetArmPosition, (Time.time - startTime) * armSpeed);
            if(arm.transform.localEulerAngles.z < armDroppingPosition.z)
            {
                droppingItem = false;
            }
            yield return new WaitForEndOfFrame();
        }
        animator.SetFloat("dropping", 1f);
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
            StartCoroutine(PickUpOverTime(closestItem));
            
        }
    }

    IEnumerator PickUpOverTime(GameObject item)
    {
        animator.SetBool("holding", true);
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
        item.transform.SetParent(holdingPoint.transform);
        item.GetComponent<Rigidbody2D>().simulated = false;
        //effects.ChangeColor(item.gameObject, new Color(0, 250, 250), 1);

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
        if (!droppingItem)
        {
            StartCoroutine(DropItemTransition());
        }
        animator.SetBool("holding", false);
    }

    IEnumerator DropItemTransition()
    {
        //use this bool to stop the player trying to 'double drop'
        droppingItem = true;
        animator.SetFloat("dropping", -1f);

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
        Rigidbody2D itemRB = heldItem.GetComponent<Rigidbody2D>();
        itemRB.simulated = true;
        itemRB.velocity = new Vector3(player.transform.eulerAngles.y==180?-5:5, -20); 
       
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
        else
        {
            PickUp();
        }
    }
}
