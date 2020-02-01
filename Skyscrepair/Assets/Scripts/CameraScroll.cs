using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScroll : MonoBehaviour
{
    public GameObject endGame;
    public GameObject deathEffect;
    public GameObject emoji;
    public float moveSpeed;
    private GameObject player;
    private GameObject cam;
    private float camHeight;
    private float screenBottomY;
    public float cameraShakeOnDeath;
    public float tiltcameraShake;
    private ItemEffects effects;

    private bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        //set height of camera
        float yPosition = PlayerPrefs.GetFloat("yCameraPosition", 5);


        effects = GameObject.Find("Main Camera").GetComponent<ItemEffects>();
        cam = gameObject;

        cam.transform.position = new Vector3(cam.transform.position.x, yPosition, cam.transform.position.z);

        player = GameObject.Find("Player");
        camHeight = Camera.main.orthographicSize;
        screenBottomY = cam.transform.position.y - camHeight;
        Debug.Log(screenBottomY);
    }

    public void UpdateHeight()
    {
        PlayerPrefs.SetFloat("yCameraPosition", cam.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            MoveCamera();
            CheckPlayer();
        }
        
    }

    void CameraShake(float amount)
    {
        cam.transform.position += Random.insideUnitSphere * Time.deltaTime * amount;
    }
    void MoveCamera()
    {   
        //slowly move the camera upwards 
        cam.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        screenBottomY = cam.transform.position.y - camHeight;
    }

    void CheckPlayer()
    {
        //check if the player has exited visible range
        if(player.transform.position.y < screenBottomY && !gameOver) {
            gameOver = true;
            //gameover
            StartCoroutine(Die());
            Debug.Log("GameOver STart");
        }
    }

    IEnumerator Die()
    {
        float startTime = Time.time;
        Instantiate(deathEffect, player.transform.position, Quaternion.identity);
        while (Time.time-startTime < 2f)
        {
            CameraShake(cameraShakeOnDeath);
            //cam.transform.Rotate(new Vector3(0, 0, Random.Range(0, 5)));
            yield return new WaitForEndOfFrame();
        }
        if(emoji!=null)effects.ChangeColor(emoji, new Color(255, 255, 255, 1), 1);
        endGame.SetActive(true);
        while (Time.time - startTime < 6f)
        {
            CameraShake(cameraShakeOnDeath);
            //cam.transform.Rotate(new Vector3(0, 0, Random.Range(0, 5)));
            yield return new WaitForEndOfFrame();
        }

        //ExplodeAll();
        //while (Time.time-startTime < 1f)
        //{
        //    cam.transform.Rotate(new Vector3(0, 0, Random.Range(15,45)));
        //    yield return new WaitForEndOfFrame();
        //}
        Debug.Log("Gameover end");
    }

    //void ExplodeAll()
    //{
    //    foreach (var obj in GameObject.FindObjectsOfType<MonoBehaviour>())
    //    {
    //        obj.transform.parent = null;
    //    }
    //    foreach (var obj in GameObject.FindObjectsOfType<MonoBehaviour>())
    //    {
    //        Debug.Log(obj.name);
    //        if (!obj.GetComponent<Rigidbody2D>())
    //        {
    //            obj.gameObject.AddComponent<Rigidbody2D>();
    //        }
    //        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
    //        //opposite position to player
    //        var direction = Vector3.Normalize(obj.transform.position - player.transform.position);
    //        rb.AddForce(direction * 100f);
    //    }
    //}
}
