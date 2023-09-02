using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyFactory))]
public class EnemyFactoryEditor : Editor
{
    protected EnemyFactory enemyFactory;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        enemyFactory = (EnemyFactory)target;

        if(GUILayout.Button("Split Instructions"))
        {
            enemyFactory.SplitInstructions();
        }

        if(GUILayout.Button("Generate Enemies"))
        {
            enemyFactory.CleanEnemies();
            enemyFactory.GenerateEnemies();
        }

        if(GUILayout.Button("Clean Enemies"))
        {
            enemyFactory.CleanEnemies();
        }

        if(GUILayout.Button("Save Enemies"))
        {
            enemyFactory.SaveEnemies();
        }

        if(GUILayout.Button("Load Enemies"))
        {
            enemyFactory.LoadEnemies();
        }
    }
}
