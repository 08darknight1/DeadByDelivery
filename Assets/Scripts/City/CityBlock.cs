using System;
using System.Collections.Generic;

[Serializable]

public class CityBlock
{
    private CityTile[] cityTiles;

    public void SetCityBlock(List<string> tilesChar, List<bool> tilesWalk)
    {
        cityTiles = new CityTile[tilesChar.Count];

        for(int x = 0; x < tilesChar.Count; x++)
        {
            cityTiles[x] = new CityTile(tilesChar[x], tilesWalk[x]);
        }
    }

    public CityTile[] ReturnCityBlockTiles()
    {
        return cityTiles;
    }
}
