using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// This class is used to find Tiles in the tile list. This makes it easier than using two variables e.g. int ColumndId and int RowId.
/// </summary>
public class TileCoordinates
{
    public int ColumnId { get; private set; }
    public int RowId { get; private set; }
    public TileCoordinates(int _ColumnId, int _RowId)
    {
        ColumnId = _ColumnId;
        RowId = _RowId;
    }

    public override string ToString()
    {
        return "ColumnId: " + ColumnId + ". RowId: " + RowId + ".";
    }
}

/// <summary>
/// This script is put on a prefab. Some information needs to be set within Unity. The environment object is a prefab. That needs to be dragged ontop of the environmentGameObject variable.
/// This is also true for possible units and buildings.
/// So the tile has a reference to the stuff that is on it.
/// </summary>
public class Tile : MonoBehaviour
{
    // These values are set within unity.
    public int ColumnId;
    public int RowId;
    // The three gameobjects below are prefabs of corresponding objects. ArcherRed-Prefab, ArcherBlue-Prefab, Grass-Prefab, etc.
    // Each of these prefabs should have the corresponding script attached e.g. BuildingGameObject, UnitGameObject or EnvironmentGameObject.
    // And be set within the unity environment.
    public EnvironmentGameObject environmentGameObject;
    public BuildingGameObject buildingGameObject;
    public UnitGameObject unitGameObject;
    public GameObject FogOfWar { get; private set; }
    public HighlightObject highlight { get; private set; }
    public TileCoordinates Coordinate { get; private set; }
    public Vector2 Vector2 { get; set; }
    // False is facing right and true is facing left
    public bool facingDirection { get; set; }

    void Awake()
    {
        Coordinate = new TileCoordinates(ColumnId, RowId);
        Vector2 = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        TileHelper.AddTile(this);

        InitHighlights();
    }

    private void InitHighlights()
    {
        FogOfWar = (GameObject)GameObject.Instantiate(Resources.Load(FileLocations.fogOfWar));
        FogOfWar.renderer.enabled = false;
        FogOfWar.transform.position = this.transform.position;
        FogOfWar.transform.parent = this.transform;

        GameObject highlight = ((GameObject)GameObject.Instantiate(Resources.Load(FileLocations.highlight)));
        highlight.transform.parent = this.transform;
        highlight.transform.position = this.transform.position;
        this.highlight = highlight.GetComponent<HighlightObject>();
        this.highlight.ChangeHighlight(HighlightTypes.highlight_none);        
    }

    public bool HasBuilding()
    {
        return buildingGameObject != null;
    }

    public bool HasUnit()
    {
        return unitGameObject != null;
    }
}
