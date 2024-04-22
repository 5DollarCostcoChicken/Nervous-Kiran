using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnButton : MonoBehaviour
{
    public GameObject spawnable;
    public int spawns;
    public TextMeshProUGUI textDisplay;
    public string text;
    // Start is called before the first frame update
    public void Start()
    {
        textDisplay.text = text + "\n(" + spawns + ")";
    }
    GameObject g;
    public void MouseDown()
    {
        if (spawns > 0)
        {
            g = Instantiate(spawnable);
            g.GetComponent<Neuron>().spawnButton = this;
            spawns--;
            textDisplay.text = text + "\n(" + spawns + ")";
        }
    }
    public void MouseUp()
    {
        g.SendMessage("MouseUp");
    }
}
