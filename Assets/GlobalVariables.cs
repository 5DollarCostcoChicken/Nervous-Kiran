using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables _instance = null; public static GlobalVariables instance { get { return _instance; } }
    
    public GameObject triviaAnswerPopup;
    public TextMeshProUGUI triviaText;

    private void Awake()
    {
        _instance = this;
    }
    private Vector2 cursorHotspot;
    public Texture2D cursorTexture;
    public Texture2D cursorTextureHeld;

    public GameObject timerPrefab;
    public Canvas canvas;
    public GameObject timers;
    public GameObject roads;

    public float score = 0;
    public float scoreGoal;

    public float counterGoal;

    bool scoring = false;
    public TextMeshProUGUI scoreText;

    public GameObject[] allNeurons;
    GameObject[] previousallNeurons;

    public TextMeshProUGUI sensoryText;
    public TextMeshProUGUI motorText;
    public TextMeshProUGUI spinalText;
    public SpawnButton sensoryLimit;
    public SpawnButton motorLimit;
    public SpawnButton spinalLimit;
    public int sensoryMaxLimit;
    public int motorMaxLimit;
    public int spinalMaxLimit;
    public AudioSource popSound;
    private void Start()
    {
        scoreText.text = "Time: -/" + scoreGoal.ToString("n3");
        scoreText.color = Color.white;
        allNeurons = GameObject.FindGameObjectsWithTag("Neuron");
        previousallNeurons = GameObject.FindGameObjectsWithTag("Neuron");
        sensoryMaxLimit = sensoryLimit.spawns;
        motorMaxLimit = motorLimit.spawns;
        spinalMaxLimit = spinalLimit.spawns;
        sensoryNeurons.Clear();
        motorNeurons.Clear();
        relayNeurons.Clear();
        popSound = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (scoring)
            scoreText.text = "Time: " + score.ToString("n3") + "/" + scoreGoal.ToString("n3");

        sensoryText.text = sensoryNeurons.Count + "/" + sensoryMaxLimit;
        if (sensoryNeurons.Count < sensoryMaxLimit)
            sensoryText.color = Color.red;
        else
            sensoryText.color = Color.green;
        motorText.text = motorNeurons.Count + "/" + motorMaxLimit;
        if (motorNeurons.Count < motorMaxLimit)
            motorText.color = Color.red;
        else
            motorText.color = Color.green;
        spinalText.text = relayNeurons.Count + "/" + spinalMaxLimit;
        if (relayNeurons.Count < spinalMaxLimit)
            spinalText.color = Color.red;
        else
            spinalText.color = Color.green;

        allNeurons = GameObject.FindGameObjectsWithTag("Neuron");
        if (allNeurons.Length != previousallNeurons.Length)
        {
            sensoryNeurons.Clear();
            motorNeurons.Clear();
            relayNeurons.Clear();
            foreach (GameObject g in allNeurons)
            {
                if (g.name.Contains("Clone"))
                {
                    if (g.name.Contains("Test Spinal"))
                    {
                        relayNeurons.Add(g.GetComponent<Neuron>());
                    }
                    if (g.name.Contains("Test Motor"))
                    {
                        motorNeurons.Add(g.GetComponent<Neuron>());
                    }
                    if (g.name.Contains("Test Sensory"))
                    {
                        sensoryNeurons.Add(g.GetComponent<Neuron>());
                    }
                }
            }
        }
        previousallNeurons = allNeurons;


        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorTextureHeld, cursorHotspot, CursorMode.Auto);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TempButton"))
            {
                Destroy(obj);
            }
        }

        if (currentRoad != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Vector3 direction = mousePosition - currentRoad.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            currentRoad.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            currentRoad.transform.localScale = new Vector3(Vector3.Distance(mousePosition, currentRoad.transform.position), currentRoad.transform.localScale.y, currentRoad.transform.localScale.z);

            if (currentNeuron != null && currentNeuron != neuron1)
            {
                bool b1 = (neuron1.name.Contains("Sensory") && currentNeuron.name.Contains("Relay") && !currentNeuron.name.Contains("Motor"));
                bool b2 = (neuron1.name.Contains("Relay") && !neuron1.name.Contains("Sensory") && currentNeuron.name.Contains("Motor"));
                bool b3 = (neuron1.name.Contains("Relay") && !neuron1.name.Contains("Motor") && currentNeuron.name.Contains("Sensory"));
                bool b4 = (neuron1.name.Contains("Motor") && currentNeuron.name.Contains("Relay") && !currentNeuron.name.Contains("Sensory"));
                if (b1 || b2 || b3 || b4)
                {
                    currentRoad.GetComponentInChildren<Road>().valid = true;
                }
                else
                {
                    currentRoad.GetComponentInChildren<Road>().invalid = true;
                }
            }
            else
            {
                currentRoad.GetComponentInChildren<Road>().valid = false;
                currentRoad.GetComponentInChildren<Road>().invalid = currentRoad.GetComponentInChildren<Road>().colliding;
            }
            if (currentRoad.transform.localScale.x > 5.5f)
            {
                currentRoad.GetComponentInChildren<Road>().invalid = true;
                currentRoad.GetComponentInChildren<Road>().valid = false;
            }
            currentRoad.transform.localScale = new Vector3(currentRoad.transform.localScale.x, -0.0526f * currentRoad.transform.localScale.x + 1.0526f, 1);

            if (currentRoad.GetComponentInChildren<Road>().invalid)
            {
                currentRoad.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
            else if (currentRoad.GetComponentInChildren<Road>().valid)
            {
                currentRoad.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
            else
            {
                currentRoad.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
        }
        if (!scoring)
        {
            if ((relayNeurons.Count >= spinalMaxLimit) && (motorNeurons.Count >= motorMaxLimit) && (sensoryNeurons.Count >= sensoryMaxLimit))
                sparkButton.interactable = true;
            else
                sparkButton.interactable = false;
        }
        else
        {
            score += Time.deltaTime;
        }
    }

    public void showTrivia(int trivia)
    {
        if (trivia == 0)
        {
            triviaAnswerPopup.SetActive(false);
        }
        else
        {
            setTriviaText(trivia);
            triviaAnswerPopup.SetActive(true);
        }
    }
    public void setTriviaText(int trivia)
    {
        switch (trivia)
        {
            case 1: //Axons: the roads
                triviaText.text = "Relay Neuron: \n\nRelay Neurons carry signals from sensory neurons all the way to the spinal cord, where they are given direction and send signals back down to motor neurons. Transporting the signal is the Axon, the part of the neuron that carries the signal from the cell body to another's axon terminal, much like a road. ";
                break;
            case 2: //Myelin Sheaths: the speed boost
                triviaText.text = "Myelin Sheath: \n\nThe Myelin Sheath is a layer of fat that surrounds the axon, providing a speed boost to the neuron's signal.";
                break;
            case 3: //Dendrite: the energy relay
                triviaText.text = "Dendrite: \n\nThe Dendrites are a neuron's receptive region, recieving signals to pass to other cells. The more dendrites, the more signals can be recieved.";
                break;
            case 4: //Spinal Cord
                triviaText.text = "Nervous System: \n\nNeurons bring the signal all the way up to the spinal cord to the RELAY NEURONS, where motor function is dictated and sent back down to the muscles. All signals must be brought up the spinal cord to recieve function.";
                break;
            case 5: //Muscle
                triviaText.text = "Nervous System: \n\nThe nerve endings of MOTOR NEURONS are given function by signals coming from the spinal cord, and move muscles accordingly.";
                break;
            case 6: //Hand
                triviaText.text = "Nervous System: \n\nThe nerve endings of SENSORY NEURONS, located in the hand, are activated through touch, and instinctively send a singal to the spinal cord.";
                break;
            case 7: //Relay Neuron
                triviaText.text = "Relay Neuron: \n\nRelay Neurons, or Inter Neurons, instinctively decide motor function and send it back down to the motor neurons.";
                break;
            case 8: //Motor Neuron
                triviaText.text = "Motor Neuron: \n\nMotor Neurons recieve function, spreading the signal throughout their nerve endings to inform muscles and tendons to move.";
                break;
            case 9: //Sensory Neuron
                triviaText.text = "Sensory Neuron: \n\nSensory Neurons are activated through touch, and carry signals to the spinal cord.";
                break;
               
        }
    }

    public GameObject roadExample;
    public GameObject currentRoad = null;
    public GameObject currentNeuron;
    public GameObject neuron1;
    public GameObject neuron2;

    public void beginRoadHold(GameObject neuron)
    {
        neuron1 = neuron;
        currentRoad = Instantiate(roadExample, roads.transform);
        currentRoad.transform.position = new Vector3(neuron1.transform.position.x, neuron1.transform.position.y, -1f);
        if (neuron1.name.Contains("Sensory"))
            currentRoad.GetComponentInChildren<SpriteRenderer>().sprite = currentRoad.GetComponentInChildren<Road>().sensoryImage;
    }
    public void endRoadHold(GameObject neuron)
    {
        neuron2 = neuron;
        bool b1 = (neuron1.name.Contains("Sensory") && neuron2.name.Contains("Relay") && !neuron2.name.Contains("Motor"));
        bool b2 = (neuron1.name.Contains("Relay") && !neuron1.name.Contains("Sensory") && neuron2.name.Contains("Motor"));
        bool b3 = (neuron1.name.Contains("Relay") && !neuron1.name.Contains("Motor") && neuron2.name.Contains("Sensory"));
        bool b4 = (neuron1.name.Contains("Motor") && neuron2.name.Contains("Relay") && !neuron2.name.Contains("Sensory"));
        if ((b1 || b2 || b3 || b4) && currentRoad.GetComponentInChildren<Road>().valid && !currentRoad.GetComponentInChildren<Road>().invalid) {
            neuron1.GetComponent<Neuron>().connections.Add(neuron2.GetComponent<Neuron>());
            neuron2.GetComponent<Neuron>().connections.Add(neuron1.GetComponent<Neuron>());
            Vector3 direction = neuron2.transform.position - currentRoad.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentRoad.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            currentRoad.GetComponentInChildren<Road>().neuron1 = neuron1;
            currentRoad.GetComponentInChildren<Road>().neuron2 = neuron2;
            currentRoad.GetComponentInChildren<Road>().connected = true;

            if (neuron2.name.Contains("Relay") && !neuron2.name.Contains("Spinal"))
            {
                if (neuron1.name.Contains("Sensory"))
                {
                    neuron2.GetComponent<SpriteRenderer>().color = new Color(20f/255f, 80f / 255f, 116f / 255f);
                    neuron2.name = "Prefab Sensory Relay Neuron";
                }
                else if (neuron1.name.Contains("Motor"))
                {
                    neuron2.GetComponent<SpriteRenderer>().color = new Color(125f / 255f, 14f / 255f, 16f / 255f);
                    neuron2.name = "Prefab Motor Relay Neuron";
                }
            }
            else if (neuron1.name.Contains("Relay") && !neuron1.name.Contains("Spinal"))
            {
                if (neuron2.name.Contains("Sensory"))
                {
                    neuron1.GetComponent<SpriteRenderer>().color = new Color(20f / 255f, 80f / 255f, 116f / 255f);
                    neuron1.name = "Prefab Sensory Relay Neuron";
                }
                else if (neuron2.name.Contains("Motor"))
                {
                    neuron1.GetComponent<SpriteRenderer>().color = new Color(125f / 255f, 14f / 255f, 16f / 255f);
                    neuron1.name = "Prefab Motor Relay Neuron";
                }
            }
            if (neuron2.name.Contains("Sensory"))
                currentRoad.GetComponentInChildren<SpriteRenderer>().sprite = currentRoad.GetComponentInChildren<Road>().sensoryImage;

            neuron1 = null;
            currentRoad.GetComponentInChildren<Road>().valid = false;
            currentRoad.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            currentRoad = null;
        }
        else
        {
            Destroy(currentRoad.gameObject);
            currentRoad = null;
        }
    }
    public Button sparkButton;
    public void scoreIncrease()
    {
        counterGoal--;
        GameObject[] signals = GameObject.FindGameObjectsWithTag("Signal");
        if (signals.Length == 0)
        {
            scoring = false;
            if (counterGoal <= 0)
            {
                if (score <= scoreGoal)
                    scoreText.color = Color.green;
                else
                    scoreText.color = Color.red;
            }
            else
            {
                scoreText.color = Color.red;
                scoreText.text = "Time: -/" + scoreGoal.ToString("n3");
            }
        }
    }

    //Graph section
    public List<Neuron> sensoryNeurons;
    public List<Neuron> motorNeurons;
    public List<Neuron> relayNeurons;
    public GameObject pulsePrefab;
    public void Spark()
    {
        sparkButton.interactable = false;
        score = 0;
        scoring = true;
        scoreText.color = Color.white;
        foreach (Neuron sense in sensoryNeurons)
        {
            GameObject pulse = Instantiate(pulsePrefab, this.transform);
            pulse.transform.position = sense.transform.position;
            pulse.tag = "Signal";
            pulse.GetComponent<Pulse>().currentNeuron = sense;
            pulse.GetComponent<Pulse>().endNeuronPos = relayNeurons[Random.Range(0, relayNeurons.Count)];
            // TODO change this when you add myelin sheathes and or dendrites
            pulse.GetComponent<Pulse>().speed = 2;
        }
        foreach (Neuron motor in motorNeurons)
        {
            motor.GetComponent<MotorNeuron>().counters = motor.GetComponent<MotorNeuron>().MaxCounters;
            counterGoal += motor.GetComponent<MotorNeuron>().MaxCounters;
            foreach (Neuron sense in relayNeurons)
            {
                sense.GetComponent<RelayNeuron>().motorNeuronsQueue.Enqueue(motor);
            }
        }
    }

    public void playPopSound()
    {
        popSound.volume = Random.Range(.3f, .5f);
        popSound.pitch = Random.Range(.55f, 1.25f);
        popSound.Play();
    }
}
