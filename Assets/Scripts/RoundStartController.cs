using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoundStartController : MonoBehaviour
{
    public Sprite goSprite;
    public float readyTime = 0.5f;
    public float goTime = 0.3f;
    Image displayImage;

    // Start is called before the first frame update
    void Start()
    {
        displayImage = GetComponent<Image>();
    }

    public IEnumerator DoRoundStart() {
        yield return new WaitForSeconds(readyTime);
        displayImage.sprite = goSprite;
        yield return new WaitForSeconds(goTime);
        displayImage.enabled = false;
    }
}
