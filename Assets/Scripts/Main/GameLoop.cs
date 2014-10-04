using System.Linq;
using Assets.Scripts.Buildings;
using Assets.Scripts.DayNight;
using Assets.Scripts.Events;
using Assets.Scripts.Levels;
using Assets.Scripts.Players;
using Assets.Scripts.Production;
using Assets.Scripts.UnitActions;
using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = Assets.Scripts.Events.EventHandler;

namespace Assets.Scripts.Main
{
    // Main gameloop
    public class GameLoop : MonoBehaviour
    {
        public bool IsDoneButtonActive { get; set; }

        private Movement movement;
        private Highlight highlight;
        private CaptureBuildings capBuildings;
        private ProductionOverlayMain productionOverlay;
        private UnitActions.AnimationInfo animInfo;
        private DayStateController dayStateController;
        private LevelManager lm;

        private void Awake() 
        {
            lm = GameObjectReferences.GetGlobalScriptsGameObject().GetComponent<LevelManager>();

            string sceneName = Application.loadedLevelName;
            if (Application.isEditor && !sceneName.Equals(LevelsEnum.Menu.ToString()))
            {
                if (lm.CurrentLevel == null)
                {
                    foreach (LevelsEnum pl in (LevelsEnum[])Enum.GetValues(typeof(LevelsEnum)))
                    {
                        if (pl.ToString() == sceneName)
                        {
                            lm.LoadLevel(pl);
                            break;
                        }
                    }
                }
            }
            movement = GameObject.Find("_Scripts").GetComponent<Movement>();
            highlight = GameObject.Find("_Scripts").GetComponent<Highlight>();
            capBuildings = GameObject.Find("_Scripts").GetComponent<CaptureBuildings>();
            productionOverlay = GameObject.Find("_Scripts").GetComponent<ProductionOverlayMain>();
            dayStateController = GameObject.Find("_Scripts").GetComponent<DayStateController>();
            animInfo = GameObject.Find("_Scripts").GetComponent<Assets.Scripts.UnitActions.AnimationInfo>();
        }

        private void Update()
        {
            CheckObjectsClick();

            if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetMouseButtonDown(0) && lm.CurrentLevel.IsEnded))
            {
                lm.LoadLevel(LevelsEnum.Menu);
            }

            // Temporary code for debuging
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndTurn();
            }
        }

        /// <summary>
        /// Check if there has been a click. Ifso then raycast and check if there has been clicked on a specific game object. Ifso fire an event with the click object as a parameter.
        /// </summary>
        private void CheckObjectsClick()
        {
            // Gives errors
            // if (Input.GetMouseButtonDown(0) && !_manager.SwipeController.IsSwipeHappening)
            if (Input.GetMouseButtonDown(0))
            {
                var unitClick = new OnUnitClick();
                var buildingClick = new OnBuildingClick();
                var highlightClick = new OnHighlightClick();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    foreach (Unit unit in lm.CurrentLevel.CurrentPlayer.OwnedUnits)
                    {
                        if (unit.UnitGameObject.collider == hit.collider)
                        {
                            unitClick.unit = unit.UnitGameObject;
                            break;
                        }
                    }
                    foreach (Building building in lm.CurrentLevel.CurrentPlayer.OwnedBuildings)
                    {
                        if (building.BuildingGameObject.collider == hit.collider)
                        {
                            buildingClick.building = building.BuildingGameObject;
                            break;
                        }
                    }
                    foreach (HighlightObject highlightObj in highlight.HighlightObjects)
                    {
                        if (highlightObj.collider == hit.collider)
                        {
                            highlightClick.highlight = highlightObj;
                            break;
                        }
                    }
                }

                if (unitClick.unit == null && highlightClick.highlight == null && !movement.NeedsMoving ||
                    (highlight.IsHighlightOn && unitClick.unit != null))
                {
                    highlight.ClearHighlights();
                }
                EventHandler.dispatch(unitClick);
                EventHandler.dispatch(buildingClick);
                EventHandler.dispatch(highlightClick);
            }
        }

        public void EndTurn()
        {
            if (!animInfo.IsAnimateFight && !movement.NeedsMoving)
            {
                productionOverlay.DestroyAndStopOverlay();
                highlight.ClearMovementAndHighLights();
                capBuildings.CalculateCapturing();

                Player player = lm.CurrentLevel.CurrentPlayer;
                player.IncreaseGoldBy(player.GetCurrentIncome());

                // Change the currentplayer to the next player. Works with all amount of players. Ignores the Neutral player.
                bool foundPlayer = false;

                SortedList<PlayerIndex, Player> list = lm.CurrentLevel.Players;
                while (!foundPlayer)
                {
                    int indexplayer = list.IndexOfKey(player.Index) + 1;
                    if (indexplayer >= list.Count)
                    {
                        indexplayer = 0;
                    }
                    player = list.Values[indexplayer];
                    foundPlayer = player.Index != PlayerIndex.Neutral;
                }
                lm.CurrentLevel.CurrentPlayer = player;

                // After end turn we want to loop through loots and IncreaseTurn so that loot will destroy after x amount turns.
                FindObjectsOfType<Loot>().ToList().ForEach(x => x.IncreaseTurn());
                dayStateController.TurnIncrease();
            }
        }
    }
}