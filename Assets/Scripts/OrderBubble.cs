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

    [Header("Pop in tuning")]
    public AnimationCurve popCurve;
    public float timeBetween;

    public void SetFruits(FruitType[] fruitTypes) {
        for(int i = 0; i < fruitSprites.Length; i++) {
            fruitSprites[i].sprite = GetFruitSprite(fruitTypes[i]);
            fruitSprites[i].transform.localScale = Vector3.zero;
        }
    }

    public void ShowFruits() {
        StartCoroutine(ShowFruitAnimation());
    }

    IEnumerator ShowFruitAnimation() {
        float animationLength = popCurve[popCurve.length - 1].time;
        for(int i = 0; i < fruitSprites.Length; i++) { 
            for(float t = 0; t < animationLength; t += Time.deltaTime) {
                fruitSprites[i].transform.localScale = Vector3.one * popCurve.Evaluate(t);
                yield return null;
            }
            yield return new WaitForSeconds(timeBetween);
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
}
