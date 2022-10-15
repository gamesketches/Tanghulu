using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum FruitType { Strawberry, Kiwi, JackFruit, Tangerine, Grape, HawthornBerry, Apple };
public class GameManager : MonoBehaviour
{
    public static int orderSize = 4;

    public static GameManager instance;

    public TextMeshProUGUI score;

    public float gameTime;

    private float gameTimer;

    public float baseTimeBetweenCustomers;
    float customerTimer;

    public static bool gamePlaying;

    public Image gameStartImage;

    public delegate void EndGameCleanup();
    public static event EndGameCleanup EndGame;


    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        instance = this;
        StartCoroutine(BeginGameSequence());
    }

    // Update is called once per frame
    void Update()
    {
        if(gamePlaying) {
            customerTimer -= Time.deltaTime;
            gameTimer -= Time.deltaTime;
            if(customerTimer <= 0) {
                GenerateOrder();
            }
            if(gameTimer < 0) {
                GameOver();
            }
            }
    }

    private IEnumerator BeginGameSequence() {
        gameStartImage.enabled = true;
        yield return new WaitForSeconds(0.5f);
        gameStartImage.enabled = false;
        gameTimer = gameTime;
        gamePlaying = true;
        GenerateOrder();
    }

    public void VerifyFruit(PokeableFruit[] pokedFruits) {
        FruitType[] fruitOnStick = new FruitType[orderSize];
        for(int i = 0; i < orderSize; i++) {
            fruitOnStick[i] = pokedFruits[i].fruitType;
        }
        if(CustomerManager.instance.CustomerServed(fruitOnStick)) {
            GenerateOrder();
        } else {
            Debug.Log("order invalid :(");
        }
    }
    
    void GenerateOrder() {
        FruitType[] newOrder = new FruitType[orderSize];
        for(int i = 0; i < orderSize; i++) {
            newOrder[i] = (FruitType)Random.Range(0, 3);
        }
        CustomerManager.instance.MakeNewCustomer(newOrder);
        customerTimer = baseTimeBetweenCustomers;
    }

    void AddPoints(int pointsToAdd) {
        StartCoroutine(CountUpPoints(pointsToAdd));
    }

    IEnumerator CountUpPoints(int pointsToAdd) {
        int startPoints = int.Parse(score.text);
        int curPoints = startPoints;
        float countTime = 0.3f;
        yield return countTime;
        for(float t = 0; curPoints < startPoints + pointsToAdd; t += countTime) {
            curPoints++;
            score.text = curPoints.ToString();
            yield return countTime;
        }
        score.text = (startPoints + pointsToAdd).ToString();
    }

    private void GameOver() {
        EndGame();
        gamePlaying = false;
        Debug.Log("ending the game");
    }
         

    private void OnEnable() {
        CustomerManager.ScorePoints += AddPoints;
    }

    private void OnDisable() {
        CustomerManager.ScorePoints -= AddPoints;
    }
}
