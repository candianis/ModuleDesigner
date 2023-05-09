using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ModuleManager))]
public class ModuleManagerEditor : Editor
{
    protected ModuleManager module_manager;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        module_manager = (ModuleManager)target;

        if(GUILayout.Button("Generate Module"))
        {
            module_manager.GenerateModule();
        }

        if(GUILayout.Button("Clean Module"))
        {
            module_manager.CleanModule();
        }
    }
}
