using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotController : MonoBehaviour
{
    public GameObject[] fruitPrefabs;
    public Transform shadowTransform;
    public int numFruits;
    public float potRadius;
    private List<PokeableFruit> fruitBag;

    public float rotationSpeed;
    public float distanceBetweenFruits;
    public int placementRetries;

    public TutorialArrow[] tutorialArrows;

    Vector3 lastPosition;
    [HideInInspector]
    public bool dragging;

    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        FillFruitBag();
        FillPot();
        lastPosition = Vector3.forward;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FillPot()
    {
        for (int i = 0; i < numFruits; i++) {
            AddRandomFruit(i);
        }
    }

    private void AddFruit(int bagIndex, int delayMultiplier = 0) {
        PokeableFruit newFruit = fruitBag[bagIndex];
        newFruit.gameObject.SetActive(true);
        newFruit.transform.parent = transform;
        fruitBag.RemoveAt(bagIndex);
        newFruit.transform.position = FindNewFruitPos();
        newFruit.transform.Rotate(0, 0, Random.Range(-180f, 180f));
        newFruit.Appear(delayMultiplier * 0.1f);
        if (fruitBag.Count == 0) FillFruitBag();
    }

    private void AddRandomFruit(int delayMultiplier = 0) {
        int randomIndex = Random.Range(0, fruitBag.Count);
        AddFruit(randomIndex, delayMultiplier);        
    }

    private void AddNeededFruit(FruitType[] neededFruits, int delayMultiplier = 0) { 
        for(int i = PokingStickController.fruitsPoked; i < neededFruits.Length; i += GameManager.orderSize) { 
            for(int j = 0; j < fruitBag.Count; j++) {
                if (fruitBag[j].fruitType == neededFruits[i]) {
                    AddFruit(j, delayMultiplier);
                    return;
                }
            }
        }
        AddRandomFruit(delayMultiplier);
    }

    private void AddFruitsAfterPoke() {
        FruitType[] neededFruits = CustomerManager.instance.GetNeededFruits();
        int delayCounter = 0;
        for (int i = transform.childCount; i < numFruits; i++) {
            AddNeededFruit(neededFruits, delayCounter);
            delayCounter++;
        }
    }

    private void AddFruits(int ignore) {
        FruitType[] neededFruits = CustomerManager.instance.GetNeededFruits();
        for(int i = transform.childCount; i < numFruits; i++) {
            AddRandomFruit(i);
        }
    }

    Vector3 FindNewFruitPos() {
        Vector3 newPos = transform.position + new Vector3(Random.Range(-1f, 1f) * potRadius, Random.Range(-1f, 1f) * potRadius, 0);
        for(int i = 0; i < placementRetries; i++) {
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
        newPos.z = -0.1f;
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

            float distance = Vector3.Distance(newPosition, lastPosition);
            if (Mathf.Approximately(distance, 0)) SFXManager.instance.StopRotateSound();
            else SFXManager.instance.AddToRotateDistance(Mathf.Abs(distance));

            transform.Rotate(Vector3.forward, difference);
            shadowTransform.Rotate(Vector3.forward, difference);
            if(Mathf.Abs(difference) > 1f) SFXManager.instance.PlaySoundEffect(SoundEffectType.RotatePot);
        }
        lastPosition = newPosition;
    }

    float GetTheta(Vector3 touchPosition) {
        Vector3 adjustedVector = transform.position - touchPosition;
        return Mathf.Atan2(adjustedVector.y, adjustedVector.x) * Mathf.Rad2Deg;
    }

    public void UpdateDragging(bool newDragState) {
        dragging = newDragState;
        DisableTutorialArrows();
        lastPosition = Vector3.forward;
        /*if (!dragging) {
            SFXManager.instance.PlaySoundEffect(SoundEffectType.AimStickEnd);
            lastPosition = Vector3.forward;
        }
        else {
            SFXManager.instance.PlaySoundEffect(SoundEffectType.RotatePot);
        }*/
    }

    public FruitType[] GetOrder() { 
        FruitType[] newOrder = new FruitType[GameManager.orderSize];
        List<int> numbersPicked = new List<int>();
        for (int i = 0; i < GameManager.orderSize; i++) {
            // Picking from fruits in the pot so there will alwas be some fruits in the order, but not always all fruits
            int diceRoll = Random.Range(0, transform.childCount);
            while (numbersPicked.IndexOf(diceRoll) > -1) diceRoll = Random.Range(0, transform.childCount);
            Transform randomChild = transform.GetChild(diceRoll);
            newOrder[i] = randomChild.GetComponent<PokeableFruit>().fruitType;
            numbersPicked.Add(diceRoll);
        }
        return newOrder;
    }

    private void FillFruitBag() {
        fruitBag = new List<PokeableFruit>();
        foreach (GameObject obj in fruitPrefabs) {
            PokeableFruit newFruit = Instantiate(obj).GetComponent<PokeableFruit>();
            fruitBag.Add(newFruit);
            newFruit.gameObject.SetActive(false);
        }
    }

    void DisableTutorialArrows() { 
        foreach(TutorialArrow arrow in tutorialArrows) {
            arrow.DeactivateArrow();
        }
    }

    private void OnEnable() {
        CustomerManager.ScorePoints += AddFruits;
        PokingStickController.StickFinishedPoking += AddFruitsAfterPoke;
    }

    private void OnDisable() {
        CustomerManager.ScorePoints -= AddFruits; 
        PokingStickController.StickFinishedPoking -= AddFruitsAfterPoke;
    }
}
