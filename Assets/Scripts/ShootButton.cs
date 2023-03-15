using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootButton : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool CheckTouchPosition(Vector3 touchPosition) {
        Debug.Log(touchPosition);
        if(spriteRenderer.bounds.Contains(touchPosition)) {
            return true;
        }
        return false;
    }
}
