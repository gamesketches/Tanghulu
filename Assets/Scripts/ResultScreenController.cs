﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultScreenController : MonoBehaviour
{
    public Canvas canvas;

    public TextMeshProUGUI scoreText;
    
    public float countUpTime;
    public float curtainDownTime;

    public RectTransform curtainRect;
    public Image curtainFringe;

    public float highScoreDelay;
    public Image highScoreLabel;

    IEnumerator curtainOnRoutine;

    // Start is called before the first frame update
    void Start()
    {
        curtainRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -Screen.height, canvas.GetComponent<RectTransform>().rect.height);
        curtainRect.GetComponent<Image>().sprite = ColorSchemeManager.currentColorScheme.mainColorSprite;
        curtainFringe.sprite = ColorSchemeManager.currentColorScheme.banner;
        canvas.enabled = false;
        highScoreLabel.enabled = false;
    }

    private void OpenResultScreen() {
        canvas.enabled = true;
        SFXManager.instance.BeginFadeMusic(1, 0.1f, curtainDownTime);
        curtainOnRoutine = MoveResultsScreenOn();
        StartCoroutine(MoveResultsScreenOn());
    }

    IEnumerator MoveResultsScreenOn() { 
        float proportion;
        for(float t = 0; t < curtainDownTime; t += Time.deltaTime) {
            proportion = Mathf.SmoothStep(-Screen.height, 0, t / curtainDownTime);

            curtainRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, proportion, curtainRect.rect.size.y);
            yield return null;
        }
    }

    public void CountUpScore(int playerScore, bool isHighScore = false) {
        StartCoroutine(CountUpPoints(playerScore, isHighScore));
    }
    
    IEnumerator CountUpPoints(int totalScore, bool isHighScore = false) {
        int pointCount = 0;
        yield return curtainOnRoutine;
        yield return countUpTime;
        for(float t = 0; pointCount < totalScore; t += countUpTime) {
            pointCount++;
            scoreText.text = pointCount.ToString();
            yield return countUpTime;
        }
        scoreText.text = totalScore.ToString();
        if(isHighScore)
        {
            yield return new WaitForSeconds(highScoreDelay);
            highScoreLabel.enabled = true;
        }
    }

    private void OnEnable() {
        GameManager.EndGame += OpenResultScreen;
    } 
            
    private void OnDisable() {
        GameManager.EndGame -= OpenResultScreen;
    } 
            
}
