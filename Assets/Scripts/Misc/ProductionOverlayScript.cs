using UnityEngine;
using System.Collections;

/// <summary>
/// This class is the parent class for the production overlay. It shows and hides the overlays. It is placed onto the parent empty gameobject of the production overlay containers.
/// For each type of unit (e.g. cavalry, melee and ranged) is an prefab which each script on it.
/// </summary>
public class ProductionOverlayScript : MonoBehaviour {

    // Define that this overlay belongs to player one or two.
    public PlayerIndex index;
    // Define the building that this overlay belongs to. E.g. BarracksCavalry, BarracksMelee or BarracksRange.
    public BuildingTypes type;

	void Update () {
	    
        if(index.Equals(GameManager.Instance.CurrentPlayer.index))
        {
            if (Input.GetMouseButtonDown(0))
            {
                bool foundBuilding = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit touchBox;
                if (Physics.Raycast(ray, out touchBox))
                {
                    foreach (BuildingsBase building in GameManager.Instance.CurrentPlayer.ownedBuildings)
                    {
                        if (type.Equals(building.type) && building.CanProduce && touchBox.collider == building.buildingGameObject.collider)
                        {
                            ShowOrHideProductionOverlay(true);
                            GameManager.Instance.IsProductionOverlayActive = true;
                            foundBuilding = true;
                            break;
                        }
                    }
                }
                if (!foundBuilding)
                {
                    // Propably here.
                    ShowOrHideProductionOverlay(false);
                }
            }
        }
	}

    private void ShowOrHideProductionOverlay(bool ShowOverlay)
    {
        this.renderer.enabled = ShowOverlay;
        foreach (ProductionScript ps in GetComponentsInChildren<ProductionScript>())
        {
            // or we could disable all of the childs.
            ps.renderer.renderer.enabled = ShowOverlay;
            ps.collider.enabled = ShowOverlay;
        }
    }
}
