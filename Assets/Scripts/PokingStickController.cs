using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokingStickController : MonoBehaviour
{
    public float stickLength;
    public bool aiming;
    public bool poking;
    private Vector3 lastFingerPosition;
    public int maxFruits;

    public float rotationLimit;

    private float rotationProportion;

    private IEnumerator pokingCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        aiming = false;
        poking = false;
        rotationProportion = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckTouchPosition(Vector3 position)
    {
        return position.y < StickEnd(false);
    }

    public void BeginAiming(Vector3 fingerStartPosition) {
        aiming = true;
        lastFingerPosition = fingerStartPosition;
    }

    public void UpdateAim(Vector3 newFingerPosition) {
        float touchDistance = newFingerPosition.x - lastFingerPosition.x;
        lastFingerPosition = newFingerPosition;
        rotationProportion = Mathf.Clamp(rotationProportion + touchDistance, 0, 1);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(rotationLimit, -rotationLimit, rotationProportion));
    }

    public void PokeStick() {
        aiming = false;
        if (!poking) {
            StartCoroutine(StickPokingAnimation());
        }
    }

    private IEnumerator StickPokingAnimation() {
        poking = true;
        float stickPokeTime = 1;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.up.x * stickLength, transform.up.y * stickLength,
                                                transform.position.z);
        for(float t = 0; t < stickPokeTime; t += Time.deltaTime) {
            float proportion = Mathf.PingPong(t, stickPokeTime / 2);
            transform.position = Vector3.Lerp(startPosition, endPosition, proportion);
            yield return null;
        }
        transform.position = startPosition;
        poking = false;
        if(transform.childCount == maxFruits) {
            GameManager.instance.VerifyFruit(gameObject.GetComponentsInChildren<PokeableFruit>());
        }
    }

    public void AttachFruit(Transform newFruit) {
        newFruit.parent = transform;
        while (transform.childCount > maxFruits)
        {
            Transform oldFruit = transform.GetChild(0);
            oldFruit.parent = null;
            Destroy(oldFruit.gameObject);
        }
        float pushDistance = 0.1f;
        for (int i = transform.childCount - 1; i > -1; i--) {
            Transform childFruit = transform.GetChild(i);
            Vector3 newFruitPosition = newFruit.localPosition;
            newFruitPosition.x = 0;
            newFruitPosition.y = (StickEnd(true) - pushDistance);
            pushDistance += childFruit.lossyScale.y;
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

    private void ClearStick(int ignore) {
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
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
