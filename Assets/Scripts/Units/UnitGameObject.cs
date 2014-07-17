using System.Runtime.Serialization;
using Assets.Scripts.Main;
using Assets.Scripts.Players;
using Assets.Scripts.Tiles;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Units
{
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
        public SpriteRenderer SelectionBox { get; set; }
        public GameObject UnitHealthText { get; private set; }

        private void Awake()
        {
            UnitGame = GameJsonCreator.CreateUnit(this, isHero, type);

            GameObject selectionBox = null;
            if (index == PlayerIndex.Red)
            {
                 selectionBox = (GameObject) Instantiate(Resources.Load<GameObject>("Prefabs/Misc/SelectionRed"));
            }
            else if(index == PlayerIndex.Blue)
            {
                selectionBox = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Misc/SelectionBlue"));
            }
            selectionBox.transform.position = transform.position;
            selectionBox.transform.parent = transform;
            SelectionBox = selectionBox.GetComponent<SpriteRenderer>();


            if (transform.parent != null)
            {
                Tile = transform.parent.GetComponent<Tile>();
                Tile.unitGameObject = this;
            }
            UnitHealthText = transform.FindChild("UnitHealth").gameObject;
            // Set the sorting layer to GUI. The same used for the hightlights. Eventhough you cannot set it via unity inspector you can still set it via code. :D
            UnitHealthText.renderer.sortingLayerName = "GUI";
            GameObject.Find("_Scripts").GetComponent<GameManager>().Players[index].AddUnit(UnitGame);
        }

        public void UpdateHealthText()
        {
            TextMesh text = UnitHealthText.GetComponent<TextMesh>();
            text.text = ((int) Mathf.Clamp(UnitGame.CurrentHealth, 1f, UnitGame.MaxHealth)).ToString();
            UnitHealthText.renderer.enabled = (!Tile.IsFogShown && UnitGame.CurrentHealth < UnitGame.MaxHealth);
        }

        public void DestroyUnit()
        {
            Tile.unitGameObject = null;
            Tile = null;
            GameObject.Find("_Scripts").GetComponent<GameManager>().Players[(index)].RemoveUnit(UnitGame);
            Destroy(gameObject);
        }
    }
}