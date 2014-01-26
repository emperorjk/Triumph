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
    public UnitBase unitGame { get; private set; }
    public Tile tile { get; set; }

    public GameObject unitHealthText { get; private set; }

	void Awake () {
        // for now ugly code
        if (type.Equals(UnitTypes.Archer)) { unitGame = new Archer(this, isHero); }
        else if (type.Equals(UnitTypes.Knight)) { unitGame = new Knight(this, isHero); }
        else if (type.Equals(UnitTypes.Swordsman)) { unitGame = new Swordsman(this, isHero); }

        if (this.transform.parent != null)
        {
            tile = this.transform.parent.GetComponent<Tile>();
            tile.unitGameObject = this;
        }
        unitHealthText = transform.FindChild("UnitHealth").gameObject;
        UpdateCapturePointsText();
        // Set the sorting layer to GUI. The same used for the hightlights. Eventhough you cannot set it via unity inspector you can still set it via code. :D
        unitHealthText.renderer.sortingLayerName = "GUI";
        GameManager.Instance.GetPlayer(index).AddUnit(unitGame);
	}

    public void UpdateCapturePointsText()
    {
        TextMesh text = unitHealthText.GetComponent<TextMesh>();
        text.text = unitGame.currentHealth.ToString();

        if (unitGame.currentHealth < unitGame.health)
        {
            unitHealthText.renderer.enabled = true;
        }
        else
        {
            unitHealthText.renderer.enabled = false;
        }
    }
}
