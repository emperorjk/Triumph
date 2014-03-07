using UnityEngine;
using System.Collections;
using System;

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

	void Awake () {
        this.BuildingGame = GameJsonCreator.CreateBuilding(this, type);
        if(this.transform.parent != null)
        {
            Tile = this.transform.parent.GetComponent<Tile>();
            Tile.buildingGameObject = this;
        }
        CapturePointsText = transform.FindChild("CapturePoints").gameObject;
        CapturePointsText.renderer.enabled = false;
        // Set the sorting layer to GUI. The same used for the hightlights. Eventough you cannot set it via unity inspector you can still set it via code. :D
        CapturePointsText.renderer.sortingLayerName = "GUI";
        GameObject.Find("_Scripts").GetComponent<GameManager>().Players[index].AddBuilding(BuildingGame);
	}

    public void UpdateCapturePointsText()
    {
        TextMesh text = CapturePointsText.GetComponent<TextMesh>();
        text.text = ((int)BuildingGame.currentCapturePoints) + "/" + ((int)BuildingGame.capturePoints);
        CapturePointsText.renderer.enabled = (!Tile.IsFogShown && BuildingGame.currentCapturePoints > 0);
    }

    public void DestroyBuilding()
    {
        this.Tile.buildingGameObject = null;
        this.Tile = null;
        GameManager man = GameObject.Find("_Scripts").GetComponent<GameManager>();
        man.Players[this.index].RemoveBuilding(this.BuildingGame);

        if (man.CaptureBuildings.BuildingsBeingCaptured.Contains(this.BuildingGame))
        {
            man.CaptureBuildings.BuildingsBeingCaptured.Remove(this.BuildingGame);
        }
        
        GameObject.Destroy(this.gameObject);
    }
}
