using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBubble : MonoBehaviour
{
    public SpriteRenderer[] fruitSprites;

    public SpriteRenderer[] scoringSprites;

    public Sprite strawBerrySprite;
    public Sprite kiwiSprite;
    public Sprite jackFruitSprite;
    public Sprite tangerineSprite;
    public Sprite grapeSprite;
    public Sprite appleSprite;
    public Sprite hawthornBerrySprite;

    public Sprite correctSprite;
    public Sprite misplacedSprite;
    public Sprite wrongSprite;

    [Header("Pop in tuning")]
    public AnimationCurve popCurve;
    public float timeBetween;

    public float scoringTimeBetween;

    private static OrderScoreController orderScoreController;

    void Start() {
        if (orderScoreController == null)
            orderScoreController = GameObject.Find("OrderScoreController").GetComponent<OrderScoreController>();
    }

    public void SetFruits(FruitType[] fruitTypes) {
        for(int i = 0; i < fruitSprites.Length; i++) {
            fruitSprites[i].sprite = GetFruitSprite(fruitTypes[i]);
            fruitSprites[i].transform.localScale = Vector3.zero;
            scoringSprites[i].color = Color.clear;
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

    public void ShowServeAnimation(ScoreType[] scoreReport) {
        StartCoroutine(ServeAnimation(scoreReport));
        Debug.Log("Serve animation started");
    }

    public IEnumerator ServeAnimation(ScoreType[] scoreReport) {
        orderScoreController.ResetTotalScore(transform.position);
        SFXManager.instance.PlaySoundEffect(SoundEffectType.Tally);
        //bool playPerfect = true;
        for(int i = 0; i < scoringSprites.Length; i++) { 
            switch(scoreReport[i]) {
                case ScoreType.Correct:
                    scoringSprites[i].sprite = correctSprite;
                    break;
                case ScoreType.Misplaced:
                    scoringSprites[i].sprite = misplacedSprite;
         //           playPerfect = false;
                    break;
                case ScoreType.Bad:
                    scoringSprites[i].sprite = wrongSprite;
          //          playPerfect = false;
                    break;
            }
            scoringSprites[i].color = Color.white;
            orderScoreController.ShowFruitPoints(scoringSprites[i].transform.position, scoreReport[i]);
            yield return new WaitForSeconds(scoringTimeBetween);
            orderScoreController.ShowUpdatedTotal(scoreReport[i]);
        }
        //if (playPerfect) SFXManager.instance.PlaySoundEffect(SoundEffectType.Perfect);
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
