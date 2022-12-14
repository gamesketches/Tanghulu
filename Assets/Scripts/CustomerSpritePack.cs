using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CustomerSpritePack", order = 1)]
public class CustomerSpritePack : ScriptableObject
{
    public Sprite excitedSprite;
    public Sprite happySprite;
    public Sprite normalSprite;
    public Sprite sadSprite;
}
