using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickCounter : MonoBehaviour
{
    public GameObject stickCounter;
    public Vector2 firstStickPos;
    public float stickOffset;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameManager.sticksRemaining);
        for (int i = 0; i < GameManager.sticksRemaining; i++) {
            GameObject newStick = GameObject.Instantiate(stickCounter, transform);
            newStick.GetComponent<RectTransform>().anchoredPosition = firstStickPos + new Vector2(stickOffset * i, 0);
         }
    }

    public void RemoveStick() {
        // Note to self: there's some complicated off by one stuff here
        transform.GetChild(GameManager.sticksRemaining).GetComponent<Image>().enabled = false;
    }

}
