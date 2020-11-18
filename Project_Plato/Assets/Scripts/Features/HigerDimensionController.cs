using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HigerDimensionController : MonoBehaviour
{
    public GameObject[] shapesWx;
    public GameObject[] shapesW;
    public CircleAnimation animation;
    public float wSpeed = 1.5f;
    public float wMax = 1.2f;
    public float wMin = -0.03f;
    float wDirection;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Period)) wDirection = wSpeed;
        else if (Input.GetKey(KeyCode.Comma)) wDirection = -wSpeed;

        if (Input.GetKey(KeyCode.Period) || Input.GetKey(KeyCode.Comma))
        {
            if(!animation.activate) animation.activate = true;
            Debug.Log("Keydown");

            if(shapesWx.Length != 0)
            {
                for (int i = 0; i < shapesWx.Length; i++)
                {
                    Debug.Log("shapes before: " + shapesWx[i].GetComponent<Shape4D>().rotationW.x);
                    Vector3 newRotation = new Vector3(shapesWx[i].GetComponent<Shape4D>().rotationW.x + wDirection * Time.deltaTime, shapesWx[i].GetComponent<Shape4D>().rotationW.y,
                        shapesWx[i].GetComponent<Shape4D>().rotationW.z);
                    shapesWx[i].GetComponent<Shape4D>().UpdateRotationW(newRotation);
                    Debug.Log("shapes after: " + shapesWx[i].GetComponent<Shape4D>().rotationW.x);
                }
            }

            if(shapesW.Length != 0)
            {

                for (int i = 0; i < shapesW.Length; i++)
                {
                    shapesW[i].GetComponent<Shape4D>().positionW += wDirection * Time.deltaTime;
                    if (shapesW[i].GetComponent<Shape4D>().positionW > wMax) shapesW[i].GetComponent<Shape4D>().positionW = wMax;
                    else if (shapesW[i].GetComponent<Shape4D>().positionW < wMin) shapesW[i].GetComponent<Shape4D>().positionW = wMin;
                }
            }

        }

    }
}
