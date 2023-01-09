using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokingStickController : MonoBehaviour
{
    public float stickLength;
    public float pokeDistance;
    public float serveTime;
    public float serveEndingScale;
    public float stickTouchHeight;
    public bool aiming;
    public bool poking;
    BoxCollider2D stickCollider;
    private Vector3 lastFingerPosition;
    public int maxFruits;

    public float rotationLimit;
    public float touchLimit;
    public float touchDampValue;

    private float rotationProportion;

    float pokeMultiplier;

    public AnimationCurve pokingCurve;
    public float fruitSlidingTime;

    public StickCounter stickCounter;
    private IEnumerator pokingCoroutine;
    bool fruitBehavior;

    public TutorialArrow[] tutorialArrows;

    public delegate void StickPoked();
    public static event StickPoked StickFinishedPoking;

    // Start is called before the first frame update
    void Start()
    {
        aiming = false;
        poking = false;
        pokeMultiplier = 1;
        rotationProportion = 0.5f;
        pokingCurve.postWrapMode = WrapMode.PingPong;
        stickCollider = GetComponent<BoxCollider2D>();
        stickCollider.enabled = false;
    }

    public bool CheckTouchPosition(Vector3 position)
    {
        return position.y < stickTouchHeight;
    }

    public void BeginAiming(Vector3 fingerStartPosition) {
        aiming = true;
        lastFingerPosition = fingerStartPosition;
        DisableTutorialArrows();
    }

    public void UpdateAim(Vector3 newFingerPosition) {
        if (poking) return;
        float touchDistance = (newFingerPosition.x - lastFingerPosition.x) * touchDampValue;
        lastFingerPosition = newFingerPosition;
        rotationProportion = Mathf.Clamp(rotationProportion + touchDistance, 0, 1);
        float rotationAmount = Mathf.Lerp(rotationLimit, -rotationLimit, rotationProportion);
        transform.rotation = Quaternion.Euler(0, 0, rotationAmount);
    }

    public void PokeStick(Vector3 newFingerPosition) {
        aiming = false;
        if (Mathf.Abs(newFingerPosition.x - transform.position.x) > touchLimit) return;
        if (!poking && GameManager.gamePlaying) {
            StartCoroutine(StickPokingAnimation());
        }
    }

    private IEnumerator StickPokingAnimation() {
        stickCollider.enabled = true;
        transform.GetChild(0).gameObject.SetActive(false);
        poking = true;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(transform.up.x * pokeDistance, transform.up.y * pokeDistance,
                                                transform.position.z);
        float pokingTime = pokingCurve.keys[pokingCurve.length - 1].time;
        for(float t = 0; t < pokingTime * 2; t += Time.deltaTime * pokeMultiplier) {
            float proportion = pokingCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPosition, endPosition, proportion);
            yield return null;
        }
        transform.position = startPosition;
        poking = false;
        if (transform.childCount == maxFruits + 1)
        {
            CustomerController customer = GameManager.instance.VerifyFruit(gameObject.GetComponentsInChildren<PokeableFruit>());
            if (customer != null)
            {
                StartCoroutine(ServeCustomer(customer));
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        stickCollider.enabled = false;
        StickFinishedPoking();
    }

    private IEnumerator PierceFruit(float duration, Transform fruitTransform)
    {
        float initialSlowDown = duration * 0.1f;
        for(float t = 0; t < initialSlowDown; t += Time.deltaTime) {
            pokeMultiplier = Mathf.SmoothStep(1, 0, t / initialSlowDown);
            yield return null;
        }
        // WHY does this break stuff
        //yield return new WaitForSeconds(0.3f);
        float pushDistance = 0.1f;
        fruitTransform.parent = transform;
        for (int i = transform.childCount - 1; i > 0; i--) {
            Transform childFruit = transform.GetChild(i);
            Vector3 newFruitPosition = childFruit.localPosition;
            newFruitPosition.y = (StickEnd(true) - pushDistance);
            pushDistance += childFruit.lossyScale.y;
            StartCoroutine(SlideFruitOnStick(childFruit, childFruit.localPosition, newFruitPosition));
        }
        //yield return new WaitForSeconds(fruitSlidingTime);
        Vector3 squishVector = new Vector3(0.8f, 0.8f, 0.8f);
        for(float t = 0; t < fruitSlidingTime; t += Time.deltaTime) { 
            fruitTransform.localScale = Vector3.Lerp(Vector3.one, squishVector, t / fruitSlidingTime);
        }

        for(float t = 0; t < duration; t += Time.deltaTime) {
            pokeMultiplier = Mathf.SmoothStep(0, 1, t / duration);
            fruitTransform.localScale = Vector3.Lerp(squishVector, Vector3.one, t / duration);
            yield return null;
        }

    }

    private IEnumerator SlideFruitOnStick(Transform fruitTransform, Vector3 startPosition, Vector3 endPosition)
    {
        for(float t = 0; t < fruitSlidingTime; t += Time.deltaTime) {
            fruitTransform.localPosition = Vector3.Lerp(startPosition, endPosition, t / fruitSlidingTime);
            yield return null;
        }
        fruitTransform.localPosition = endPosition;
    }

    private IEnumerator ServeCustomer(CustomerController customer) {
        Vector3 targetPosition = customer.transform.position;
        Vector3 startPosition = transform.position;
        Vector3 targetScale = new Vector3(serveEndingScale, serveEndingScale, serveEndingScale);

        for(float t = 0; t < serveTime; t += Time.deltaTime) {
            float proportion = Mathf.SmoothStep(0, 1, t / serveTime);
            transform.position = Vector3.Lerp(startPosition, targetPosition, proportion);
            transform.localScale = Vector3.Lerp(Vector3.one, targetScale, proportion);
            yield return null;
        }

        stickCounter.RemoveStick();
        CustomerManager.instance.ServeCustomer(customer, GetFruits());
        if (GameManager.sticksRemaining > 0)
        {
            transform.position = startPosition;
            transform.localScale = Vector3.one;
            transform.GetChild(0).gameObject.SetActive(true);
            stickCollider.enabled = true;
        } else
        {
            transform.position = new Vector3(1000, 0);
        }
    }

    public bool AttachFruit(Transform newFruit) {
        if (transform.childCount == maxFruits + 1) return false;
        //newFruit.parent = transform;
        StartCoroutine(PierceFruit(0.15f, newFruit));
        return true;
    }

    private float StickEnd(bool local = false) {
        float yOffset = stickLength;
        if (local)
            return yOffset;
        else
            return transform.position.y + yOffset;
    }

    private void ClearStick(int ignore) {
        for (int i = 1; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public FruitType[] GetFruits() {
        PokeableFruit[] fruits = gameObject.GetComponentsInChildren<PokeableFruit>();
        FruitType[] fruitTypes = new FruitType[fruits.Length];

        for(int i = 0; i < fruits.Length; i++) {
            fruitTypes[i] = fruits[i].fruitType;
        }

        return fruitTypes;
    }

    void DisableTutorialArrows() { 
        foreach(TutorialArrow arrow in tutorialArrows) {
            arrow.DeactivateArrow();
        }
    }

    private void OnEnable()
    {
        CustomerManager.ScorePoints += ClearStick;
    }
    
    private void OnDisable()
    {
        CustomerManager.ScorePoints -= ClearStick;
    }
}
