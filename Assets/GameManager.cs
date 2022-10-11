using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum FruitType { Strawberry, Kiwi, Lemon };
public class GameManager : MonoBehaviour
{
    public static int orderSize = 4;

    public static GameManager instance;

    public TextMeshProUGUI score;

    // Start is called before the first frame update
    void Start()
    {
        GenerateOrder();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void OnEnable() {
        CustomerManager.ScorePoints += AddPoints;
    }

    private void OnDisable() {
        CustomerManager.ScorePoints -= AddPoints;
    }
}
