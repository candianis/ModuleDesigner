using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    public uint ID;
    public uint keysToOpen;

    public void SetGate(uint ID, uint keys)
    {
        this.ID = ID;
        this.keysToOpen = keys;
    }
}
