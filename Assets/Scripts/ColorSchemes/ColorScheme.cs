using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorScheme", order = 1)]
public class ColorScheme : ScriptableObject
{
    public Sprite banner;

    public Sprite background;

    public Sprite tableSprite;

    public Sprite potTop;

    public Sprite potBottom;

    public Sprite mainColorSprite;

    public SchemePricePoint pricePoint;

    public Color mainColor;

    public Sprite storeSprite;

    public Sprite storeSpriteHighlighted;
}
