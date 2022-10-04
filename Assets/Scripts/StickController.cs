using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    public ToppingController[] fruits;
    int pokedFruit;

    // Start is called before the first frame update
    void Start()
    {
        pokedFruit = 0;
        foreach (ToppingController fruit in fruits) fruit.ToggleRenderer(false);
    }

    public void AddFruit(ToppingType newFruit) {
        fruits[pokedFruit].ToggleRenderer(true);
        fruits[pokedFruit].UpdateFruitType(newFruit);
        pokedFruit++;
        if (pokedFruit >= fruits.Length) GameManagerWordle.Instance.VerifyTanghulu();
    }

    public void ClearStick() { 
        pokedFruit = 0;
        foreach (ToppingController fruit in fruits) fruit.ToggleRenderer(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
