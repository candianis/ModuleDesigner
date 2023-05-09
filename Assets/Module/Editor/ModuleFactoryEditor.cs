using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;




[CustomEditor(typeof(ModuleFactory))]
public class ModuleFactoryEditor : Editor
{
    protected ModuleFactory moduleFactory;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        moduleFactory = (ModuleFactory)target;

        if(GUILayout.Button("Split Instructions"))
        {
            moduleFactory.SplitAllInstructions();
        }
        if(GUILayout.Button("Generate Modules"))
        {
            moduleFactory.GenerateModules();
            moduleFactory.GenerateLevel();
        }
        if(GUILayout.Button("Clean Modules"))
        {
            moduleFactory.DeleteModules();
        }

        if(GUILayout.Button("Save Level"))
        {
            moduleFactory.SaveLevelToFile();
        }

        if(GUILayout.Button("Load Level"))
        {
            moduleFactory.LoadLevel();
            moduleFactory.GenerateLevel();
        }
    }
}
