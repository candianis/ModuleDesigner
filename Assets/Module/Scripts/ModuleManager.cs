using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum piece_type
{
    PILLAR,
    WALL,
    DOOR,
    GATE,
    KEY,
    DYNAMIC_ENEMY,
    STATIC_ENEMY,
    NULL
}

public class ModuleManager : MonoBehaviour
{
    public GameObject pillar_prefab;
    public GameObject door_prefab;
    public GameObject wall_prefab;
    public GameObject gate_prefab;
    public GameObject key_prefab;
    public GameObject dynamicEnemy_prefab;
    public GameObject staticEnemy_prefab;
    public GameObject[] module_pieces;
    public GameObject[] gate_pieces;
    public GameObject key_piece;

    public List<GameObject> instantiated_pieces;

    public void GenerateModule()
    {
        instantiated_pieces = new List<GameObject>();

        foreach (GameObject go in module_pieces)
        {
            go.GetComponent<Piece_Assigner>().CreatePiece();
        }
        foreach (GameObject gate in gate_pieces)
        {
            gate.GetComponent<Piece_Assigner>().CreatePiece();
        }
        key_piece.GetComponent<Piece_Assigner>().CreatePiece();
    }

    public void CleanModule()
    {
        if(instantiated_pieces.Count > 0)
        {
            for (int i = 0; i < instantiated_pieces.Count; i++)
            {
                DestroyImmediate(instantiated_pieces[i], false);
            }
            instantiated_pieces.Clear();
        }   
    }
}
