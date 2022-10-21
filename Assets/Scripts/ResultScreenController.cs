using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultScreenController : MonoBehaviour
{
    public Canvas canvas;

    public TextMeshProUGUI scoreText;
    
    public float countUpTime;

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenResultScreen() {
        canvas.enabled = true;
    }

    public void CountUpScore(int playerScore) {
        StartCoroutine(CountUpPoints(playerScore));
    }
    
    IEnumerator CountUpPoints(int totalScore) {
        int pointCount = 0;
        yield return countUpTime;
        for(float t = 0; pointCount < totalScore; t += countUpTime) {
            pointCount++;
            scoreText.text = pointCount.ToString();
            yield return countUpTime;
        }
        scoreText.text = totalScore.ToString();
    }

    private void OnEnable() {
        GameManager.EndGame += OpenResultScreen;
    } 
            
    private void OnDisable() {
        GameManager.EndGame -= OpenResultScreen;
    } 
            
}
