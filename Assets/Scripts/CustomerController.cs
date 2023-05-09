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

    [Header("Bounce tuning variables")]
    public Vector2 bounceTimeRange;
    public Vector2 bounceWaitTime;
    IEnumerator bounceRoutine;

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
        //StartCoroutine(SFXManager.instance.PlayFootstep(walkOnTime));
        for(float t = 0; t <= walkOnTime; t += Time.deltaTime) {
            Vector3 newPos = Vector3.Lerp(startPos, positionInLine, t / walkOnTime);
            newPos.y += Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed) * bounceRange);
            transform.position = newPos;
            yield return null;
        }
        if(openBubble) 
            StartCoroutine(OpenBubble());

        transform.position = positionInLine;
        bounceRoutine = BounceExcitedly();
        StartCoroutine(bounceRoutine);
    }

    private IEnumerator BounceExcitedly() {
        float waitTime = Random.Range(bounceWaitTime.x, bounceWaitTime.y);
        float bouncingTime = Random.Range(bounceTimeRange.x, bounceTimeRange.y);
        Vector3 startPos = transform.position;
        float seed = Random.Range(0.75f, 0.82f);
        while(true) {
            yield return new WaitForSeconds(waitTime);
            for(float t = 0; t < bouncingTime; t += Time.deltaTime) {
                Vector3 newPos = startPos + new Vector3(0,Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed * seed) * bounceRange), 0);
                transform.position = newPos;
                yield return null;
            }
            transform.position = startPos;
        }
    }

    public void MoveToSpotInLine(Vector3 positionInLine) {
        StopCoroutine(bounceRoutine);
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
        StopCoroutine(bounceRoutine);
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
        //StartCoroutine(SFXManager.instance.PlayFootstep(walkOnTime / 2));
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
        // NOTE: These do not factor in score multiplier
        if(score > 7) {
            spriteRenderer.sprite = spritePack.excitedSprite;
            SFXManager.instance.PlaySoundEffect(SoundEffectType.Perfect);
        } else if(score > 5) {
            spriteRenderer.sprite = spritePack.happySprite;
            SFXManager.instance.PlaySoundEffect(SoundEffectType.GoodResult);
        } else if(score > 3) {
            spriteRenderer.sprite = spritePack.normalSprite;
            SFXManager.instance.PlaySoundEffect(SoundEffectType.OkayResult);
        } else {
            spriteRenderer.sprite = spritePack.sadSprite;
            SFXManager.instance.PlaySoundEffect(SoundEffectType.TerribleResult);
        }
    }
    
    public int CalculateSatisfaction(FruitType[] preparedOrder) {
        int score = 0;
        
        ScoreType[] scoreReport = CalculateSatisfactionReport(preparedOrder);
        foreach(ScoreType fruitScore in scoreReport) { 
            switch(fruitScore) {
                case ScoreType.Correct:
                    score += GameManager.CorrectFruitValue;
                    break;
                case ScoreType.Misplaced:
                    score += GameManager.MisplacedFruitValue;
                    break;
                case ScoreType.Bad:
                    score += GameManager.IncorrectFruitValue;
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
