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
        ProcessClicks();
    }

    void ProcessClicks() {
        Vector3 worldPosition = GetWorldPosition(Input.mousePosition);
        Ray touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if(pokingStick.CheckTouchPosition(worldPosition)) {
                pokingStick.BeginAiming(worldPosition);
            }
            else if (potController.CheckTouchPosition(worldPosition))
            {
                potController.UpdateDragging(true);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if(pokingStick.aiming) {
                pokingStick.UpdateAim(worldPosition);
            }
            else if (potController.CheckTouchPosition(worldPosition))
            {
                potController.NewDragPosition(worldPosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(pokingStick.aiming) {
                Debug.Log("mouse up");
                pokingStick.PokeStick();
            }
            else 
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
        Vector3 position = Camera.main.ScreenToWorldPoint(screenPosition);
        position.z = 0;
        return position;
    }
}
