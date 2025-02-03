using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridPosition
{
    public int y;
    public int z;


    public GridPosition(int z, int y)
    {
        this.y = y;
        this.z = z;
    }

    public override string ToString()
    {
        return "z: " + z + "; y:" + y;
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.z == b.z && a.y == b.y;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.z + b.z, a.y + b.y);
    }

    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.z - b.z, a.y - b.y);
    }



}

