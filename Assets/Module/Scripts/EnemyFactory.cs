using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyFactory : MonoBehaviour
{
    private Dictionary<uint, List<PointPosition>> dynamicEnemiesPoints;

    [Header("Enemies")]
    public string instructions;
    public string[] keys;
    public List<GameObject> enemyInstances;

    [Header("Prefabs")]
    public GameObject static_enemy_prefab;
    public GameObject dynamic_enemy_prefab;
    public GameObject point_prefab;

    public void SplitInstructions()
    {
        Array.Clear(keys, 0, keys.Length);
        keys = instructions.Split('!', StringSplitOptions.RemoveEmptyEntries);
    }

    public void GenerateEnemies()
    {
        foreach(string key in keys)
        {
            string[] keyInfo = key.Split(new string[] { "|", ":" }, StringSplitOptions.RemoveEmptyEntries);
            int positionX = int.Parse(keyInfo[0]);
            int positionY = int.Parse(keyInfo[1]);
            foreach(string info in keyInfo)
            {
                uint dynamicID;
                switch (info[0])
                {
                    case 'D':
                        dynamicID = info[1];
                        EnemyIDExists(dynamicID);
                        GameObject newDynamic = Instantiate(dynamic_enemy_prefab);
                        newDynamic.transform.position = new Vector3(positionX + 0.5f, 0.335f, positionY + 0.5f);
                        newDynamic.transform.rotation = Quaternion.Euler(0, 0, 0);
                        newDynamic.GetComponentInChildren<DynamicEnemy>().SetID(dynamicID);
                        enemyInstances.Add(newDynamic);
                        break;

                    case 'P':
                        string[] pointInfo = info.Split(new string[] { "-", "P" }, StringSplitOptions.RemoveEmptyEntries);
                        dynamicID = uint.Parse(pointInfo[0]);
                        uint pointPlace = uint.Parse(pointInfo[1]);
                        PointPosition point = new PointPosition(pointPlace, new Vector3(positionX + 0.5f, 0.335f, positionY + 0.5f));

                        EnemyIDExists(dynamicID);
                        dynamicEnemiesPoints[dynamicID].Add(point);
                        break;

                    case 'S':

                        break;
                }
            }
        }
    }

    private void EnemyIDExists(uint ID)
    {
        if (!dynamicEnemiesPoints.ContainsKey(ID))
        {
            List<PointPosition> newDynamicPoints = new List<PointPosition>();
            dynamicEnemiesPoints.Add(ID, newDynamicPoints);
        }
        else
        {
            Debug.Log(ID + " ID already exists");
        }
    }

    private void AssignDynamicEnemiesPositions()
    {
        dynamicEnemiesPoints[0].ToArray();
    }

    public void CleanEnemies()
    {

    }
}
