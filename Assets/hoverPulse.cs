using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverPulse : MonoBehaviour
{
    public void MouseEnter()
    {
        StartCoroutine(pulse(true));
    }
    public void MouseExit()
    {
        StartCoroutine(pulse(false));
    }
    private void OnMouseEnter()
    {
        StartCoroutine(pulse(true));
    }
    private void OnMouseExit()
    {
        StartCoroutine(pulse(false));
    }
    private IEnumerator pulse(bool direction)
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSecondsRealtime(.01f);
            if (direction)
                this.transform.localScale *= 1.02f;
            if (!direction)
                this.transform.localScale /= 1.02f;
        }
    }
}
