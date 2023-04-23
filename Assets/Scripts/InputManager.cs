using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PotController potController;
    public PokingStickController pokingStick;
    public ShootButton shootButton;

    int potControllingId = -1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //if (Input.touchCount > 0) ProcessTouches();
        ProcessClicks();
#else
        ProcessTouches();
#endif
    }

    void ProcessClicks() {
        Vector3 worldPosition = GetWorldPosition(Input.mousePosition);
        Ray touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (pokingStick.CheckTouchPosition(worldPosition))
            {
                pokingStick.BeginAiming(worldPosition);
            }
            else if (shootButton.CheckTouchPosition(worldPosition))
            {
                pokingStick.PokeStick(worldPosition);
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
                // pokingStick.PokeStick(worldPosition);
                pokingStick.StopAiming();
            }
            else 
                potController.UpdateDragging(false);
        }
    }

    void ProcessTouches() {
        //Touch mainTouch = Input.touches[0];

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch theTouch = Input.touches[i];
            Vector3 worldPosition = GetWorldPosition(theTouch.position);
            switch (theTouch.phase)
            {
                case TouchPhase.Began:
                    if(pokingStick.CheckTouchPosition(worldPosition)) {
                        pokingStick.BeginAiming(worldPosition);
                    }
                    else if(shootButton.CheckTouchPosition(worldPosition)) {
                        pokingStick.PokeStick(worldPosition);
                    }
                    else if (potController.CheckTouchPosition(worldPosition))
                    {
                        potController.UpdateDragging(true);
                        potControllingId = theTouch.fingerId;
                    }
                    break;
                case TouchPhase.Moved:
                    if(theTouch.fingerId != potControllingId && pokingStick.CheckTouchPosition(worldPosition)) {
                        pokingStick.UpdateAim(worldPosition);
                    }
                    else if (theTouch.fingerId == potControllingId && potController.CheckTouchPosition(worldPosition))
                    {
                        potController.NewDragPosition(worldPosition);
                    }
                    break;
                case TouchPhase.Ended:
                    if (theTouch.fingerId != potControllingId && pokingStick.CheckTouchPosition(worldPosition) && pokingStick.aiming)
                    {
                        //pokingStick.PokeStick(worldPosition);
                    }
                    else if (theTouch.fingerId == potControllingId || potController.CheckTouchPosition(worldPosition)) {
                        potController.UpdateDragging(false);
                        potControllingId = -1;
                    }
                    break;
            }
        }
    }

    Vector3 GetWorldPosition(Vector2 screenPosition) {
        Vector3 position = Camera.main.ScreenToWorldPoint(screenPosition);
        position.z = 0;
        return position;
    }
}
