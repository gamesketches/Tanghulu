using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeableFruit : MonoBehaviour
{
    public FruitType fruitType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Stick")) {
            other.GetComponent<PokingStickController>().AttachFruit(transform);
            /*transform.parent = other.transform;
            Vector3 currentPosition = transform.localPosition;
            currentPosition.x = 0;
            transform.localPosition = currentPosition;*/
        }
    }
}
