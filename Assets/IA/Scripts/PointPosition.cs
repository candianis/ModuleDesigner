using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PointPosition
{
    public int place;
    public Vector3 position;

    public PointPosition(int place, Vector3 pos)
    {
        this.place = place;
        this.position = pos;
    }
    public void SetPoint(int place, Vector3 pos)
    {
        this.place = place;
        this.position = pos;
    }

    public int GetPlace()
    {
        return place;
    }

    public Vector3 GetPosition()
    {
        return position;
    }
}
