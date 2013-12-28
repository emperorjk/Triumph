using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// possibleLocations relative to the location of the player.
public struct PossibleLocations
{
    public int x { get; private set; }
    public int y { get; private set; }
    public PossibleLocations(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class RangeTiles
{
    // key = range 1,2,3,4 value = possibleLocations
    public Dictionary<int, PossibleLocations[]> possibleLocations { get; private set; }


    public void CreatePossibleRangeLocations()
    {
        possibleLocations = new Dictionary<int, PossibleLocations[]>();

        // creating the arrays.
        PossibleLocations[] tilesRangeOne = new PossibleLocations[4];
        PossibleLocations[] tilesRangeTwo = new PossibleLocations[12];
        PossibleLocations[] tilesRangeThree = new PossibleLocations[24];
        PossibleLocations[] tilesRangeFour = new PossibleLocations[40];

        // filling the arrays with locations
        tilesRangeOne[0] = new PossibleLocations(0, 1);
        tilesRangeOne[1] = new PossibleLocations(0, -1);
        tilesRangeOne[2] = new PossibleLocations(1, 0);
        tilesRangeOne[3] = new PossibleLocations(-1, 0);

        for (int i = 0; i < 4; i++)
        {
            tilesRangeTwo[i] = tilesRangeOne[i];
        }

        tilesRangeTwo[4] = new PossibleLocations(-1, -1);
        tilesRangeTwo[5] = new PossibleLocations(1, -1);
        tilesRangeTwo[6] = new PossibleLocations(-1, 1);
        tilesRangeTwo[7] = new PossibleLocations(1, 1);
        tilesRangeTwo[8] = new PossibleLocations(2, 0);
        tilesRangeTwo[9] = new PossibleLocations(-2, 0);
        tilesRangeTwo[10] = new PossibleLocations(0, 2);
        tilesRangeTwo[11] = new PossibleLocations(0, -2);

        for (int i = 0; i < 12; i++ )
        {
            tilesRangeThree[i] = tilesRangeTwo[i];
        }

        tilesRangeThree[12] = new PossibleLocations(-2, 1);
        tilesRangeThree[13] = new PossibleLocations(2, 1);
        tilesRangeThree[14] = new PossibleLocations(-1, 2);
        tilesRangeThree[15] = new PossibleLocations(1, 2);
        tilesRangeThree[16] = new PossibleLocations(3, 0);
        tilesRangeThree[17] = new PossibleLocations(-3, 0);
        tilesRangeThree[18] = new PossibleLocations(0, 3);
        tilesRangeThree[19] = new PossibleLocations(0, -3);
        tilesRangeThree[20] = new PossibleLocations(2, -1);
        tilesRangeThree[21] = new PossibleLocations(1, -2);
        tilesRangeThree[22] = new PossibleLocations(-2, -1);
        tilesRangeThree[23] = new PossibleLocations(-1, -2);

        for (int i = 0; i < 24; i++)
        {
            tilesRangeFour[i] = tilesRangeThree[i];
        }

        tilesRangeFour[12] = new PossibleLocations(-4, 0);
        tilesRangeFour[12] = new PossibleLocations(4, 0);
        tilesRangeFour[12] = new PossibleLocations(0, -4);
        tilesRangeFour[12] = new PossibleLocations(0, 4);
        tilesRangeFour[12] = new PossibleLocations(1, -3);
        tilesRangeFour[12] = new PossibleLocations(1, 3);
        tilesRangeFour[12] = new PossibleLocations(-1, -3);
        tilesRangeFour[12] = new PossibleLocations(-1, 3);
        tilesRangeFour[12] = new PossibleLocations(-2, -2);
        tilesRangeFour[12] = new PossibleLocations(-2, 2);
        tilesRangeFour[12] = new PossibleLocations(2, -2);
        tilesRangeFour[12] = new PossibleLocations(2, 2);
        tilesRangeFour[12] = new PossibleLocations(-3, -1);
        tilesRangeFour[12] = new PossibleLocations(-3, 1);
        tilesRangeFour[12] = new PossibleLocations(3, -1);
        tilesRangeFour[12] = new PossibleLocations(3, 1);

        possibleLocations.Add(1, tilesRangeOne);
        possibleLocations.Add(2, tilesRangeTwo);
        possibleLocations.Add(3, tilesRangeThree);
        possibleLocations.Add(4, tilesRangeFour);
    }
}
