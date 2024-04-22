using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron : MonoBehaviour
{
    public List<Neuron> connections;
    public bool myelin = false;
    public bool placed;
    public GameObject UIx;
    protected bool zone = false;
    protected bool colliding = false;
    public SpawnButton spawnButton;
    // Start is called before the first frame update
    void Start()
    {
        connections = new List<Neuron>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        GlobalVariables.instance.beginRoadHold(this.gameObject);
    }
    private void OnMouseUp()
    {
        GlobalVariables.instance.popSound.Play();
        if (GlobalVariables.instance.currentNeuron != this.gameObject && GlobalVariables.instance.currentNeuron != null)
            GlobalVariables.instance.endRoadHold(GlobalVariables.instance.currentNeuron);
        else
        {
            StartCoroutine(killRoad());
        }
    }
    private void OnMouseEnter()
    {
        GlobalVariables.instance.currentNeuron = this.gameObject;
    }
    private void OnMouseExit()
    {
        GlobalVariables.instance.currentNeuron = null;
    }

    public IEnumerator killRoad()
    {
        yield return new WaitForSecondsRealtime(.03f);
        if (GlobalVariables.instance.neuron1 != null && GlobalVariables.instance.currentRoad != null)
        {
            Destroy(GlobalVariables.instance.currentRoad.gameObject);
            GlobalVariables.instance.currentRoad = null;
        }
    }
}
