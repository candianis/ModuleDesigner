using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public uint ID = 0;

    public void SetID(uint ID)
    {
        this.ID = ID;
    }
    public uint GetID()
    {
        return ID;
    } 
}
