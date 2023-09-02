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

    [Header("Enemies")]
    [SerializeField]
    public DynamicData[] dynamicEnemies;
    [SerializeField]
    public RotationalData[] rotationalEnemies;
    [SerializeField]
    public StaticData[] staticEnemies;

    public void SaveSettingsToFile(string instructions, List<ModuleFile> modules)
    {
        level_instructions = instructions;
        level_modules = new List<ModuleFile>(modules);
    }

    public void SaveEnemySettings(DynamicData[] dynamicEnemies, RotationalData[] rotationalEnemies, StaticData[] staticEnemies)
    {
        this.dynamicEnemies = dynamicEnemies;
        this.rotationalEnemies = rotationalEnemies;
        this.staticEnemies = staticEnemies;
    }
}
