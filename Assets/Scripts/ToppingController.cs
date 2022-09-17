using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToppingController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ToppingType toppingType;
    private SpriteRenderer spriteRenderer;
    Vector3 startPosition;
    public StickController stick;

    public float distanceToPoke;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        AssignSprite();
    }

    public void UpdateFruitType(ToppingType newType) {
        toppingType = newType;
        AssignSprite();
    }

    public void ToggleRenderer(bool mode)
    {
        spriteRenderer.enabled = mode;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        transform.position = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Vector3.Distance(transform.position, stick.transform.position) < distanceToPoke)
            stick.AddFruit(toppingType);
        transform.position = startPosition;
    }

    void AssignSprite()
    {
        switch (toppingType)
        {
            case ToppingType.BLUE:
                spriteRenderer.color = Color.blue;
                break;
            case ToppingType.RED:
                spriteRenderer.color = Color.red;
                break;
            case ToppingType.GREEN:
                spriteRenderer.color = Color.green;
                break;
            case ToppingType.ORANGE:
                spriteRenderer.color = new Color(1, 0.5f, 0);
                break;
            case ToppingType.PURPLE:
                spriteRenderer.color = Color.magenta;
                break;
        }
    }
}
