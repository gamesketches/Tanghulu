using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokingStickController : MonoBehaviour
{
    public float stickLength;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckTouchPosition(Vector3 position)
    {
        return Mathf.Abs(position.y - transform.position.y) < transform.localScale.y / 2;
    }

    public void PokeStick() {
        StartCoroutine(StickPokingAnimation());
    }

    private IEnumerator StickPokingAnimation() {
        Debug.Log("Poking stick");
        float stickPokeTime = 1;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x, transform.position.y + stickLength,
                                                transform.position.z);
        for(float t = 0; t < stickPokeTime; t += Time.deltaTime) {
            float proportion = Mathf.PingPong(t, stickPokeTime / 2);
            transform.position = Vector3.Lerp(startPosition, endPosition, proportion);
            yield return null;
        }
        transform.position = startPosition;
    }

    public void AttachFruit(Transform newFruit) {
        newFruit.parent = transform;
        newFruit.localRotation = Quaternion.identity;
        float pushDistance = 0.1f;
        for (int i = transform.childCount - 1; i > -1; i--) {
            Transform childFruit = transform.GetChild(i);
            Vector3 newFruitPosition = newFruit.localPosition;
            newFruitPosition.x = 0;
            newFruitPosition.y = (StickEnd(true) - pushDistance);
            pushDistance -= childFruit.lossyScale.y;
            childFruit.localPosition = newFruitPosition;
        }
    }

    private float StickEnd(bool local = false) {
        float yOffset = stickLength / 2;
        if (local)
            return yOffset;
        else
            return transform.position.y + yOffset;
    }
}
