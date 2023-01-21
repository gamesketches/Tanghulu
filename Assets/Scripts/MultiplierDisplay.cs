using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierDisplay : MonoBehaviour
{
    public static int scoreMultiplier;

    public Sprite[] scoreSprites;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        scoreMultiplier = 1;
        UpdateScoreMultiplier(scoreMultiplier);
    }

    public void UpdateScoreMultiplier(int newMultiplier) {
        newMultiplier = Mathf.Clamp(newMultiplier, 1, 4);
        if (newMultiplier == 1) spriteRenderer.enabled = false;
        else spriteRenderer.enabled = true;

        if (newMultiplier > scoreMultiplier) scoreMultiplier = newMultiplier;
        spriteRenderer.sprite = scoreSprites[scoreMultiplier - 1];
    }

    void ResetScoreMultiplier(int ignore) {
        scoreMultiplier = 1;
        UpdateScoreMultiplier(scoreMultiplier);
    }

    void OnEnable() {
        CustomerManager.ScorePoints += ResetScoreMultiplier;
    }

    void OnDisable() {
        CustomerManager.ScorePoints -= ResetScoreMultiplier;
    }
}
