using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HigerDimensionController : MonoBehaviour
{
    public GameObject[] shapesWx;
    public GameObject[] shapesW;
    public CircleAnimation animation;

    float origin;
    int c = 0;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if(!animation.activate) animation.activate = true;
            Debug.Log("Keydown");
            if(c == 0)
            {
                origin = transform.position.z;
                c++;
            }

            Debug.Log("origin" + origin);

            for (int i = 0; i < shapesWx.Length; i++)
            {
                Debug.Log("shapes before: " + shapesWx[i].GetComponent<Shape4D>().rotationW.x);
                Vector3 newRotation = new Vector3(shapesWx[i].GetComponent<Shape4D>().rotationW.x + (transform.position.z - origin)*Time.deltaTime, shapesWx[i].GetComponent<Shape4D>().rotationW.y,
                    shapesWx[i].GetComponent<Shape4D>().rotationW.z);
                shapesWx[i].GetComponent<Shape4D>().UpdateRotationW(newRotation);
                Debug.Log("shapes after: " + shapesWx[i].GetComponent<Shape4D>().rotationW.x);
            }

            for (int i = 0; i < shapesW.Length; i++)
            {
                shapesW[i].GetComponent<Shape4D>().positionW += (transform.position.z - origin) * Time.deltaTime;
                if (shapesW[i].GetComponent<Shape4D>().positionW > 1.2f) shapesW[i].GetComponent<Shape4D>().positionW = 1.2f;
                else if (shapesW[i].GetComponent<Shape4D>().positionW < -0.03f) shapesW[i].GetComponent<Shape4D>().positionW = -0.03f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) c = 0;

    }
}
