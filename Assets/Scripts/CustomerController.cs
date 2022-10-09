using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public OrderBubble customerBubble;

    FruitType[] order;
    public float walkOnTime = 1f;
    public Vector3 orderPosition;

    SpriteRenderer spriteRenderer;


    void Awake()
    {
        customerBubble.gameObject.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(FruitType[] customerOrder, Sprite customerSprite, Vector3 positionInLine) {
        customerBubble.SetFruits(customerOrder);
        order = customerOrder;
        StartCoroutine(WalkOnScreen(positionInLine));
        spriteRenderer.sprite = customerSprite;
    }

    public void Leave() {
        StartCoroutine(WalkOffScreen());
    }

    private IEnumerator WalkOnScreen(Vector3 positionInLine)
    {
        Vector3 startPos = transform.position;
        for(float t = 0; t <= walkOnTime; t += Time.deltaTime) {
            transform.position = Vector3.Lerp(startPos, positionInLine, t / walkOnTime);
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

    private IEnumerator WalkOffScreen() {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position - new Vector3(5, 0, 0);
        for(float t = 0; t <= walkOnTime; t += Time.deltaTime) {
            transform.position = Vector3.Lerp(startPos, endPos, t / walkOnTime);
            yield return null;
        }
        gameObject.SetActive(false);
    }
    
    public int OrderSatisfied(FruitType[] preparedOrder) {
        int score = 0;
        List<FruitType> stickFruits = new List<FruitType>(order);
        for(int i = 0; i < GameManager.orderSize; i++) {
            if (order[i] == preparedOrder[i]) {
                score += 2;
                Debug.Log("Matched typed and Position");
            }
            else if (stickFruits.IndexOf(preparedOrder[i]) > -1) {
                Debug.Log("Matched type");
                score += 1;
                stickFruits.Remove(preparedOrder[i]);
            }
            else {
                Debug.Log("One fruit is messed up");
            }
        }
        return score;
    }
}
