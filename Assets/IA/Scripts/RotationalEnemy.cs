using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StaticData
{
    public Vector3 position;
    public float angle;

    public StaticData(Vector3 position, float angle)
    {
        this.position = position;
        this.angle = angle;
    }
}

[System.Serializable]
public struct RotationalData
{
    public Vector3 position;
    public float[] angles;

    public RotationalData(Vector3 pos, float[] angles)
    {
        this.position = pos;
        this.angles = angles;
    }

}

public class RotationalEnemy : MonoBehaviour
{
    public float rotationSpeed = 10;
    public float[] angles;

    public float timeToWait = 3;

    private int curPointIndex;
    private float timeElapsed;
    private float timeToRotate;
    
    private Quaternion startRotation;
    private Quaternion targetRotation;

    private bool isRotating;

    void Start()
    {
        curPointIndex = 0;
        timeElapsed = 0;
        timeToWait = 3;
        isRotating = true;

        startRotation = transform.rotation;
    }

    void Update()
    {
        EnemyLookAround();
    }

    private void EnemyLookAround()
    {
        targetRotation = Quaternion.Euler(0, angles[curPointIndex], 0);

        if (isRotating)
        {
            if (timeElapsed < 1.0f)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed);
                timeElapsed += Time.deltaTime * rotationSpeed;
                return;
            }
            else
            {
                startRotation = targetRotation;
                if (curPointIndex < angles.Length - 1)
                {
                    ++curPointIndex;
                    isRotating = false;
                    timeElapsed = 0;
                }
                else
                {
                    curPointIndex = 0;
                    isRotating = false;
                    timeElapsed = 0;
                }
            }
        }

        else
        {
            timeToRotate += Time.deltaTime;
            if(timeToRotate > timeToWait)
            {
                isRotating = true;
                timeToRotate = 0;
            }
        }
            

    }

    public void SetAngle(float[] angles)
    {
        this.angles = angles;    
    }
}
