using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public OrderBubble customerBubble;
    FruitType[] order;
    public float walkOnTime = 1f;
    public Vector3 orderPosition;
    // Start is called before the first frame update
    void Start()
    {
        customerBubble.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(FruitType[] order) {
        customerBubble.SetFruits(order);
        StartCoroutine(WalkOnScreen());
    }

    private IEnumerator WalkOnScreen()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = orderPosition;
        for(float t = 0; t <= walkOnTime; t += Time.deltaTime) {
            transform.position = Vector3.Lerp(startPos, endPos, t / walkOnTime);
            yield return null;
        }
        StartCoroutine(OpenBubble());
    }

    private IEnumerator OpenBubble() {
        customerBubble.gameObject.SetActive(true);
        
        for(float t = 0; t < 0.4f; t += Time.deltaTime) {
            yield return null;
            // Put scaling in or something
        }
    }
    
    public bool OrderSatisfied(FruitType[] preparedOrder) {
        bool success = true;
        List<FruitType> stickFruits = new List<FruitType>(order);
        for(int i = 0; i < GameManager.orderSize; i++) {
            if (order[i] == preparedOrder[i]) {
                Debug.Log("Matched typed and Position");
            }
            else if (stickFruits.IndexOf(preparedOrder[i]) > -1) {
                Debug.Log("Matched type");
                stickFruits.Remove(preparedOrder[i]);
            }
            else {
                Debug.Log("One fruit is messed up");
                success = false;
            }
        }
        return success;
    }
}
