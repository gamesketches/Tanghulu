using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PotController potController;
    public PokingStickController pokingStick;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount > 0) ProcessTouches();
        if (Input.GetMouseButton(0)) ProcessClicks();
    }

    void ProcessClicks() {
        Vector3 worldPosition = GetWorldPosition(Input.mousePosition);
        Ray touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if(pokingStick.CheckTouchPosition(worldPosition)) {
                pokingStick.PokeStick();
            }
            else if (potController.CheckTouchPosition(worldPosition))
            {
                potController.UpdateDragging(true);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (potController.CheckTouchPosition(worldPosition))
            {
                potController.NewDragPosition(worldPosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            potController.UpdateDragging(false);
        }
    }

    void ProcessTouches() {
        Touch mainTouch = Input.touches[0];

        Vector3 worldPosition = GetWorldPosition(mainTouch.position);
        switch(mainTouch.phase) {
            case TouchPhase.Began:
                if(potController.CheckTouchPosition(worldPosition)) {
                    potController.UpdateDragging(true);
                }
                break;
            case TouchPhase.Moved:
                if(potController.CheckTouchPosition(worldPosition)) {
                    potController.NewDragPosition(worldPosition);
                }
                break;
            case TouchPhase.Ended:
                potController.UpdateDragging(false);
                break;
        }
    }

    Vector3 GetWorldPosition(Vector2 screenPosition) {
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
