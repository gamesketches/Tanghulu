﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public OrderBubble customerBubble;

    FruitType[] order;
    SpriteRenderer spriteRenderer;

    [Header("Walking on tuning vals")]
    public float walkOnTime = 1f;
    public float bounceRange;
    public float bounceSpeed;

    [Header("Other tuning values")]
    public float bubbleOpenTime;

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
            Vector3 newPos = Vector3.Lerp(startPos, positionInLine, t / walkOnTime);
            newPos.y += Mathf.Sin(Time.time * bounceSpeed) * bounceRange;
            transform.position = newPos;
            yield return null;
        }
        StartCoroutine(OpenBubble());
    }

    public void MoveToSpotInLine(Vector3 positionInLine) {
        StartCoroutine(WalkOnScreen(positionInLine));
    }

    private IEnumerator OpenBubble() {
        customerBubble.gameObject.SetActive(true);
        Vector3 startScale = new Vector3(0, 1, 1);
        customerBubble.transform.localScale = startScale;
        
        for(float t = 0; t < bubbleOpenTime; t += Time.deltaTime) {
            float proportion = Mathf.SmoothStep(0, 1, t / bubbleOpenTime);
            customerBubble.transform.localScale = Vector3.Lerp(startScale, Vector3.one, proportion);
            yield return null;
        }
    }

    public void GetDismissed() {
        StartCoroutine(WalkOffScreen());
    }

    private IEnumerator WalkOffScreen() {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position - new Vector3(5, 0, 0);
        customerBubble.gameObject.SetActive(false);
        for(float t = 0; t <= walkOnTime; t += Time.deltaTime) {
            Vector3 newPos = Vector3.Lerp(startPos, endPos, t / walkOnTime);
            newPos.y += Mathf.Sin(Time.time * bounceSpeed) * bounceRange;
            transform.position = newPos; 
            yield return null;
        }
        gameObject.SetActive(false);
    }
    
    public int OrderSatisfied(FruitType[] preparedOrder) {
        int score = 0;
        List<FruitType> requestedOrder = new List<FruitType>(order);
        for(int i = 0; i < GameManager.orderSize; i++) {
            if (requestedOrder[i] == preparedOrder[i]) {
                score += 2;
            }
            else if (requestedOrder.IndexOf(preparedOrder[i]) > -1) {
                score += 1;
                //requestedOrder.Remove(preparedOrder[i]);
            }
            else {
                Debug.Log("One fruit is messed up");
            }
        }
        return score;
    }

}
