using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotController : MonoBehaviour 
{
    public GameObject[] fruitPrefabs;
    public int numFruits;
    public float potRadius;
    public bool dragging;
    public float rotationSpeed;

    Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        FillPot();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FillPot()
    { 
        for(int i = 0; i < numFruits; i++) {
            int randomIndex = Random.Range(0, fruitPrefabs.Length);
            GameObject newFruit = Instantiate(fruitPrefabs[randomIndex], transform);
            newFruit.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f) * potRadius, Random.Range(-1f, 1f) * potRadius, 0);
        }
    }

    public bool CheckTouchPosition(Vector3 touchPosition) {
        float distance = Vector3.Distance(transform.position, touchPosition);
        if(distance < potRadius) {
            return true;
        }
        return false;
    }

    public void NewDragPosition(Vector3 newPosition) {
        if (!dragging) return;
        float oldPositionAngle = GetTheta(lastPosition);
        float newPositionAngle = GetTheta(newPosition);
        float difference = newPositionAngle - oldPositionAngle;
        transform.Rotate(Vector3.forward, difference);
        lastPosition = newPosition;
    }

    float GetTheta(Vector3 touchPosition) {
        Vector3 adjustedVector = transform.position - touchPosition;
        return Mathf.Atan2(adjustedVector.y, adjustedVector.x) * Mathf.Rad2Deg;
    }

    public void UpdateDragging(bool newDragState) {
        dragging = newDragState;
    }
}
