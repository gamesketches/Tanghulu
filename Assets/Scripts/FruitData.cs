using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FruitData", order = 1)]
public class FruitData : ScriptableObject
{
    public float hitStopTime = 0.04f;
    public float slidingTime = 0.2f;
    public float squishSize = 0.9f;
}
