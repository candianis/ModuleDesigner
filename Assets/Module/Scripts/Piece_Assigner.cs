using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Assigner : MonoBehaviour
{
    public piece_type piece;
    public bool isEdge = false;
    private ModuleManager manager;
    private GameObject obj_prefab;

    public void AssignPiece(piece_type piece)
    {
        this.piece = piece;
    }

    public void CreatePiece()
    {
        manager = GetComponentInParent<ModuleManager>();
        switch (piece)
        {
            case piece_type.WALL:
                if (isEdge)
                    break;
                InstantiatePiece(manager.wall_prefab);
                break;

            case piece_type.DOOR:
                if (isEdge)
                    break;
                InstantiatePiece(manager.door_prefab);
                break;

            case piece_type.PILLAR:
                if (!isEdge)
                    break;
                InstantiatePiece(manager.pillar_prefab);
                break;

            case piece_type.NULL:
            default:
                break;
        }
    }

    private void InstantiatePiece(GameObject piece_gameObject)
    {
        obj_prefab = GameObject.Instantiate(piece_gameObject);
        obj_prefab.transform.SetParent(this.gameObject.transform);
        obj_prefab.transform.localPosition = Vector3.zero;
        obj_prefab.transform.localRotation = Quaternion.Euler(0, 0, 0);
        manager.instantiated_pieces.Add(obj_prefab);
    }

    public void InstantiatePieceWithID(piece_type type, Material mat_id, uint ID, uint keys = 0)
    {
        manager = GetComponentInParent<ModuleManager>();
        switch (type)
        {
            case piece_type.GATE:
                this.piece = type;
                InstantiatePiece(manager.gate_prefab);
                obj_prefab.GetComponent<MeshRenderer>().material = mat_id;
                obj_prefab.GetComponent<GateManager>().SetGate(ID, keys);
                break;
            case piece_type.KEY:
                this.piece = type;
                InstantiatePiece(manager.key_prefab);
                for(int i = 0; i < obj_prefab.transform.childCount; i++)
                {
                    obj_prefab.transform.GetChild(i).GetComponent<MeshRenderer>().material = mat_id;
                }
                obj_prefab.GetComponent<KeyManager>().SetID(ID);
                break;
        }
    }
}
