using System;
using UnityEngine;

public class Coordinate
{
    public int x;
    public int z;
    public bool visited = false;
    public bool northWall = false;
    public bool eastWall = false;
    public bool southWall = false;
    public bool westWall = false;

    public Coordinate(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public bool Equals(Coordinate other)
    {
        if (other == null)
        {
            return false;
        }
        if (x == other.x && z == other.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
