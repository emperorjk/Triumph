using UnityEngine;
using System.Collections;

/// <summary>
/// This script is put on the childs of the production overlay prefabs. So on each of the gameobjects which contain the sprites.
/// </summary>
public class ProductionScript : MonoBehaviour {

    // Define the type of unit. Propably not needed anymore.
    public UnitTypes type;
    // Drag the appropiate unit prefab to spawn. If this script is put on the production overlay of the Knight. Than the Knight prefab should be dragged on here.
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
                            // The position in the prefab should be 0,0,0. So when the parent is set to a tile it should spawn directly on a tile.
                            // The same happens now with pre spawned units. Its position is 0,0,0 yet its parent is placed on a certain position.
                            UnitGameObject game = (UnitGameObject)Instantiate(unitToSpawn);
                            game.transform.parent = GameManager.Instance.LastClickedBuildingTile.transform;
                        }
                    }
                }
            }
        }
	}
}
