using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyFactory : MonoBehaviour
{
    private Dictionary<uint, List<PointPosition>> dynamicEnemiesPoints;

    [Header("Organization")]
    public Transform staticGroup;
    public Transform dynamicGroup;
    public Transform rotationalGroup;

    [Header("Enemies")]
    public string instructions;
    public string[] keys;
    public List<GameObject> staticInstances;
    public List<GameObject> rotationalInstances;
    public List<GameObject> dynamicInstances;

    [Header("Prefabs")]
    public GameObject static_enemy_prefab;
    public GameObject dynamic_enemy_prefab;
    public GameObject rotational_enemy_prefab;
    public GameObject point_prefab;

    [Header("Save File")]
    public LevelFile levelFile;

    public void SplitInstructions()
    {
        Array.Clear(keys, 0, keys.Length);
        keys = instructions.Split('!', StringSplitOptions.RemoveEmptyEntries);
    }

    public void GenerateEnemies()
    {
        CleanEnemies();
        dynamicEnemiesPoints = new Dictionary<uint, List<PointPosition>>();
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
                        dynamicID = uint.Parse(info.Substring(1));
                        EnemyIDExists(dynamicID);
                        GameObject newDynamic = Instantiate(dynamic_enemy_prefab);
                        SetUpEnemyTransform(newDynamic, positionX, positionY);
                        newDynamic.transform.SetParent(dynamicGroup);
                        newDynamic.GetComponentInChildren<DynamicEnemy>().SetID(dynamicID);
                        newDynamic.name = "Dynamic_Enemy_(" + positionX + "," + positionY + ")";
                        dynamicInstances.Add(newDynamic);
                        break;

                    case 'P':
                        string[] pointInfo = info.Split(new string[] { "-", "P" }, StringSplitOptions.RemoveEmptyEntries);
                        dynamicID = uint.Parse(pointInfo[0]);
                        int pointPlace = int.Parse(pointInfo[1]);
                        PointPosition point = new PointPosition(pointPlace, new Vector3(positionX + 0.5f, 0.335f, -(positionY + 0.5f)));
                        EnemyIDExists(dynamicID);
                        dynamicEnemiesPoints[dynamicID].Add(point);
                        break;

                    case 'S':
                        string direction = info.Substring(1);
                        float desiredRotation = float.Parse(direction);
                        GameObject newStatic = Instantiate(static_enemy_prefab);
                        newStatic.transform.position = new Vector3(positionX + 0.5f, 0.335f, -(positionY + 0.5f));
                        newStatic.transform.rotation = Quaternion.Euler(0, desiredRotation, 0);
                        newStatic.transform.SetParent(staticGroup);
                        newStatic.name = "Static_Enemy_(" + positionX + "," + positionY + ")_" + desiredRotation;
                        staticInstances.Add(newStatic);
                        break;

                    case 'R':
                        string[] directions = info.Split(new string[] { "R", ";" }, StringSplitOptions.RemoveEmptyEntries);
                        float[] dirs = new float[directions.Length];
                        for(int i = 0; i < dirs.Length; i++)
                        {
                            dirs[i] = float.Parse(directions[i]);
                        }

                        GameObject newRotational = Instantiate(rotational_enemy_prefab);
                        SetUpEnemyTransform(newRotational, positionX, positionY);
                        newRotational.transform.SetParent(rotationalGroup);
                        newRotational.GetComponentInChildren<RotationalEnemy>().SetAngle(dirs);
                        newRotational.name = "Rotational_Enemy_(" + positionX + "," + positionY + ")";
                        rotationalInstances.Add(newRotational);
                        break;
                }
            }
        }
        AssignDynamicEnemiesPositions();
    }

    public void SetUpEnemyTransform(GameObject enemy, int posX, int posY)
    {
        enemy.transform.position = new Vector3(posX + 0.5f, 0.335f, -(posY + 0.5f));
        enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
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
            //Debug.Log(ID + " ID already exists");
        }
    }

    private void AssignDynamicEnemiesPositions()
    {
        foreach(GameObject dynamicEnemy in dynamicInstances)
        {
            DynamicEnemy dynamicInfo = dynamicEnemy.GetComponentInChildren<DynamicEnemy>();
            uint enemyID = dynamicInfo.GetID();
            dynamicEnemiesPoints[enemyID].Sort((x, y) => x.place - y.place);
            dynamicInfo.SetDynamicEnemy(dynamicEnemiesPoints[enemyID].ToArray());
        }
    }

    public void CleanEnemies()
    {
        foreach(GameObject enemy in staticInstances)
        {
            DestroyImmediate(enemy, false);
        }
        staticInstances.Clear();

        foreach(GameObject enemy in dynamicInstances)
        {
            DestroyImmediate(enemy, false);
        }
        dynamicInstances.Clear();

        foreach(GameObject enemy in rotationalInstances)
        {
            DestroyImmediate(enemy, false);
        }
        rotationalInstances.Clear();
    }

    public void SaveEnemies()
    {
        List<DynamicData> dynamicEnemies = new List<DynamicData>();
        List<RotationalData> rotationalEnemies = new List<RotationalData>();
        List<StaticData> staticEnemies = new List<StaticData>();

        foreach(GameObject instance in dynamicInstances)
        {
            DynamicEnemy enemy = instance.GetComponentInChildren<DynamicEnemy>();
            uint ID = enemy.GetID();
            Vector3[] path = enemy.path;
            DynamicData data = new DynamicData(ID, instance.transform.position, path);
            dynamicEnemies.Add(data);
        }

        foreach(GameObject instance in staticInstances)
        {
            StaticData data = new StaticData(instance.transform.position, instance.transform.localEulerAngles.y);
            staticEnemies.Add(data);
        }

        foreach(GameObject instance in rotationalInstances)
        {
            float[] angles = instance.GetComponentInChildren<RotationalEnemy>().angles;
            RotationalData data = new RotationalData(instance.transform.position, angles);
            rotationalEnemies.Add(data);
        }

        levelFile.SaveEnemySettings(dynamicEnemies.ToArray(), rotationalEnemies.ToArray(), staticEnemies.ToArray());

    }

    public void LoadEnemies()
    {
        CleanEnemies();
        foreach(DynamicData data in levelFile.dynamicEnemies)
        {
            GameObject instance = Instantiate(dynamic_enemy_prefab);
            instance.transform.position = data.position;
            SetUpEnemyTransform(instance, (int)data.position.x, -(int)data.position.z);
            instance.transform.SetParent(dynamicGroup);
            instance.GetComponentInChildren<DynamicEnemy>().SetID(data.ID);
            instance.name = "Dynamic_Enemy_(" + data.position.x + "," + data.position.z + ")";
            instance.GetComponentInChildren<DynamicEnemy>().path = data.path;
            dynamicInstances.Add(instance);
        }

        foreach(StaticData data in levelFile.staticEnemies)
        {
            GameObject instance = Instantiate(static_enemy_prefab);
            instance.transform.position = data.position;
            instance.transform.rotation = Quaternion.Euler(0, data.angle, 0);
            instance.transform.SetParent(staticGroup);
            instance.name = "Static_Enemy_(" + data.position.x + "," + data.position.z + ")_" + data.angle;
            staticInstances.Add(instance);
        }

        foreach(RotationalData data in levelFile.rotationalEnemies)
        {
            GameObject instance = Instantiate(rotational_enemy_prefab);
            SetUpEnemyTransform(instance, (int)data.position.x, -(int)data.position.z);
            instance.transform.SetParent(rotationalGroup);
            instance.GetComponentInChildren<RotationalEnemy>().SetAngle(data.angles);
            instance.name = "Rotational_Enemy_(" + data.position.x + "," + data.position.z + ")";
            rotationalInstances.Add(instance);
        }
    }
}
