using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotController : MonoBehaviour 
{
    public GameObject[] fruitPrefabs;
    public int numFruits;
    public float potRadius;

    public float rotationSpeed;
    public float distanceBetweenFruits;

    Vector3 lastPosition;
    [HideInInspector]
    public bool dragging;

    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        FillPot();
        lastPosition = Vector3.forward;
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    void FillPot()
    { 
        for(int i = 0; i < numFruits; i++) {
            AddFruit();
        }
    }

    private void AddFruit() { 
        int randomIndex = Random.Range(0, fruitPrefabs.Length);
        GameObject newFruit = Instantiate(fruitPrefabs[randomIndex], transform);
        newFruit.transform.position = FindNewFruitPos();
        //newFruit.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f) * potRadius, Random.Range(-1f, 1f) * potRadius, 0);
        newFruit.transform.Rotate(0, 0, Random.Range(-180f, 180f));
    }

    private void AddFruits(int ignore) {
        for(int i = 0; i < GameManager.orderSize; i++) {
            AddFruit();
        }
    }

    Vector3 FindNewFruitPos() {
        Vector3 newPos = transform.position + new Vector3(Random.Range(-1f, 1f) * potRadius, Random.Range(-1f, 1f) * potRadius, 0);
        for(int i = 0; i < 10; i++) {
            bool noCollisions = true;
            for(int j = 0; j < transform.childCount; j++) {
                if(Vector3.Distance(newPos, transform.GetChild(j).position) < distanceBetweenFruits) {
                    noCollisions = false;
                    break;
                }
            }
            if (noCollisions) break;
            else { 
                newPos = transform.position + new Vector3(Random.Range(-1f, 1f) * potRadius, Random.Range(-1f, 1f) * potRadius, 0);
            }
        }
        return newPos;
    }

    public bool CheckTouchPosition(Vector3 touchPosition) {
        if(spriteRenderer.bounds.Contains(touchPosition)) {
            return true;
        }
        return false;
    }

    public void NewDragPosition(Vector3 newPosition) {
        if (!dragging) return;
        else if (lastPosition != Vector3.forward)
        {
            float oldPositionAngle = GetTheta(lastPosition);
            float newPositionAngle = GetTheta(newPosition);
            float difference = newPositionAngle - oldPositionAngle;
            transform.Rotate(Vector3.forward, difference);
        }
        lastPosition = newPosition;
    }

    float GetTheta(Vector3 touchPosition) {
        Vector3 adjustedVector = transform.position - touchPosition;
        return Mathf.Atan2(adjustedVector.y, adjustedVector.x) * Mathf.Rad2Deg;
    }

    public void UpdateDragging(bool newDragState) {
        dragging = newDragState;
        if (!dragging) lastPosition = Vector3.forward;
    }

    public FruitType[] GetOrder() { 
        FruitType[] newOrder = new FruitType[GameManager.orderSize];
        for (int i = 0; i < GameManager.orderSize; i++) {
            // Picking from fruits in the pot so there will alwas be some fruits in the order, but not always all fruits
            Transform randomChild = transform.GetChild(Random.Range(0, transform.childCount));
            newOrder[i] = randomChild.GetComponent<PokeableFruit>().fruitType;
        }
        return newOrder;
    }

    private void OnEnable() {
        CustomerManager.ScorePoints += AddFruits;
    }

    private void OnDisable() {
        CustomerManager.ScorePoints -= AddFruits; 
    }
}
