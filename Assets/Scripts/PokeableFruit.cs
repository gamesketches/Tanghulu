using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeableFruit : MonoBehaviour
{
    public FruitType fruitType;

    public float appearTime = 0.4f;

    public float scaleAmount = 0.1f;
    public float scaleSpeed;
    public float timeOffset;

    bool floating = false;

    ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(floating) {
            transform.localScale = Vector3.one + 
                (Mathf.Sin((timeOffset + Time.time) * scaleSpeed) * new Vector3(scaleAmount, scaleAmount, scaleAmount));
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Stick")) {
            if (other.GetComponent<PokingStickController>().AttachFruit(transform))
            {
                transform.localScale = Vector3.one;
                floating = false;
                particles.Play();
                SFXManager.instance.PlaySoundEffect(SoundEffectType.PokeFruit);
            }
            else {
                StartCoroutine(KnockFruitOff());
            }
        }
    }

    public void Appear(float delay = 0) {
        StartCoroutine(BobInFruit(delay));
        timeOffset = Random.value;
    }

    IEnumerator BobInFruit(float delay = 0)
    {
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(delay);
        for (float t = 0; t < appearTime; t += Time.deltaTime)
        {
            float proportion = Mathf.SmoothStep(0, 1, t / appearTime);
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, proportion);
            yield return null;
        }
        transform.localScale = Vector3.one;
        floating = true;
    }

    IEnumerator KnockFruitOff() { 
        for(float t = 0; t < appearTime; t += Time.deltaTime) {
            transform.Translate(10 * Time.deltaTime, 10 * Time.deltaTime, 0, Space.World);
            yield return null;
        }

        Destroy(gameObject);
    }
}
