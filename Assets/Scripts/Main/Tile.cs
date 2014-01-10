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

    public GameObject FogOfWar { get; private set; }
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
        FogOfWar = (GameObject)GameObject.Instantiate(Resources.Load(FileLocations.fogOfWar));
        FogOfWar.renderer.enabled = false;
        FogOfWar.transform.position = this.transform.position;
        FogOfWar.transform.parent = this.transform;

        HighlightAttack = ((GameObject)GameObject.Instantiate(Resources.Load(FileLocations.highlightAttack)));
        HighlightMove = ((GameObject)GameObject.Instantiate(Resources.Load(FileLocations.highlightMove)));

        // Setting the parent for the Unity Hierarchy and the position to the correct place
        HighlightAttack.transform.parent = this.transform;
        HighlightAttack.transform.position = this.transform.position;
        HighlightMove.transform.parent = this.transform;
        HighlightMove.transform.position = this.transform.position;

        HighlightAttack.name = "highlight_attack";
        HighlightMove.name = "highlight_move";
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
