using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DynamicData
{
    public uint ID;
    public Vector3 position;
    public Vector3[] path;

    public DynamicData(uint ID, Vector3 pos, Vector3[] path)
    {
        this.ID = ID;
        this.position = pos;
        this.path = path;
    }
}

public class DynamicEnemy : MonoBehaviour
{
    public uint ID;
    public float speed = 2f;
    //Distance, between the enemy and the targetPoint, needed to get to the next checkpoint
    private float distance;
    private float currentSpeed;

    private float currentTime;
    public float timeToWait = 2;

    private Vector3 direction;

    public bool isLooking;

    private Vector3 targetPoint;
    private int pathIndex;
    private int pathLength;
    public Vector3[] path;

    void Start()
    {
        pathIndex = 0;
        pathLength = path.Length;
        isLooking = false;
        timeToWait = 1;
        distance = 0.1f;
    }

    void Update()
    {
        currentSpeed = speed * Time.deltaTime;
        targetPoint = path[pathIndex];

        if (path[pathIndex] == null)
            return;

        if(!isLooking)
        {
            if (Vector3.Distance(transform.position, targetPoint) < distance)
            {
                if (pathIndex < pathLength - 1)
                {
                    pathIndex++;
                    isLooking = true;
                }
                else
                {
                    pathIndex = 0;
                    isLooking = true;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, currentSpeed);
        }
        else
        {
            currentTime += Time.deltaTime;
            //transform.LookAt(path[pathIndex]);
            //Use a lerp here to rotate the enemy towards the next point as we can use the 't' as a time it takes for it to rotate completely
            direction = targetPoint - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, currentTime / timeToWait);
            if (currentTime > timeToWait)
            {
                isLooking = false;
                currentTime = 0;
            }
        }

    }

    public void SetDynamicEnemy(PointPosition[] points)
    {
        this.path = new Vector3[points.Length];
        for(int i = 0; i < points.Length; i++)
        {
            this.path[i] = points[i].position;
        }
    }

    public void SetID(uint ID)
    {
        this.ID = ID;
    }

    public uint GetID()
    {
        return ID;
    }
}
