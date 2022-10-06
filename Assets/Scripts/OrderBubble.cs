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
