using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
}
