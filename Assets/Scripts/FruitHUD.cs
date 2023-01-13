using UnityEngine;

public class FruitHUD : MonoBehaviour
{
    public Sprite strawberrySprite;
    public Sprite kiwiSprite;
    public Sprite jackfruitSprite;
    public Sprite appleSprite;
    public Sprite tangerineSprite;
    public Sprite hawthornSprite;
    public Sprite grapeSprite;
    public Sprite emptySprite;

    SpriteRenderer[] displaySprites;
    // Start is called before the first frame update
    void Start()
    {
        displaySprites = GetComponentsInChildren<SpriteRenderer>();
        ClearDisplay();
    }

    public void UpdateDisplay(FruitType[] currentFruits)
    {
        for (int i = 0; i < currentFruits.Length; i++)
        {
            displaySprites[i].sprite = GetFruitSprite(currentFruits[i]);
        }
    }

    public void ClearDisplay() { 
        foreach (SpriteRenderer sprite in displaySprites) sprite.sprite = emptySprite;
    }

    Sprite GetFruitSprite(FruitType fruitType) { 
        switch(fruitType) {
            case FruitType.Strawberry:
                return strawberrySprite;
            case FruitType.Kiwi:
                return kiwiSprite;
            case FruitType.JackFruit:
                return jackfruitSprite;
            case FruitType.Tangerine:
                return tangerineSprite;
            case FruitType.Grape:
                return grapeSprite;
            case FruitType.HawthornBerry:
                return hawthornSprite;
            case FruitType.Apple:
                return appleSprite;
            default:
                return emptySprite;
        }
    }
}
