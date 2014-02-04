﻿using UnityEngine;
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
    public BuildingsBase buildingGame { get; private set; }
    public Tile tile { get; set; }
    public GameObject capturePointsText { get; private set; }

	void Awake () {
        this.buildingGame = GameJsonCreator.CreateBuilding(this, type);
        if(this.transform.parent != null)
        {
            tile = this.transform.parent.GetComponent<Tile>();
            tile.buildingGameObject = this;
        }
        capturePointsText = transform.FindChild("CapturePoints").gameObject;
        UpdateCapturePointsText();
        // Set the sorting layer to GUI. The same used for the hightlights. Eventough you cannot set it via unity inspector you can still set it via code. :D
        capturePointsText.renderer.sortingLayerName = "GUI";
        GameManager.Instance.Players[index].AddBuilding(buildingGame);
	}

    public void UpdateCapturePointsText()
    {
        TextMesh text = capturePointsText.GetComponent<TextMesh>();
        text.text = buildingGame.currentCapturePoints + "/" + buildingGame.capturePoints;

        if(buildingGame.currentCapturePoints <= 0)
        {
            capturePointsText.renderer.enabled = false;
        }
        else
        {
            capturePointsText.renderer.enabled = true;
        }
    }

    public void DestroyBuilding()
    {
        this.tile.buildingGameObject = null;
        this.tile = null;
        GameManager.Instance.Players[this.index].RemoveBuilding(this.buildingGame);
        GameObject.Destroy(this.gameObject);
    }
}
