using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
public struct ModuleFile
{
    [SerializeField]
    public Vector2 position;
    public List<piece_type> pieces;
}


public class ModuleFactory : MonoBehaviour
{
    [Header("Arquitecture")]
    public string instructions_arq;
    public string[] keys_arq;

    [Header("Gates and Keys")]
    public string instructions_GK;
    public string[] keys_GK;

    [Header("Module Info")]
    public GameObject modulePrefab;
    [SerializeField]
    public List<ModuleFile> modules = new List<ModuleFile>();
    public List<GameObject> modules_instances;
    public Material[] id_materials;

    [Header("Level Settings")]
    public LevelFile levelFile;

    public void SplitAllInstructions()
    { 
        //Arquitecture parse
        instructions_arq.Remove(instructions_arq.Length-1);
        Array.Clear(keys_arq, 0, keys_arq.Length);
        keys_arq = instructions_arq.Split('!', StringSplitOptions.RemoveEmptyEntries);

        //Gates & Keys parse
        Array.Clear(keys_GK, 0, keys_GK.Length);
        keys_GK = instructions_GK.Split('!', StringSplitOptions.RemoveEmptyEntries);
    }

    public void GenerateModules()
    {
        DeleteModules();
        foreach (string key in keys_arq)
        {
            GameObject moduleIns = GameObject.Instantiate(modulePrefab);
            modules_instances.Add(moduleIns);
            moduleIns.transform.SetParent(this.gameObject.transform);
            moduleIns.transform.localPosition = Vector3.zero;
            moduleIns.transform.localRotation = Quaternion.Euler(0, 0, 0);
            string[] keyInfo = key.Split(new string[] { ":", "-", "!" }, StringSplitOptions.RemoveEmptyEntries);

            //Debug.Log(keyInfo[0].ToString() + " " + keyInfo[1].ToString() + " " + keyInfo[2].ToString() + " " + keyInfo[2].Length);

            float x = float.Parse(keyInfo[0]);
            float z = -float.Parse(keyInfo[1]);
            moduleIns.transform.position += new Vector3(x, 0, z);

            ModuleFile newModuleInfo = new ModuleFile();
            newModuleInfo.position = new Vector2(x, z);
            newModuleInfo.pieces = new List<piece_type>();

            for(int i = 0; i < keyInfo[2].Length; ++i)
            {
                piece_type piece = piece_type.NULL;
                switch (keyInfo[2][i])
                {
                    case 'F':
                        break;
                    case 'P':
                        piece = piece_type.PILLAR;
                        break;
                    case 'W':
                        piece = piece_type.WALL;
                        break;
                    case 'D':
                        piece = piece_type.DOOR;
                        break;
                    default:
                        piece = piece_type.NULL;
                        break;
                }
                moduleIns.GetComponent<ModuleManager>().module_pieces[i].GetComponent<Piece_Assigner>().AssignPiece(piece);
                newModuleInfo.pieces.Add(piece);
            }

            for(int i = 0; i < keys_GK.Length; i++)
            {
                string[] keyGKInfo = keys_GK[i].Split(new string[] { ":", "-" }, StringSplitOptions.RemoveEmptyEntries);
                if (keyGKInfo[0] == keyInfo[0] && keyGKInfo[1] == keyInfo[1])
                {
                    uint currentID;
                    uint keysToOpen;
                    for(int j = 2; j < keyGKInfo.Length; j++)
                    {
                        switch (keyGKInfo[j][0])
                        {

                            case 'K':
                                currentID = uint.Parse(keyGKInfo[j][1].ToString());
                                moduleIns.GetComponent<ModuleManager>().key_piece.GetComponent<Piece_Assigner>().InstantiatePieceWithID(piece_type.KEY, id_materials[currentID], currentID);
                                break;

                            case 'G':
                                currentID = uint.Parse(keyGKInfo[j][1].ToString());
                                keysToOpen = uint.Parse(keyGKInfo[j][3].ToString());
                                int currentGateIndex = 0;

                                if (keyGKInfo[j][2] == 'W')
                                    currentGateIndex = 1;
                                else if (keyGKInfo[j][2] == 'E')
                                    currentGateIndex = 2;
                                else if (keyGKInfo[j][2] == 'S')
                                    currentGateIndex = 3;

                                moduleIns.GetComponent<ModuleManager>().gate_pieces[currentGateIndex].GetComponent<Piece_Assigner>().InstantiatePieceWithID(piece_type.GATE,id_materials[currentID], currentID, keysToOpen);

                                break;

                        }

                    }
                }
            }

            modules.Add(newModuleInfo);
        }

    }

    public void GenerateGatesKeys()
    {
        
    }

    public void DeleteModules()
    {
        if (modules_instances.Count > 0)
        {
            foreach (GameObject module in modules_instances)
                DestroyImmediate(module, false);
            modules_instances.Clear();
        }
        
        if(modules.Count > 0)
        {
            modules.Clear();
        }
    }

    public void GenerateLevel()
    {
        foreach(GameObject module in modules_instances)
        {
            module.GetComponent<ModuleManager>().GenerateModule();
        }
    }

    public void SaveLevelToFile()
    {
        if(levelFile != null)
        {
            levelFile.SaveSettingsToFile(instructions_arq, modules);
            //EditorUtility.SetDirty(levelFile);
            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
        }
        else
        {
            Debug.Log("Assign a clean level file");
        }
    }

    public void LoadLevel()
    {
        if(levelFile != null)
        {
            DeleteModules();
            instructions_arq = levelFile.level_instructions;
            modules = new List<ModuleFile>(levelFile.level_modules);

            #region NEW_GENERATION

            foreach(ModuleFile module in modules)
            {
                GameObject moduleIns = GameObject.Instantiate(modulePrefab);
                modules_instances.Add(moduleIns);
                moduleIns.transform.SetParent(this.gameObject.transform);
                moduleIns.transform.localPosition = Vector3.zero;
                moduleIns.transform.localRotation = Quaternion.Euler(0, 0, 0);
                moduleIns.transform.position = new Vector3(module.position.x, 0, module.position.y);

                int i = 0;
                
                foreach(GameObject piece in moduleIns.GetComponent<ModuleManager>().module_pieces)
                {
                    piece.GetComponent<Piece_Assigner>().AssignPiece(module.pieces[i]);
                    if(i < module.pieces.Count)
                        ++i;
                }
            }

            #endregion

            //GenerateModules();
        }
        else
        {
            Debug.Log("Assign a level file with information");
        }
    }
}
