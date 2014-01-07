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

    public GameObject HighlightMove { get; private set; }
    public GameObject HighlightAttack { get; private set; }
    public TileCoordinates Coordinate { get; private set; }

    void Awake()
    {
        Coordinate = new TileCoordinates(ColumnId, RowId);
        GameManager.Instance.AddTile(this);

        InitHighlights();
    }

    private void InitHighlights()
    {
        GameObject highlightAttack = ((GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Level/highlight_attack")));
        GameObject highlightMove = ((GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Level/highlight_move")));

        // Setting the parent for the Unity Hierarchy and the position to the correct place
        highlightAttack.transform.parent = this.transform;
        highlightAttack.transform.position = this.transform.position;
        highlightMove.transform.parent = this.transform;
        highlightMove.transform.position = this.transform.position;

        highlightAttack.name = "highlight_attack";
        highlightMove.name = "highlight_move";

        HighlightAttack = this.gameObject.transform.FindChild("highlight_attack").gameObject;
        HighlightMove = this.gameObject.transform.FindChild("highlight_move").gameObject;
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
