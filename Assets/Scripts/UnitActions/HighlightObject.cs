using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public enum HighlightTypes
{
    highlight_none,
    highlight_attack,
    highlight_move
}

public class HighlightObject : MonoBehaviour {

    public HighlightTypes highlightTypeActive;

    public List<Sprite> highlights;

    public Tile tile { get; private set; }

	void Start () {
        highlights = new List<Sprite>();
        tile = transform.parent.gameObject.GetComponent<Tile>();
        highlights.Add((Sprite)Resources.Load<Sprite>(FileLocations.highlightAttack));
        highlights.Add((Sprite)Resources.Load<Sprite>(FileLocations.highlightMove));
	}
    
    public void ChangeHighlight(HighlightTypes highlightToActivate)
    {
        if(highlightToActivate == HighlightTypes.highlight_none)
        {
            GetComponent<SpriteRenderer>().sprite = null;
            this.highlightTypeActive = highlightToActivate;
            this.renderer.enabled = false;
            this.collider.enabled = false;
        }
        else if (this.highlightTypeActive != highlightToActivate)
        {
            Sprite s = highlights.FirstOrDefault(x => x.name == highlightToActivate.ToString());
            if (s != null)
            {
                GetComponent<SpriteRenderer>().sprite = s;
                this.highlightTypeActive = highlightToActivate;
                this.renderer.enabled = true;
                this.collider.enabled = true;
            }
        }
        
    }
}
