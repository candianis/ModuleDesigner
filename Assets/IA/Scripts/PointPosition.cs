using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPosition : MonoBehaviour
{
    public uint place;
    public Vector3 position;

    public PointPosition(uint place, Vector3 pos)
    {
        this.place = place;
        this.position = pos;
    }
    public void SetPoint(uint place, Vector3 pos)
    {
        this.place = place;
        this.position = pos;
    }
}
