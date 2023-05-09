using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Level", menuName = "Level_Files/Level", order = 1)]
public class LevelFile : ScriptableObject
{
    [SerializeField]
    public string level_instructions;
    [SerializeField]
    public List<ModuleFile> level_modules;

    public void SaveSettingsToFile(string instructions, List<ModuleFile> modules)
    {
        level_instructions = instructions;
        level_modules = new List<ModuleFile>(modules);
    }
}
