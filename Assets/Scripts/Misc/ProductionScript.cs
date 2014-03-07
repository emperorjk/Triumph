using UnityEngine;
using System.Collections;

/// <summary>
/// This script is put on the childs of the production overlay prefabs. So on each of the gameobjects which contain the sprites.
/// </summary>
public class ProductionScript : MonoBehaviour {

    // Define the type of unit.
    public UnitTypes type;
    public ProductionOverlayMain parentProduction { get; set; }
    public bool CanClick { get; set; }
	
    void Update () {
        
        if(CanClick && Input.GetMouseButtonDown(0) && parentProduction.IsProductionOverlayActive && !parentProduction.BuildingClickedProduction.Tile.HasUnit())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit touchBox;
            if (Physics.Raycast(ray, out touchBox))
            {
                if (touchBox.collider == this.collider)
                {
                    BuildingGameObject buildingToProduceFrom = parentProduction.BuildingClickedProduction;
                    // Kind of ugly yet could not find better solution. The unit is created before we check if it can be bought.
                    // Set it inactive immediatly and then check for enough gold. If not then destroy else decrease the gold and set it active.
                    UnitGameObject unit = CreatorFactoryUnit.CreateUnit(buildingToProduceFrom.Tile, buildingToProduceFrom.index, type);
                    unit.gameObject.SetActive(false);
                    GameManager man = GameObject.Find("_Scripts").GetComponent<GameManager>();
                    if(man.CurrentPlayer.CanBuy(unit.UnitGame.Cost))
                    {
                        unit.gameObject.SetActive(true);
                        man.CurrentPlayer.DecreaseGoldBy(unit.UnitGame.Cost);
                        CanClick = false;
                        parentProduction.InitiateMoving(true);
                    }
                    else
                    {
                        unit.DestroyUnit();
                    }
                }
            }
        }
	}
}
