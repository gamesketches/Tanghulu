using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderScoreController : MonoBehaviour
{
    public TextMeshProUGUI totalNumber;
    public TextMeshProUGUI[] numberLabels;
    public float floatDistance;
    public float floatTime;
    public float holdTime;

    int totalScore;
    // Start is called before the first frame update
    void Start()
    {
        totalScore = 0;
        ClearLabels(0);
    }

    public void ShowFruitPoints(Vector3 position, ScoreType scoreType) {
        TextMeshProUGUI newLabel = GetFreeLabel();
        int pointsScored = 0;
        switch(scoreType) {
            case ScoreType.Correct:
                newLabel.text = GameManager.CorrectFruitValue.ToString();
                break;
            case ScoreType.Misplaced:
                newLabel.text = GameManager.MisplacedFruitValue.ToString();
                break;
            case ScoreType.Bad:
                newLabel.text = GameManager.IncorrectFruitValue.ToString();
                break;
        }
        newLabel.transform.position = position;
        newLabel.enabled = true;
        StartCoroutine(ShowPoints(newLabel));
    }

    IEnumerator ShowPoints(TextMeshProUGUI label)
    {
        Vector3 startPoint = label.transform.position;
        Vector3 endPoint = startPoint + (Vector3.up * floatDistance);
        for (float t = 0; t < floatTime; t += Time.deltaTime)
        {
            label.transform.position = Vector3.Lerp(startPoint, endPoint, t);
            yield return null;
        }

        yield return holdTime;
        label.enabled = false;
    }

    TextMeshProUGUI GetFreeLabel() { 
        foreach(TextMeshProUGUI label in numberLabels) {
            if (!label.enabled) return label;
        }

        return numberLabels[0];
    }
    
    private void ClearLabels(int ignore) { 
        foreach (TextMeshProUGUI textMesh in numberLabels) textMesh.enabled = false;
    }

    public void ResetTotalScore(Vector3 position) {
        totalNumber.text = "0" + (MultiplierDisplay.scoreMultiplier > 1 ? " x " + MultiplierDisplay.scoreMultiplier.ToString() : "");
        totalNumber.transform.position = position + (Vector3.up * floatDistance / 2);
        totalNumber.enabled = true;
        totalScore = 0;
    }

    public void ShowUpdatedTotal(ScoreType scoreType) {
        switch(scoreType) {
            case ScoreType.Correct:
                totalScore += GameManager.CorrectFruitValue;
                break;
            case ScoreType.Misplaced:
                totalScore = GameManager.MisplacedFruitValue;
                break;
            case ScoreType.Bad:
                totalScore = GameManager.IncorrectFruitValue;
                break;
        }
        totalNumber.text = totalScore.ToString() + (MultiplierDisplay.scoreMultiplier > 1 ? " x " + MultiplierDisplay.scoreMultiplier.ToString() : "");
    }

    private void OnEnable() {
        CustomerManager.ScorePoints += ClearLabels;
    }
    
    private void OnDisable() {
        CustomerManager.ScorePoints -= ClearLabels;
    }
}
