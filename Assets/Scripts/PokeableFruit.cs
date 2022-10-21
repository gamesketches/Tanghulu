using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeableFruit : MonoBehaviour
{
    public FruitType fruitType;

    public float appearTime = 0.4f;
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

    public void Appear(float delay = 0) {
        StartCoroutine(BobInFruit(delay));
    }

    IEnumerator BobInFruit(float delay = 0)
    {
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(delay);
        for (float t = 0; t < appearTime; t += Time.deltaTime)
        {
            float proportion = Mathf.SmoothStep(0, 1, t / appearTime);
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, proportion);
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

}
