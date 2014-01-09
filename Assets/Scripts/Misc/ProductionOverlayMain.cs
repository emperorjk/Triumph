using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

public class ProductionOverlayMain
{
    /// <summary>
    /// The building variable used for the production of units.
    /// </summary>
    public BuildingGameObject BuildingClickedProduction { get; set; }
    public GameObject CurrentOverlay { get; set; }
    public bool IsProductionOverlayActive { get; set; }
    public bool NeedsMoving { get; set; }
    public bool EndOfMovingDestroyed { get; set; }
    private Vector2 targetPosition;
    private Vector2 startPosition;
    private float startTime;
    private float duration = 0.5f;
    
    public ProductionOverlayMain()
    {
        NeedsMoving = false;
    }

    /// <summary>
    /// This method is called from inside the gameloop.
    /// </summary>
    public void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool foundBuilding = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit touchBox;
            if (Physics.Raycast(ray, out touchBox) && !IsProductionOverlayActive)
            {
                // Go through all of the buildings that can produce units.
                foreach (BuildingsBase building in GameManager.Instance.CurrentPlayer.ownedBuildings.Where(x => x.CanProduce))
                {
                    if (touchBox.collider == building.buildingGameObject.collider)
                    {
                        foundBuilding = true;
                        BuildingClickedProduction = building.buildingGameObject;
                        CurrentOverlay = CreatorFactoryProductionOverlay.CreateProductionOverlay(BuildingClickedProduction.type);
                        // Add the overlay as a child to the camera so when camera moving is implemented it follows along.
                        CurrentOverlay.transform.parent = Camera.main.transform;
                        foreach (ProductionScript item in CurrentOverlay.GetComponentsInChildren<ProductionScript>()) { item.parentProduction = this; }
                        InitiateMoving(false);
                        IsProductionOverlayActive = true;
                        break;
                    }
                }
            }

            if (!NeedsMoving && !foundBuilding && IsProductionOverlayActive && !CurrentOverlay.GetComponentsInChildren<ProductionScript>().Any(x => x.collider == touchBox.collider))
            {
                // If there was not clicked on an building. The overlay is already active and there was not clicked on any overlay that was active destroy the overlay.
                //DestroyAndStopOverlay();
                InitiateMoving(true);
            }
        }
        PositionOverlay();
    }

    public void InitiateMoving(bool EndOfMoveDestroyed)
    {
        
        if(!IsProductionOverlayActive)
        {
            startPosition = GetBelowScreenPosition();
            targetPosition = GetAboveScreenPosition();
        }
        else
        {
            startPosition = GetAboveScreenPosition();
            targetPosition = GetBelowScreenPosition();
        }

        this.EndOfMovingDestroyed = EndOfMoveDestroyed;
        NeedsMoving = true;
        startTime = Time.time;
    }

    // Screen space is as follows: bottom left is 0,0 top right is Screen.width || Camera.main.pixelWidth and Screen.height || Camera.main.pixelHeight.
    // The center is calculated from the sceen space. Along the 0 y point.
    // Then we translate that point to world space which returns the point in the center of the screen in world space.
    // I think because of the sprite size (pixels to units setting) we need to add or substract exactly one.
    
    // Viewportspace bottom left is 0,0 and top right is 1,1. I think this is a better thing to use since what i read it is resolution / pixel independent.

    /// <summary>
    /// Calculates and sets the start position for the overlay to move from.
    /// </summary>
    private Vector2 GetBelowScreenPosition()
    {
        Vector3 centerWorldSpace = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 0));
        //Vector3 centerWorldSpace = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
        return new Vector2(centerWorldSpace.x, centerWorldSpace.y - 1);
    }

    /// <summary>
    /// Calculates and sets the target position for the overlay to move to.
    /// </summary>
    private Vector2 GetAboveScreenPosition()
    {
        Vector3 centerWorldSpace = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 0));
        //Vector3 centerWorldSpace = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
        return new Vector2(centerWorldSpace.x, centerWorldSpace.y + 1);
    }

    /// <summary>
    /// Move the overlay from the bottom of the screen to just above.
    /// </summary>
    public void PositionOverlay()
    {
        if (NeedsMoving)
        {
            float time = GetTimePassed();
            CurrentOverlay.transform.position = Vector2.Lerp(startPosition, targetPosition, time);
            
            if (time >= 1f)
            {
                if(EndOfMovingDestroyed)
                {
                    DestroyAndStopOverlay();
                }
                else
                {
                    NeedsMoving = false;
                    foreach (ProductionScript item in CurrentOverlay.GetComponentsInChildren<ProductionScript>()) { item.CanClick = true; }
                }
            }
        }
    }

    /// <summary>
    /// Get the time that has passed. This is used for the movement of the overlay.
    /// </summary>
    /// <returns></returns>
    private float GetTimePassed()
    {
        return (Time.time - startTime) / duration;
    }

    /// <summary>
    /// Destroys the overlay and resets some properties.
    /// </summary>
    public void DestroyAndStopOverlay()
    {
        EndOfMovingDestroyed = false;
        BuildingClickedProduction = null;
        IsProductionOverlayActive = false;
        NeedsMoving = false;
        GameObject.Destroy(CurrentOverlay);
        CurrentOverlay = null;
    }
}
