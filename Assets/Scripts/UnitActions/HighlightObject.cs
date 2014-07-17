using Assets.Scripts.Main;
using Assets.Scripts.Tiles;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Scripts.UnitActions
{
    public class HighlightObject : MonoBehaviour
    {

        public HighlightTypes highlightTypeActive;

        public List<Sprite> highlights;

        public Tile Tile { get; private set; }

        private void Start()
        {
            highlights = new List<Sprite>();
            Tile = transform.parent.gameObject.GetComponent<Tile>();
            highlights.Add(Resources.Load<Sprite>(FileLocations.highlightAttack));
            highlights.Add(Resources.Load<Sprite>(FileLocations.highlightMove));
        }

        public void ChangeHighlight(HighlightTypes highlightToActivate)
        {
            if (highlightToActivate == HighlightTypes.highlight_none)
            {
                GetComponent<SpriteRenderer>().sprite = null;
                highlightTypeActive = highlightToActivate;
                renderer.enabled = false;
                collider.enabled = false;
            }
            else if (highlightTypeActive != highlightToActivate)
            {
                Sprite s = highlights.FirstOrDefault(x => x.name == highlightToActivate.ToString());
                if (s != null)
                {
                    GetComponent<SpriteRenderer>().sprite = s;
                    highlightTypeActive = highlightToActivate;
                    renderer.enabled = true;
                    collider.enabled = true;
                }
            }

        }
    }
}