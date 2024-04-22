using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayUIx : MonoBehaviour
{
    SpawnButton relaySpawner;
    GameObject node;
    public IEnumerator begin(GameObject objecto)
    {
        node = objecto;
        Vector3 pos = objecto.transform.position;
        transform.localScale = Vector3.zero;
        transform.position = Camera.main.WorldToScreenPoint(pos) + Vector3.up * 30f + Vector3.left * -30f;
        for (int i = 1; i <= 6; i++)
        {
            yield return new WaitForSecondsRealtime(.02f);
            transform.localScale = new Vector3((i / 7f) * 1f, (i / 7f) * 1f, (i / 7f) * 1f);
        }
        for (int i = 1; i <= 1; i++)
        {
            yield return new WaitForSecondsRealtime(.02f);
            transform.localScale = new Vector3(((7 - i) / 6f) * 1f, ((7 - i) / 6f) * 1f, ((7 - i) / 6f) * 1f);
        }
        gameObject.tag = "TempButton";
    }
    public void Kill()
    {
        relaySpawner = node.GetComponent<Neuron>().spawnButton;
        foreach (Neuron connection in node.GetComponent<Neuron>().connections)
        {
            connection.connections.Remove(node.GetComponent<Neuron>());
        }
        relaySpawner.spawns++;
        relaySpawner.Start();
        Destroy(GlobalVariables.instance.currentRoad);
        Destroy(node.gameObject);
        Destroy(gameObject);
    }
    private void Update()
    {
        if (node == null && name.Contains("Clone"))
        {
            Destroy(gameObject);
        }
    }
}
