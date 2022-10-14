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
                return new Color(0.7725491f, 0.08235294f, 0.05882353f);
            case FruitType.Kiwi:
                return Color.green;
            case FruitType.JackFruit:
                return Color.yellow;
            case FruitType.Tangerine:
                return new Color(1, 0.5529412f, 0.0627451f);
            case FruitType.Grape:
                return new Color(0.3333333f, 0, 0.3254902f);
            case FruitType.Apple:
                return new Color(0.7960785f, 0.1803922f, 0.08627451f);
            default:
            case FruitType.HawthornBerry:
                return new Color(0.6313726f, 0.09803922f, 0.02352941f);

        }
    }
}
