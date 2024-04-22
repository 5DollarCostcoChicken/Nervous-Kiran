using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Road : MonoBehaviour
{
    public bool invalid = false;
    public bool valid = false;
    public bool colliding = false;
    public Sprite sensoryImage;
    public bool connected = false;

    public GameObject neuron1;
    public GameObject neuron2;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Outline"))
        {
            colliding = true;
        }        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Outline"))
        {
            colliding = false;
        }
    }

    private void Update()
    {
        if (connected)
        {
            if (neuron1 == null || neuron2 == null)
                Destroy(gameObject);
        }
    }
}
