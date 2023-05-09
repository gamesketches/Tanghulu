using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootButton : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    IEnumerator lerpingRoutine;

    public float shrunkSize;
    public float normalSize;
    public float scaleTime;
    public Color shrinkColor;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool CheckTouchPosition(Vector3 touchPosition) {
        if(spriteRenderer.bounds.Contains(touchPosition)) {
            return true;
        }
        return false;
    }

    public void ScaleButtonDown() {
        lerpingRoutine = null;
        Vector3 smallScale = new Vector3(shrunkSize, shrunkSize, shrunkSize);
        lerpingRoutine = ScaleButton(smallScale, shrinkColor);
        StartCoroutine(lerpingRoutine);
    }

    public void ScaleButtonUp() {
        lerpingRoutine = null;
        lerpingRoutine = ScaleButton(Vector3.one * normalSize, Color.white);
        StartCoroutine(lerpingRoutine);
    }


    IEnumerator ScaleButton(Vector3 targetScale, Color targetColor) {
        Vector3 startScale = transform.localScale;
        Color startColor = spriteRenderer.color;
        for(float t = 0; t < scaleTime; t += Time.deltaTime) {
            float proportion = Mathf.SmoothStep(0, 1, t / scaleTime);
            transform.localScale = Vector3.Lerp(startScale, targetScale, proportion);
            spriteRenderer.color = Color.Lerp(startColor, targetColor, proportion);
            yield return null;
        }
        spriteRenderer.color = targetColor;
        transform.localScale = targetScale;
    }
}
