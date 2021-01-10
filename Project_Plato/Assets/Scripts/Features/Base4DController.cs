using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a base 4D shape controller that controls 4D shapes based on two group timestamps
// script can find objects in the scene under specific name, no need for declaring public variables

public class Base4DController : MonoBehaviour
{
    [Header("Scene Time A Settings")]
    public float timeAMaxW = 3.8f; // positive
    public float timeAMinW = 0f;

    [Header("Scene Time B Settings")]
    public float timeBMaxW = 0f; // equals to timeAMinW
    public float timeBMinW = -3.8f; // negative

    [Header("Scene Time Change Settings")]
    public float speed = 1.5f;

    public List<Shape4D> shapesAPublic;
    public List<Shape4D> shapesBPublic;
    private float wDirection;
    private List<Shape4D> shapesA;
    private List<Shape4D> shapesB;

    private CircleAnimation animation;

    private void Start()
    {
        animation = GameObject.Find("Player4DIndicator").GetComponent<CircleAnimation>();
        if (!animation.activate) animation.activate = true;
        AddShapes();
        SetShapes4DParameters();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Period) || Input.GetKey(KeyCode.Comma))
        {
            ChangeShapes4DParameters();
        }
    }

    void AddShapes()
    {
        GameObject A = GameObject.Find("A");
        shapesA = new List<Shape4D>(A.GetComponentsInChildren<Shape4D>());
        shapesA.AddRange(shapesAPublic);

        GameObject B = GameObject.Find("B");
        shapesB = new List<Shape4D>(B.GetComponentsInChildren<Shape4D>());
        shapesB.AddRange(shapesBPublic);

    }

    void SetShapes4DParameters()
    {
        for (int i = 0; i < shapesA.Count+shapesB.Count; i++)
        {
            if(i < shapesA.Count)
            {
                if (shapesA[i].changePosW)
                {
                    //keep the speed proportional to the scene time
                    shapesA[i].PosWChangeAmount = (shapesA[i].MaxPosW - shapesA[i].MinPosW) / ((timeAMaxW - timeAMinW) / speed);
                }

                if (shapesA[i].changeRotW)
                {
                    float RotXSpeed = (shapesA[i].MaxRotW.x - shapesA[i].MinRotW.x) / ((timeAMaxW - timeAMinW) / speed);
                    float RotYSpeed = (shapesA[i].MaxRotW.y - shapesA[i].MinRotW.y) / ((timeAMaxW - timeAMinW) / speed);
                    float RotZSpeed = (shapesA[i].MaxRotW.z - shapesA[i].MinRotW.z) / ((timeAMaxW - timeAMinW) / speed);
                    shapesA[i].RotWChangeAmount = new Vector3(RotXSpeed, RotYSpeed, RotZSpeed);
                }
            }
            if (i < shapesB.Count)
            {
                if (shapesB[i].changePosW)
                {
                    //keep the speed proportional to the scene time
                    shapesB[i].PosWChangeAmount = (shapesB[i].MaxPosW - shapesB[i].MinPosW) / ((timeBMaxW - timeBMinW) / speed);
                }

                if (shapesB[i].changeRotW)
                {
                    float RotXSpeed = (shapesB[i].MaxRotW.x - shapesB[i].MinRotW.x) / ((timeBMaxW - timeBMinW) / speed);
                    float RotYSpeed = (shapesB[i].MaxRotW.y - shapesB[i].MinRotW.y) / ((timeBMaxW - timeBMinW) / speed);
                    float RotZSpeed = (shapesB[i].MaxRotW.z - shapesB[i].MinRotW.z) / ((timeBMaxW - timeBMinW) / speed);
                    shapesB[i].RotWChangeAmount = new Vector3(RotXSpeed, RotYSpeed, RotZSpeed);
                    Debug.Log(shapesB[i].RotWChangeAmount);
                }
            }
            if (i >= shapesA.Count && i >= shapesB.Count) return;
        }
    }

    void ChangeShapes4DParameters()
    {
        if (Input.GetKey(KeyCode.Period)) wDirection = 1;
        else if (Input.GetKey(KeyCode.Comma)) wDirection = -1;
        for (int i = 0; i < shapesA.Count; i++)
        {
            if (shapesA[i].changePosW)
            {
                shapesA[i].positionW += wDirection * shapesA[i].PosWChangeAmount * Time.deltaTime;
                if (shapesA[i].positionW > shapesA[i].MaxPosW) shapesA[i].positionW = shapesA[i].MaxPosW;
                else if (shapesA[i].positionW < shapesA[i].MinPosW) shapesA[i].positionW = shapesA[i].MinPosW;
            }

            if (shapesA[i].changeRotW)
            {
                shapesA[i].rotationW += wDirection * shapesA[i].RotWChangeAmount * Time.deltaTime;
                if (shapesA[i].rotationW.x > shapesA[i].MaxRotW.x || shapesA[i].rotationW.y > shapesA[i].MaxRotW.y
                    || shapesA[i].rotationW.z > shapesA[i].MaxRotW.z) shapesA[i].rotationW = shapesA[i].MaxRotW;
                else if (shapesA[i].rotationW.x < shapesA[i].MinRotW.x || shapesA[i].rotationW.y < shapesA[i].MinRotW.y
                    || shapesA[i].rotationW.z < shapesA[i].MinRotW.z) shapesA[i].rotationW = shapesA[i].MinRotW;
            }

            //Shape B
            if(i == 1)
            {
                float BTerm = Mathf.InverseLerp(shapesA[i].MinPosW, shapesA[i].MaxPosW, Mathf.Abs(shapesA[i].positionW));
                if (BTerm == 0) BTerm = 1;

                for (int j = 0; j < shapesB.Count; j++)
                {
                    if (shapesB[j].changePosW)
                    {
                        shapesB[j].positionW += BTerm * wDirection * shapesB[j].PosWChangeAmount * Time.deltaTime;
                        Debug.Log(j);
                        Debug.Log(shapesB[j].positionW);
                        if (shapesB[j].positionW > shapesB[j].MaxPosW) shapesB[j].positionW = shapesB[j].MaxPosW;
                        else if (shapesB[j].positionW < shapesB[j].MinPosW) shapesB[j].positionW = shapesB[j].MinPosW;
                    }

                    if (shapesB[j].changeRotW)
                    {
                        shapesB[j].rotationW += BTerm * wDirection * shapesB[j].RotWChangeAmount * Time.deltaTime;
                        if (shapesB[j].rotationW.x > shapesB[j].MaxRotW.x || shapesB[j].rotationW.y > shapesB[j].MaxRotW.y
                            || shapesB[j].rotationW.z > shapesB[j].MaxRotW.z) shapesB[j].rotationW = shapesB[j].MaxRotW;
                        else if (shapesB[j].rotationW.x < shapesB[j].MinRotW.x || shapesB[j].rotationW.y < shapesB[j].MinRotW.y
                            || shapesB[j].rotationW.z < shapesB[j].MinRotW.z) shapesB[j].rotationW = shapesB[j].MinRotW;
                    }
                }
            }
        }
    }
}
