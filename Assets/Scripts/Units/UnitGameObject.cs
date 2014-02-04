using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This script is put on a prefab and creates the appropiate UnitBase object. It also passes itself to the UnitBase so they can both reach each other via a variable.
/// And adds the UnitBase object the the correct player list of owned units.
/// </summary>
public class UnitGameObject : MonoBehaviour
{
    public PlayerIndex index;
    public UnitTypes type;
    public bool isHero;
    public UnitBase UnitGame { get; private set; }
    public Tile Tile { get; set; }

    public GameObject UnitHealthText { get; private set; }

	void Awake () {
        this.UnitGame = GameJsonCreator.CreateUnit(this, isHero, type);

        if (this.transform.parent != null)
        {
            Tile = this.transform.parent.GetComponent<Tile>();
            Tile.unitGameObject = this;
        }
        UnitHealthText = transform.FindChild("UnitHealth").gameObject;
        UpdateCapturePointsText();
        // Set the sorting layer to GUI. The same used for the hightlights. Eventhough you cannot set it via unity inspector you can still set it via code. :D
        UnitHealthText.renderer.sortingLayerName = "GUI";
        GameManager.Instance.Players[index].AddUnit(UnitGame);
	}

    public void UpdateCapturePointsText()
    {
        TextMesh text = UnitHealthText.GetComponent<TextMesh>();
        text.text = UnitGame.CurrentHealth.ToString();

        if (UnitGame.CurrentHealth < UnitGame.MaxHealth)
        {
            UnitHealthText.renderer.enabled = true;
        }
        else
        {
            UnitHealthText.renderer.enabled = false;
        }
    }
    public void DestroyUnit()
    {
        this.Tile.unitGameObject = null;
        this.Tile = null;
        GameManager.Instance.Players[(this.index)].RemoveUnit(this.UnitGame);
        GameObject.Destroy(this.gameObject);
    }
}
