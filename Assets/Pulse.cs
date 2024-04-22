using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public Neuron currentNeuron;
    public Vector3 targetPos;
    public Neuron endNeuronPos;
    public float speed = 0;

    // Update is called once per frame
    void Update()
    {
        if (name.Contains("Clone"))
        {
            if (targetPos.Equals(Vector3.zero))
            {
                //TODO when you add myelin sheathes between neurons, check if myelin is checked for both current and new neuron, then change speed or revert it
                Neuron temp = FindPath(currentNeuron, endNeuronPos);
                currentNeuron = temp;
                if (currentNeuron == null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    targetPos = currentNeuron.transform.position;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, targetPos) < .025f)
                {
                    targetPos = Vector3.zero;
                    if (currentNeuron == endNeuronPos)
                    {
                        endNeuronPos.gameObject.SendMessage("activate");
                        Destroy(gameObject);
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                }
            }
        }
    }

    public Neuron FindPath(Neuron startNeuron, Neuron targetNeuron)
    {
        Queue<Neuron> queue = new Queue<Neuron>();
        HashSet<Neuron> visited = new HashSet<Neuron>();

        Dictionary<Neuron, Neuron> parentMap = new Dictionary<Neuron, Neuron>();

        queue.Enqueue(startNeuron);
        visited.Add(startNeuron);

        while (queue.Count > 0)
        {
            Neuron currentNeuron = queue.Dequeue();

            if (currentNeuron == targetNeuron)
            {
                // Reconstruct and return the path
                List<Neuron> path = new List<Neuron>();
                while (currentNeuron != null)
                {
                    path.Add(currentNeuron);
                    currentNeuron = parentMap.ContainsKey(currentNeuron) ? parentMap[currentNeuron] : null;
                }
                path.Reverse(); // Reverse the path to get it from start to target
                return path[1]; // Return the next Neuron in the path after the start Neuron
            }

            foreach (Neuron neighbor in currentNeuron.connections)
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    parentMap[neighbor] = currentNeuron; // Track the parent Neuron for each neighbor
                }
            }
        }

        return null; // Path not found
    }
}
