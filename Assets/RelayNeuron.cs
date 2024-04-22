using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayNeuron : Neuron
{
    public Queue<Neuron> motorNeuronsQueue = new Queue<Neuron>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!placed && gameObject.name.Contains("(Clone)"))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = -1f;

            transform.position = mousePosition;
            if (name.Contains("Spinal"))
            {
                if (!colliding && zone)
                {
                    GetComponent<SpriteRenderer>().color = Color.green;
                }
                else
                    GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                if (!colliding)
                {
                    GetComponent<SpriteRenderer>().color = Color.green;
                }
                else
                    GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
    public void MouseUp()
    {
        if (!placed)
        {
            if (name.Contains("Spinal"))
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
            else
            {
                if (!colliding)
                {
                    placed = true;
                    GetComponent<SpriteRenderer>().color = Color.black;
                }
                else
                {
                    spawnButton.spawns++;
                    spawnButton.textDisplay.text = spawnButton.text + "\n(" + spawnButton.spawns + ")";
                    Destroy(this.gameObject);
                }
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Outline") || collision.gameObject.tag.Equals("Road"))
        {
            colliding = true;
        }
        if (collision.gameObject.tag.Equals("Spinal"))
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
        if (collision.gameObject.tag.Equals("Spinal"))
        {
            zone = false;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (!name.Contains("Spinal"))
        {
            GameObject ui = Instantiate(UIx, GlobalVariables.instance.timers.gameObject.transform);
            ui.GetComponent<RelayUIx>().StartCoroutine(ui.GetComponent<RelayUIx>().begin(this.gameObject));
            GlobalVariables.instance.popSound.Play();
        }
    }

    public void activate()
    {
        MotorNeuron motor = motorNeuronsQueue.Dequeue().GetComponent<MotorNeuron>();

        GameObject pulse = Instantiate(GlobalVariables.instance.pulsePrefab, GlobalVariables.instance.transform);
        pulse.transform.position = this.transform.position;
        pulse.GetComponent<Pulse>().currentNeuron = this;
        pulse.GetComponent<Pulse>().endNeuronPos = motor;
        pulse.GetComponent<Pulse>().speed = 2;

        if (motor.counters > 1)
        {
            motorNeuronsQueue.Enqueue(motor);
        }
    }
}
