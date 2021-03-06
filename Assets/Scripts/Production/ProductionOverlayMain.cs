﻿using System.Linq;
using Assets.Scripts.Buildings;
using Assets.Scripts.Events;
using Assets.Scripts.FactoryPattern.ProductionOverlayFactory;
using UnityEngine;
using EventHandler = Assets.Scripts.Events.EventHandler;

namespace Assets.Scripts.Production
{
    public class ProductionOverlayMain : MonoBehaviour
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
            EventHandler.register<OnBuildingClick>(OnBuildingClick);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit touchBox;
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out touchBox);
                if (!NeedsMoving && BuildingClickedProduction != null && IsProductionOverlayActive &&
                    CurrentOverlay.GetComponentsInChildren<ProductionScript>().All(x => x.collider != touchBox.collider))
                {
                    InitiateMoving(true);
                }
            }
            PositionOverlay();
        }

        private void OnDestroy()
        {
            EventHandler.unregister<OnBuildingClick>(OnBuildingClick);
        }

        /// <summary>
        /// This method is executed when an clicked on building event is fired.
        /// </summary>
        /// <param Name="evt"></param>
        private void OnBuildingClick(OnBuildingClick evt)
        {
            if (evt.building != null)
            {
                if (!IsProductionOverlayActive && evt.building.BuildingGame.CanProduce)
                {
                    BuildingClickedProduction = evt.building;
                    CurrentOverlay = CreatorFactoryProductionOverlay.CreateProductionOverlay(BuildingClickedProduction.type);
                    CurrentOverlay.transform.position = GetBelowScreenPosition();
                    CurrentOverlay.transform.parent = Camera.main.transform;

                    CurrentOverlay.GetComponentsInChildren<ProductionScript>().ToList().ForEach(x=> x.ParentProduction = this);
                    InitiateMoving(false);
                    IsProductionOverlayActive = true;
                }
            }
        }

        public void InitiateMoving(bool endOfMoveDestroyed)
        {
            if (!IsProductionOverlayActive)
            {
                startPosition = GetBelowScreenPosition();
                targetPosition = GetAboveScreenPosition();
            }
            else
            {
                startPosition = GetAboveScreenPosition();
                targetPosition = GetBelowScreenPosition();
            }

            EndOfMovingDestroyed = endOfMoveDestroyed;
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
                    if (EndOfMovingDestroyed)
                    {
                        DestroyAndStopOverlay();
                    }
                    else
                    {
                        NeedsMoving = false;
                        CurrentOverlay.GetComponentsInChildren<ProductionScript>()
                            .ToList().ForEach(x => x.CanClick = true);
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
            return (Time.time - startTime)/duration;
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
            Destroy(CurrentOverlay);
            CurrentOverlay = null;
        }
    }
}