using UnityEngine;
using System.Collections;

/// <summary>
/// This script is put on the childs of the production overlay prefabs. So on each of the gameobjects which contain the sprites.
/// </summary>
public class ProductionScript : MonoBehaviour {

    // Define the type of unit.
    public UnitTypes type;
    // Drag the appropiate unit prefab to spawn. If this script is put on the production overlay of the Knight. Than the Knight prefab should be dragged on here.
    // Propably not needed anymore. Need to find a solution for this.
    public UnitGameObject unitToSpawn;
	
	void Update () {
        
        if(GameManager.Instance.IsProductionOverlayActive)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit touchBox;
                if (Physics.Raycast(ray, out touchBox))
                {
                    if (touchBox.collider == this.collider)
                    {
                        // Create a new instance of the unit.
                        if(GameManager.Instance.CurrentPlayer.CanBuy(unitToSpawn.unitGame.cost))
                        {
                            Tile lastClickedBuildingTile = GameManager.Instance.LastClickedBuildingTile;
                            CreatorFactoryUnit.CreateUnit(lastClickedBuildingTile, lastClickedBuildingTile.buildingGameObject.index, type);
                            GameManager.Instance.CurrentPlayer.DecreaseGoldBy(unitToSpawn.unitGame.cost);
                            GameManager.Instance.UpdateTextboxes();
                        }
                    }
                }
            }
        }
	}
}
