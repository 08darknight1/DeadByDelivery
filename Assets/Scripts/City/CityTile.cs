using UnityEngine;

public class CityTile
{
    protected string DefiningChar;

    protected bool Walkable;

    public CityTile(string newChar, bool newWalkable)
    {
        DefiningChar = newChar;
        Walkable = newWalkable;
    }

    public string ReturnDefiningChar()
    {
        return DefiningChar;
    }

    public bool ReturnWalkable()
    {
        return Walkable;
    }
}
