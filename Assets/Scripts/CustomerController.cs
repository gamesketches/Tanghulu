using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreType { Bad, Misplaced, Correct};
public class CustomerController : MonoBehaviour
{
    public OrderBubble customerBubble;

    public FruitType[] order;
    CustomerSpritePack spritePack;
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

    public void Initialize(FruitType[] customerOrder, CustomerSpritePack customerSprites, Vector3 positionInLine) {
        customerBubble.SetFruits(customerOrder);
        order = customerOrder;
        StartCoroutine(WalkOnScreen(positionInLine));
        spritePack = customerSprites;
        spriteRenderer.sprite = customerSprites.normalSprite;
    }

    public void Leave() {
        StartCoroutine(WalkOffScreen());
    }

    private IEnumerator WalkOnScreen(Vector3 positionInLine, bool openBubble = true)
    {
        Vector3 startPos = transform.position;
        StartCoroutine(SFXManager.instance.PlayFootstep(walkOnTime));
        for(float t = 0; t <= walkOnTime; t += Time.deltaTime) {
            Vector3 newPos = Vector3.Lerp(startPos, positionInLine, t / walkOnTime);
            newPos.y += Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed) * bounceRange);
            transform.position = newPos;
            yield return null;
        }
        if(openBubble) 
            StartCoroutine(OpenBubble());

        transform.position = positionInLine;
    }

    public void MoveToSpotInLine(Vector3 positionInLine) {
        StartCoroutine(WalkOnScreen(positionInLine, false));
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
        customerBubble.ShowFruits();
    }

    public IEnumerator ShowSatisfaction(FruitType[] preparedOrder) {
        yield return customerBubble.ServeAnimation(CalculateSatisfactionReport(preparedOrder));
        int score = CalculateSatisfaction(preparedOrder);
        UpdateSprite(score);
    }

    public void GetDismissed() {
        StartCoroutine(WalkOffScreen());
    }

    private IEnumerator WalkOffScreen() {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position - new Vector3(Camera.main.orthographicSize, 0, 0);
        customerBubble.gameObject.SetActive(false);
        StartCoroutine(SFXManager.instance.PlayFootstep(walkOnTime / 2));
        for(float t = 0; t <= walkOnTime; t += Time.deltaTime) {
            Vector3 newPos = Vector3.Lerp(startPos, endPos, t / walkOnTime);
            newPos.y += Mathf.Sin(Time.time * bounceSpeed) * bounceRange;
            transform.position = newPos; 
            yield return null;
        }
        transform.position = transform.position + new Vector3(Camera.main.orthographicSize * 3, 0, 0);
        gameObject.SetActive(false);
    }

    public void UpdateSprite(int score) { 
        if(score > 6) {
            spriteRenderer.sprite = spritePack.excitedSprite;
        } else if(score > 4) {
            spriteRenderer.sprite = spritePack.happySprite;
        } else if(score > 2) {
            spriteRenderer.sprite = spritePack.normalSprite;
        } else {
            spriteRenderer.sprite = spritePack.sadSprite;
        }
    }
    
    public int CalculateSatisfaction(FruitType[] preparedOrder) {
        int score = 0;
        
        ScoreType[] scoreReport = CalculateSatisfactionReport(preparedOrder);
        foreach(ScoreType fruitScore in scoreReport) { 
            switch(fruitScore) {
                case ScoreType.Correct:
                    score += 2;
                    break;
                case ScoreType.Misplaced:
                    score += 1;
                    break;
                case ScoreType.Bad:
                    score += 0;
                    break;
            }
        }
        
        return score;
    }

    public ScoreType[] CalculateSatisfactionReport(FruitType[] preparedOrder) {
        ScoreType[] scoreReport = new ScoreType[4];
        List<FruitType> requestedOrder = new List<FruitType>(order);
        for(int i = 0; i < GameManager.orderSize; i++) {
            if (requestedOrder[i] == preparedOrder[i]) {
                scoreReport[i] = ScoreType.Correct;
            }
            else if (requestedOrder.IndexOf(preparedOrder[i]) > -1) {
                scoreReport[i] = ScoreType.Misplaced;
            }
            else {
                scoreReport[i] = ScoreType.Bad;
            }
        }
        return scoreReport;
    }

}
