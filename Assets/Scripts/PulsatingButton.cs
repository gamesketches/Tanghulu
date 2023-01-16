using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingButton : MonoBehaviour
{
    public float cycleTime;
    public float maxSize;

    void Start()
    {
        StartCoroutine(Pulsate());
    }

    IEnumerator Pulsate() {
        float t = 0;
        Vector3 startSize = transform.localScale;
        Vector3 maxSizeVector3 = new Vector3(maxSize, maxSize, maxSize);
        float halfCycle = cycleTime / 2;
        while(true) {
            float proportion = Mathf.PingPong(t, halfCycle) / halfCycle;
            transform.localScale = Vector3.Lerp(startSize, maxSizeVector3, proportion);
            t += Time.deltaTime;
            yield return null;
            }
        }
}
