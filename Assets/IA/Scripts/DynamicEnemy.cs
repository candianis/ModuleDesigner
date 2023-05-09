using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEnemy : MonoBehaviour
{
    public float speed = 10f;
    //Distance, between the enemy and the targetPoint, needed to get to the next checkpoint
    public float distance;
    private float currentSpeed;

    private float currentTime;
    public float timeToWait;

    private Vector3 direction;

    public bool isLooking;

    private Vector3 targetPoint;
    private int pathIndex;
    private int pathLength;
    public Transform[] checkPoints;
    public List<Vector3> path;

    void Start()
    {
        pathIndex = 0;
        pathLength = checkPoints.Length;
        isLooking = false;
        foreach(Transform checkpoint in checkPoints)
        {
            path.Add(checkpoint.position);
        }
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
}
