using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    private float currentRotationSpeed;
    public float rotationSpeed;
    private float angle;
    public float minAngle;
    public float maxAngle;

    private float currentTime;
    private float lastTime;
    public float timeToWait;

    private int curPointIndex;
    private int pointsLength;
    private Vector3 direction;
    private Vector3 targetPoint;
    public Transform[] points;
    public List<Vector3> pointsPosition;

    private bool isLooking;
    public bool shouldLookAround;

    void Start()
    {
        pointsLength = points.Length;
        curPointIndex = 0;
        lastTime = 0f;
        isLooking = true;
        foreach (Transform point in points)
            pointsPosition.Add(point.position);   
    }

    void Update()
    {
        currentRotationSpeed = rotationSpeed * Time.deltaTime;
        targetPoint = pointsPosition[curPointIndex];
        EnemyLookAround();
    }

    private void EnemyLookAround()
    {
        if (!shouldLookAround)
        {
            Debug.Log("Enemy does not rotate");
            return;
        }

        if (isLooking)
        {
            direction = targetPoint - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, currentRotationSpeed);

            angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(pointsPosition[0] - transform.position));
            if (angle < minAngle)
            {
                curPointIndex = 1;
                isLooking = false;
            }
            else if (angle > maxAngle)
            {
                curPointIndex = 0;
                isLooking = false;
            }
        }

        if (!isLooking)
        {
            currentTime += Time.deltaTime;
            if (currentTime - lastTime > timeToWait)
            {
                isLooking = true;
                currentTime = 0;
                lastTime = 0;
            }
        }
        
    }
}
