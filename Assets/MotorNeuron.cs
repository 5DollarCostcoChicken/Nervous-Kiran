using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorNeuron : Neuron
{
    public GameObject timer;
    public float maxtimer;
    public float counters;
    public float MaxCounters;

    bool hungry = false;
    // Start is called before the first frame update
    void Start()
    {
        MaxCounters = counters;
        hungry = this.gameObject.activeInHierarchy;
        if (hungry)
        {
            timer = Instantiate(GlobalVariables.instance.timerPrefab, GlobalVariables.instance.timers.gameObject.transform);
            timer.GetComponent<Timer>().counter.text = counters.ToString();
            timer.SetActive(true);
            timer.GetComponent<Timer>().StartCoroutine(timer.GetComponent<Timer>().begin(transform.position));
        }
    }

    void Update()
    {
        if (!placed && gameObject.name.Contains("(Clone)"))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = -1f;

            transform.position = mousePosition;
            if (!colliding && zone)
            {
                GetComponent<SpriteRenderer>().color = Color.green;
            }
            else
                GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    public void MouseUp()
    {
        if (!placed)
        {
            if (!colliding && zone)
            {
                placed = true;
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                spawnButton.spawns++;
                spawnButton.textDisplay.text = spawnButton.text + "\n(" + spawnButton.spawns + ")";
                Destroy(this.gameObject);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Outline") || collision.gameObject.tag.Equals("Road"))
        {
            colliding = true;
        }
        if (collision.gameObject.tag.Equals("Motor"))
        {
            zone = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Outline") || collision.gameObject.tag.Equals("Road"))
        {
            colliding = false;
        }
        if (collision.gameObject.tag.Equals("Motor"))
        {
            zone = false;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (name.Contains("Clone"))
        {
            GameObject ui = Instantiate(UIx, GlobalVariables.instance.timers.gameObject.transform);
            ui.GetComponent<RelayUIx>().StartCoroutine(ui.GetComponent<RelayUIx>().begin(this.gameObject));
            GlobalVariables.instance.popSound.Play();
        }
    }

    public void activate()
    {
        if (connections.Count > 0 && counters > 0)
        {
            counters--;
            if (counters > 0)
            {
                timer.GetComponent<Timer>().counter.text = counters.ToString();
            }
            else
            {
                Destroy(timer);
                timer = null;
            }
            GlobalVariables.instance.scoreIncrease();
        }
    }
}
