using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectRepair : MonoBehaviour
{
    public List<GameObject> parts;
    public bool repaired = false;
    public Vector2 pos;
    public float rotation;
    public int order;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (parts.Count == 0)
        {
            repaired = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<objectRepair>())
            return;
        //if (order > collision.gameObject.GetComponent<objectRepair>().order)
        //    return;

        foreach (var item in parts)
        {
            if (collision.gameObject == item)
            {
                FixParts(collision.gameObject);
            }
        }
    }

    public void FixParts(GameObject part)
    {
        //play any audio for fixing items
        GameObject parent;
        if (transform.parent == null && part.transform.parent == null)
        {
            parent = new GameObject();
            parent.transform.parent = null;
            parent.transform.position = transform.position;
            parent.transform.rotation = transform.rotation;
            parent.gameObject.AddComponent<Rigidbody2D>();
            parent.GetComponent<Rigidbody2D>().mass = 0;
            parent.gameObject.AddComponent<BoxCollider2D>();
            parent.tag = "item";
        }
        else if (transform.parent == null)
        {
            parent = part.transform.parent.gameObject;
        }
        else
        {
            parent = transform.parent.gameObject;
        }

        positionPart(parent);
        part.GetComponent<objectRepair>().positionPart(parent);

        part.GetComponent<objectRepair>().parts.Remove(gameObject);
        parts.Remove(part);
    }

    void positionPart(GameObject parent)
    {
        gameObject.transform.parent = parent.transform;
        gameObject.tag = "Untagged";
        if (gameObject.GetComponent<Rigidbody2D>())
        {
            parent.GetComponent<Rigidbody2D>().mass += gameObject.GetComponent<Rigidbody2D>().mass;
            Destroy(gameObject.GetComponent<Rigidbody2D>());
        }
        gameObject.transform.localPosition = pos;
        gameObject.transform.rotation = new Quaternion(0, 0, rotation, 0);
    }
}
