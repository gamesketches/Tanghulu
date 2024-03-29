﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokingStickController : MonoBehaviour
{
    public float stickLength;
    public float pokeDistance;

    [Header("Serving variables")]
    public float serveTime;
    public float serveEndingScale;
    public float stickTouchHeight;
    public float servingOffset;
    public float servingHoldTime;

    [HideInInspector]
    public bool aiming;
    [HideInInspector]
    public bool poking;
    BoxCollider2D stickCollider;
    private Vector3 lastFingerPosition;
    public int maxFruits;

    [Header("Feel variables")]
    public float rotationLimit;
    public float touchLimit;
    public float touchDampValue;
    public AnimationCurve pokingCurve;
    public float fruitSlidingTime;
    public float fruitHitStop;
    public float fruitSquishSize;
    public float pokeAnimationLength;

    List<PokeableFruit> pokedFruits;
    private float rotationProportion;

    float pokeMultiplier;

    int pokeCombo;

    [Header("Hud stuff")]
    public StickCounter stickCounter;

    public FruitHUD fruitHUD;

    public MultiplierDisplay multiplierDisplay;

    public static int fruitsPoked;

    public TutorialArrow[] tutorialArrows;
    public float arrowDisableTreshold;

    public delegate void StickPoked();
    public static event StickPoked StickFinishedPoking;

    // Start is called before the first frame update
    void Start()
    {
        aiming = false;
        poking = false;
        pokeMultiplier = 1;
        rotationProportion = 0.5f;
        fruitsPoked = 0;
        pokingCurve.postWrapMode = WrapMode.PingPong;
        stickCollider = GetComponent<BoxCollider2D>();
        stickCollider.enabled = false;
        pokedFruits = new List<PokeableFruit>();
    }

    public bool CheckTouchPosition(Vector3 position)
    {
        return position.y < stickTouchHeight && Mathf.Abs(position.x) < 2;
    }

    public void BeginAiming(Vector3 fingerStartPosition) {
        aiming = true;
        lastFingerPosition = fingerStartPosition;
        //SFXManager.instance.PlaySoundEffect(SoundEffectType.AimStick);
    }

    public void UpdateAim(Vector3 newFingerPosition) {
        if (poking) return;
        float touchDistance = (newFingerPosition.x - lastFingerPosition.x) * touchDampValue;

        float rawDistance = Mathf.Abs(newFingerPosition.x - lastFingerPosition.x);
        if (Mathf.Approximately(rawDistance, 0)) SFXManager.instance.StopAimSound();
        else SFXManager.instance.AddToAimDistance(rawDistance);
            //SFXManager.instance.PlaySoundEffect(SoundEffectType.AimStick);

        lastFingerPosition = newFingerPosition;
        rotationProportion = Mathf.Clamp(rotationProportion + touchDistance, 0, 1);
        if (Mathf.Abs(rotationProportion - 0.5f) > arrowDisableTreshold)
        {
            DisableTutorialArrows();
        }
        float rotationAmount = Mathf.Lerp(rotationLimit, -rotationLimit, rotationProportion);
        transform.rotation = Quaternion.Euler(0, 0, rotationAmount);
    }

    public void PokeStick(Vector3 newFingerPosition) {
        aiming = false;
        //if (Mathf.Abs(newFingerPosition.x - transform.position.x) > touchLimit) return;
        if (!poking && GameManager.gamePlaying) {
            StartCoroutine(StickPokingAnimation());
            SFXManager.instance.PlaySoundEffect(SoundEffectType.PokeStick);
        }
    }

    public void StopAiming() {
        aiming = false;
        //SFXManager.instance.PlaySoundEffect(SoundEffectType.AimStickEnd);
    }

    private IEnumerator StickPokingAnimation() {
        stickCollider.enabled = true;
        transform.GetChild(0).gameObject.SetActive(false);
        poking = true;
        pokeCombo = 0;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(transform.up.x * pokeDistance, transform.up.y * pokeDistance,
                                                transform.position.z);
        float pokingTime = pokingCurve.keys[pokingCurve.length - 1].time;
        for(float t = 0; t < pokingTime; t += Time.deltaTime * pokeMultiplier) {
            float proportion = pokingCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPosition, endPosition, proportion);
            yield return null;
        }
        stickCollider.enabled = false;
        if (pokedFruits.Count == 0)
            SFXManager.instance.PlaySoundEffect(SoundEffectType.Miss);
        for(float t = pokingTime; t > 0; t -= (Time.deltaTime * pokeMultiplier * 1.35f)) {
            float proportion = pokingCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPosition, endPosition, proportion);
            yield return null;
        }
        transform.position = startPosition;
        if (pokedFruits.Count == maxFruits)
        {
            CustomerController customer = GameManager.instance.VerifyFruit(GetFruits());
            if (customer != null)
            {
                StartCoroutine(ServeCustomer(customer));
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            poking = false;
        }
        StickFinishedPoking();
    }

    public bool AttachFruit(Transform newFruit) {
        PokeableFruit fruitObject = newFruit.GetComponent<PokeableFruit>();
        if (pokedFruits.Count >= maxFruits || pokedFruits.IndexOf(fruitObject) > -1) return false;
        StartCoroutine(PierceFruit(pokeAnimationLength, newFruit, fruitObject));
        pokedFruits.Add(fruitObject);
        fruitsPoked++;
        return true;
    }

    private IEnumerator PierceFruit(float duration, Transform fruitTransform, PokeableFruit fruitObject)
    {
        float initialSlowDown = fruitObject.pokeSlowdown;
        float pushDistance = 0.15f;

        Vector3 startingFruitPos = fruitTransform.position;
        Vector3 offsetPosition = fruitTransform.position + (transform.up * pushDistance *3);
        for(float t = 0; t < initialSlowDown; t += Time.deltaTime) {
            pokeMultiplier = Mathf.SmoothStep(1, 0, t / initialSlowDown);
            float fruitTransformationProportion = Mathf.PingPong(1 - pokeMultiplier, 0.5f);
            fruitTransform.position = Vector3.Lerp(startingFruitPos, offsetPosition, fruitTransformationProportion);
            //fruitTransform.position = Vector3.Lerp(offsetPosition, startingFruitPos, pokeMultiplier);
            yield return null;
        }
        pokeMultiplier = 0;
        SFXManager.instance.PlaySoundEffect(SoundEffectType.PokeFruit);

        yield return new WaitForSeconds(fruitObject.hitStop);
        fruitTransform.parent = transform;
        for (int i = transform.childCount - 1; i > 0; i--) {
            Transform childFruit = transform.GetChild(i);
            Vector3 newFruitPosition = childFruit.localPosition;
            newFruitPosition.y = (StickEnd(true) - pushDistance);
            pushDistance += childFruit.lossyScale.y;
            StartCoroutine(SlideFruitOnStick(childFruit, childFruit.localPosition, newFruitPosition, duration * 0.6f));// initialSlowDown * 2.7f));//
        }
        Vector3 squishVector = new Vector3(fruitObject.squishSize, fruitObject.squishSize, fruitObject.squishSize);
        /*for(float t = 0; t < fruitSlidingTime; t += Time.deltaTime) { 
            fruitTransform.localScale = Vector3.Lerp(Vector3.one, squishVector, t / fruitSlidingTime);
        }*/

        fruitObject.PlayParticles();
        for(float t = 0; t < duration; t += Time.deltaTime) {
            float proportion = Mathf.PingPong(t, duration / 2) / (duration / 2);
            pokeMultiplier = Mathf.SmoothStep(0, 1, proportion); 
            fruitTransform.localScale = Vector3.Lerp(Vector3.one, squishVector, proportion);
            //fruitTransform.localScale = Vector3.Lerp(squishVector, Vector3.one, t / duration);
            yield return null;
        }
        fruitTransform.localScale = Vector3.one;
        pokeMultiplier = 0;
        yield return new WaitForSeconds(initialSlowDown);
        for(float t = 0; t < initialSlowDown; t += Time.deltaTime) {
            pokeMultiplier = Mathf.SmoothStep(0, 1, t / initialSlowDown);
            yield return null;
        }
        pokeMultiplier = 1;
        fruitHUD.UpdateDisplay(GetFruits());
        multiplierDisplay.UpdateScoreMultiplier(++pokeCombo);
    }

    private IEnumerator SlideFruitOnStick(Transform fruitTransform, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        for(float t = 0; t < duration; t += Time.deltaTime) {
            fruitTransform.localPosition = Vector3.Lerp(startPosition, endPosition, t / duration);
            yield return null;
        }
        fruitTransform.localPosition = endPosition;
    }

    private IEnumerator ServeCustomer(CustomerController customer) {
        Vector3 targetPosition = customer.transform.position - new Vector3(0, servingOffset, 0);
        Vector3 startPosition = transform.position;
        Vector3 targetScale = new Vector3(serveEndingScale, serveEndingScale, serveEndingScale);
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, -90f);

        for(float t = 0; t < serveTime; t += Time.deltaTime) {
            float proportion = Mathf.SmoothStep(0, 1, t / serveTime);
            transform.position = Vector3.Lerp(startPosition, targetPosition, proportion);
            transform.localScale = Vector3.Lerp(Vector3.one, targetScale, proportion);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, proportion);
            yield return null;
        }
        
        stickCounter.RemoveStick();
        yield return customer.ShowSatisfaction(GetFruits());
        yield return new WaitForSeconds(servingHoldTime);
        CustomerManager.instance.ServeCustomer(customer, GetFruits());
        if (GameManager.sticksRemaining > 0)
        {
            transform.position = startPosition;
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;
            transform.GetChild(0).gameObject.SetActive(true);
            stickCollider.enabled = true;
            fruitHUD.ClearDisplay();
            pokedFruits.Clear();
            fruitsPoked = 0;
            poking = false;
        } else
        {
            transform.position = new Vector3(1000, 0);
        }
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
        FruitType[] fruitTypes = new FruitType[pokedFruits.Count];

        for(int i = 0; i < pokedFruits.Count; i++) { 
        //for(int i = 0; i < fruits.Length; i++) {
            fruitTypes[i] = pokedFruits[i].fruitType;
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
