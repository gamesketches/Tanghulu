using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum FruitType { Strawberry, Kiwi, JackFruit, Tangerine, Grape, HawthornBerry, Apple };
public class GameManager : MonoBehaviour
{
    // Static variables
    public static int orderSize = 4;
    public static GameManager instance;

    // UI stuff
    public TextMeshProUGUI scoreDisplay;
    public RoundStartController gameStartImage;
    public ClockController clockController;

    public ResultScreenController resultScreen;

    // Game tuning stuff
    public float gameTime;
    public int sticksPerGame;
    public static int sticksRemaining;

    public float baseTimeBetweenCustomers;
    float customerTimer;

    public static bool gamePlaying;

    public delegate void EndGameCleanup();
    public static event EndGameCleanup EndGame;

    public PotController potController;

    int score;
    int customersGenerated;

    void Awake() { 
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        instance = this;
        score = 0;
        customersGenerated = 0;
        sticksRemaining = sticksPerGame;
    }
    // Start is called before the first frame update
    void Start()
    {
        ScaleCamera();
        StartCoroutine(BeginGameSequence());
    }

    // Update is called once per frame
    void Update()
    {
        if(gamePlaying) {
            customerTimer -= Time.deltaTime;
            if (customerTimer <= 0)
            {
                if(CustomerManager.instance.activeCustomerCount < 3 && customersGenerated < sticksPerGame)
                {
                    GenerateOrder();
                }
            }
        }
    }

    private IEnumerator BeginGameSequence() {
        yield return StartCoroutine(gameStartImage.DoRoundStart());
        //gameTimer = gameTime;
        gamePlaying = true;
        GenerateOrder();
    }

    public CustomerController VerifyFruit(FruitType[] pokedFruits) {
        sticksRemaining--;
        return CustomerManager.instance.SatisfiesCustomer(pokedFruits);
    }
    
    void GenerateOrder() {
        FruitType[] newOrder = potController.GetOrder();
        CustomerManager.instance.MakeNewCustomer(newOrder);
        customerTimer = baseTimeBetweenCustomers;
        customersGenerated++;
    }

    void AddPoints(int pointsToAdd) {
        score += pointsToAdd;
        StartCoroutine(CountUpPoints());
        if (sticksRemaining == 0) GameOver();
    }

    IEnumerator CountUpPoints() {
        int startPoints = int.Parse(scoreDisplay.text);
        int curPoints = startPoints;
        float countTime = 0.3f;
        yield return countTime;
        for(float t = 0; curPoints < score; t += countTime) {
            curPoints++;
            scoreDisplay.text = curPoints.ToString("000");
            yield return countTime;
        }
        scoreDisplay.text = score.ToString("000");
    }

    private void GameOver() {
        EndGame();
        gamePlaying = false;
        resultScreen.CountUpScore(score);
        SaveDataManager.instance.UpdatePlayerCoins(score);
    }
         
    private void ScaleCamera() {
        float inGameSize = 5.5f;
        float unitsPerPixel = inGameSize / Screen.width;

        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        Camera.main.orthographicSize = desiredHalfHeight;
    }

    private void OnEnable() {
        CustomerManager.ScorePoints += AddPoints;
    }

    private void OnDisable() {
        CustomerManager.ScorePoints -= AddPoints;
    }

    public void RestartGame() {
        LoadingScreenManager.instance.LoadScene(SceneType.RotatingPot, false, true);
    }

    public void BackToHome() {
        LoadingScreenManager.instance.LoadScene(SceneType.TitleScreen, false, true);
    }

    public void BackToHomeCurtain() { 
        LoadingScreenManager.instance.LoadScene(SceneType.TitleScreen, true, true);
    }

    public void GoToShop() {
        LoadingScreenManager.instance.LoadScene(SceneType.StoreScreen, false, true);
    }
}
