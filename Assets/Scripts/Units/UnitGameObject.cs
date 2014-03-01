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
    public Unit UnitGame { get; private set; }
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
        // Set the sorting layer to GUI. The same used for the hightlights. Eventhough you cannot set it via unity inspector you can still set it via code. :D
        UnitHealthText.renderer.sortingLayerName = "GUI";
        GameManager.Instance.Players[index].AddUnit(UnitGame);
	}

    public void UpdateHealthText()
    {
        TextMesh text = UnitHealthText.GetComponent<TextMesh>();
        text.text = ((int)Mathf.Clamp(UnitGame.CurrentHealth, 1f, UnitGame.MaxHealth)).ToString();
        UnitHealthText.renderer.enabled = (!Tile.IsFogShown && UnitGame.CurrentHealth < UnitGame.MaxHealth);
    }

    public void DestroyUnit()
    {
        this.Tile.unitGameObject = null;
        this.Tile = null;
        GameManager.Instance.Players[(this.index)].RemoveUnit(this.UnitGame);
        GameObject.Destroy(this.gameObject);
    }
}
