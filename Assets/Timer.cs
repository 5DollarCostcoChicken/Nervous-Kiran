using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI counter;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator begin(Vector3 pos)
    {
        transform.localScale = Vector3.zero;
        transform.position = Camera.main.WorldToScreenPoint(pos) + Vector3.up * 60f;
        for (int i = 1; i <= 12; i++) {
            yield return new WaitForSecondsRealtime(.02f);
            transform.localScale = new Vector3((i / 10f) * .3f, (i / 10f) * .3f, (i / 10f) * .3f);
        }
        for (int i = 1; i <= 2; i++)
        {
            yield return new WaitForSecondsRealtime(.02f);
            transform.localScale = new Vector3(((12 - i) / 10f) * .3f, ((12 - i) / 10f) * .3f, ((12 - i) / 10f) * .3f);
        }
    }
}
