using Assets.Scripts.Buildings;
using Assets.Scripts.FactoryPattern.UnitFactory;
using Assets.Scripts.Main;
using Assets.Scripts.Units;
using UnityEngine;
using Assets.Scripts.Notification;
using Assets.Scripts.Levels;

namespace Assets.Scripts.Production
{
    /// <summary>
    /// This script is put on the childs of the production overlay prefabs. So on each of the gameobjects which contain the sprites.
    /// </summary>
    public class ProductionScript : MonoBehaviour
    {

        // Define the type of unit.
        public UnitTypes type;
        public ProductionOverlayMain ParentProduction { get; set; }
        public bool CanClick { get; set; }

        private void Update()
        {

            if (CanClick && Input.GetMouseButtonDown(0) && ParentProduction.IsProductionOverlayActive &&
                !ParentProduction.BuildingClickedProduction.Tile.HasUnit())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit touchBox;
                if (Physics.Raycast(ray, out touchBox))
                {
                    if (touchBox.collider == this.collider)
                    {
                        BuildingGameObject buildingToProduceFrom = ParentProduction.BuildingClickedProduction;
                        // Kind of ugly yet could not find better solution. The unit is created before we check if it can be bought.
                        // Set it inactive immediatly and then check for enough Gold. If not then destroy else decrease the Gold and set it active.
                        UnitGameObject unit = CreatorFactoryUnit.CreateUnit(buildingToProduceFrom.Tile,
                            buildingToProduceFrom.index, type);
                        unit.gameObject.SetActive(false);
                        if (LevelManager.CurrentLevel.CurrentPlayer.CanBuy(unit.UnitGame.Cost))
                        {
                            unit.gameObject.SetActive(true);
                            LevelManager.CurrentLevel.CurrentPlayer.DecreaseGoldBy(unit.UnitGame.Cost);
                            CanClick = false;
                            ParentProduction.InitiateMoving(true);
                        }
                        else
                        {
                            Notificator.Notify("Not enough gold!", 1.5f);
                            unit.DestroyUnit();
                        }
                    }
                }
            }
        }
    }
}