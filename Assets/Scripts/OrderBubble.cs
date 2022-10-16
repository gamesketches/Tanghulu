using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBubble : MonoBehaviour
{

    public SpriteRenderer[] fruitSprites;

    public Sprite strawBerrySprite;
    public Sprite kiwiSprite;
    public Sprite jackFruitSprite;
    public Sprite tangerineSprite;
    public Sprite grapeSprite;
    public Sprite appleSprite;
    public Sprite hawthornBerrySprite;

    public void SetFruits(FruitType[] fruitTypes) {
        for(int i = 0; i < fruitSprites.Length; i++) {
            fruitSprites[i].sprite = GetFruitSprite(fruitTypes[i]);
        }
    }

    Sprite GetFruitSprite(FruitType fruitType) {
        switch (fruitType)
        {
            case FruitType.Strawberry:
                return strawBerrySprite;
            case FruitType.Kiwi:
                return kiwiSprite;
            case FruitType.JackFruit:
                return jackFruitSprite;
            case FruitType.Tangerine:
                return tangerineSprite;
            case FruitType.Grape:
                return grapeSprite;
            case FruitType.Apple:
                return appleSprite;
            default:
            case FruitType.HawthornBerry:
                return hawthornBerrySprite;
        }

    } 

    Color GetFruitColor(FruitType fruitType) { 
        switch(fruitType) {
            case FruitType.Strawberry:
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
