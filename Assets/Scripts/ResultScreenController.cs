using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultScreenController : MonoBehaviour
{
    public Canvas canvas;

    public TextMeshProUGUI scoreText;

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
        scoreText.text = "100";
    }

    private void OnEnable() {
        GameManager.EndGame += OpenResultScreen;
    } 
            
    private void OnDisable() {
        GameManager.EndGame -= OpenResultScreen;
    } 
            
}
