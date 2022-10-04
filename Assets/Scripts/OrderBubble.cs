using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBubble : MonoBehaviour
{

    public SpriteRenderer[] fruitSprites;

    private FruitType[] currentOrder;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFruits(FruitType[] fruitTypes) {
        currentOrder = fruitTypes;
        for(int i = 0; i < fruitSprites.Length; i++) {
            fruitSprites[i].color = GetFruitColor(fruitTypes[i]);
        }
    }

    public bool OrderSatisfied(FruitType[] preparedOrder) {
        bool success = true;
        List<FruitType> stickFruits = new List<FruitType>(currentOrder);
        for(int i = 0; i < GameManager.orderSize; i++) {
            if (currentOrder[i] == preparedOrder[i]) {
                Debug.Log("Matched typed and Position");
            }
            else if (stickFruits.IndexOf(preparedOrder[i]) > -1) {
                Debug.Log("Matched type");
                stickFruits.Remove(preparedOrder[i]);
            }
            else {
                Debug.Log("One fruit is messed up");
                success = false;
            }
        }
        return success;
    }

    Color GetFruitColor(FruitType fruitType) { 
        switch(fruitType) {
            case FruitType.Strawberry:
                return Color.red;
            case FruitType.Kiwi:
                return Color.green;
            default:
            case FruitType.Lemon:
                return Color.yellow;
        }
    }
}
