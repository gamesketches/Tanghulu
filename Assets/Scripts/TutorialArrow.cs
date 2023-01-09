using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    public float timeTilAppearance;
    public bool shown;
    public Vector3 targetPoint;

    SpriteRenderer spriteRenderer;

    public float alphaLevel;
    public float cycleTime;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.clear;
        float timer = 0;
        while(timer < timeTilAppearance && !shown) {
            timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ShowArrow());
    }

    IEnumerator ShowArrow() {
        Vector3 startPoint = transform.position;
        Color fadedColor = new Color(1, 1, 1, alphaLevel);
        while(true) { 
            for(float t = 0; t < cycleTime * 2; t += Time.deltaTime) {
                float proportion = Mathf.SmoothStep(0, 1, Mathf.PingPong(t, cycleTime));
                transform.position = Vector3.Slerp(startPoint, targetPoint, proportion);
                spriteRenderer.color = Color.Lerp(fadedColor, Color.white, proportion);
                yield return null;
            }
        }
    }

    public void DeactivateArrow() {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
