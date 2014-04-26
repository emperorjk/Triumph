namespace Assets.Scripts.Tiles
{
    /// <summary>
    /// This class is used to find Tiles in the Tile list. This makes it easier than using two variables e.g. int ColumndId and int RowId.
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
}
