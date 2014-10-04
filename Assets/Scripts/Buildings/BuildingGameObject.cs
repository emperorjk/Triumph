using Assets.Scripts.Levels;
using Assets.Scripts.Main;
using Assets.Scripts.Players;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    /// <summary>
    /// This script is put on a prefab and creates the appropiate BuildingBase object. It also passes itself to the BuildingBase so they can both reach each other via a variable.
    /// And adds the BuildingBase object the the correct player list of owned buildings.
    /// </summary>
    public class BuildingGameObject : MonoBehaviour
    {
        public PlayerIndex index;
        public BuildingTypes type;
        public Building BuildingGame { get; private set; }
        public Tile Tile { get; set; }
        public GameObject CapturePointsText { get; private set; }

        private void Awake()
        {
            BuildingGame = GameJsonCreator.CreateBuilding(this, type);
            if (transform.parent != null)
            {
                Tile = transform.parent.GetComponent<Tile>();
                Tile.buildingGameObject = this;
            }
            CapturePointsText = transform.FindChild("CapturePoints").gameObject;
            CapturePointsText.renderer.enabled = false;
            // Set the sorting layer to GUI. The same used for the hightlights. Eventough you cannot set it via unity inspector you can still set it via code. :D
            CapturePointsText.renderer.sortingLayerName = "GUI";

            var levelManager = GameObjectReferences.GetGlobalScriptsGameObject().GetComponent<LevelManager>();
            if (levelManager.IsCurrentLevelLoaded())
            {
                levelManager.CurrentLevel.Players[index].AddBuilding(BuildingGame);
            }
            
        }

        public void UpdateCapturePointsText()
        {
            var text = CapturePointsText.GetComponent<TextMesh>();
            text.text = ((int) BuildingGame.CurrentCapturePoints) + "/" + ((int) BuildingGame.CapturePoints);
            CapturePointsText.renderer.enabled = (!Tile.IsFogShown && BuildingGame.CurrentCapturePoints > 0);
        }

        public void DestroyBuilding()
        {
            Tile.buildingGameObject = null;
            Tile = null;
            var levelmanager = GameObjectReferences.GetGlobalScriptsGameObject().GetComponent<LevelManager>();
            levelmanager.CurrentLevel.Players[index].RemoveBuilding(BuildingGame);

            var capBuilding = GameObjectReferences.GetScriptsGameObject().GetComponent<CaptureBuildings>();
            if (capBuilding.BuildingsBeingCaptured.Contains(BuildingGame))
            {
                capBuilding.BuildingsBeingCaptured.Remove(BuildingGame);
            }

            Destroy(gameObject);
        }
    }
}