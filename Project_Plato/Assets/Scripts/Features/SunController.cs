using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    public GameObject[] shapesSpring;
    public GameObject[] shapesWinter;
    public Snow snowMaker;
    public GameObject ground;
    public GameObject[] plantWalls;
    public Gradient gradient;
    public Gradient gradientPlant;
    public GameObject key;
    float origin;
    int c = 0;

    Color color;

    //spring settings
    public float springMaxW = 3.8f;
    public float springMinW = 0f;

    //winter settings
    public float winterMaxW = 0f;
    public float winterMinW = -2f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Keydown");
            if (c == 0)
            {
                origin = transform.position.z;
                c++;
            }

            Debug.Log("origin" + origin);

            //Spring
            for (int i = 0; i < shapesSpring.Length; i++)
            {
                //Debug.Log("shapes before: " + shapesSpring[i].GetComponent<Shape4D>().rotationW.x);
                shapesSpring[i].GetComponent<Shape4D>().positionW += (transform.position.z - origin) * Time.deltaTime;
                if (shapesSpring[i].GetComponent<Shape4D>().positionW > springMaxW) shapesSpring[i].GetComponent<Shape4D>().positionW = springMaxW;
                else if (shapesSpring[i].GetComponent<Shape4D>().positionW < springMinW) shapesSpring[i].GetComponent<Shape4D>().positionW = springMinW;
                //Debug.Log("shapes after: " + shapesSpring[i].GetComponent<Shape4D>().rotationW.x);

                //Winter
                if (i == 0)
                {
                    float colorW = Mathf.InverseLerp(0, springMaxW, Mathf.Abs(shapesSpring[i].GetComponent<Shape4D>().positionW));
                    color = gradient.Evaluate(colorW);
                    ground.GetComponent<Shape4D>().color = color;

                    Color color2 = gradientPlant.Evaluate(colorW);
                    for (int j = 0; j < plantWalls.Length; j++)
                    {
                        plantWalls[j].GetComponent<Shape4D>().color = color2;
                    }

                    if(Mathf.Abs(shapesSpring[i].GetComponent<Shape4D>().positionW) == springMaxW)
                    {
                        snowMaker.snowing = true;
                    }
                    else
                    {
                        snowMaker.snowing = false;
                    }

                    float winterTerm = Mathf.InverseLerp(0, springMaxW, Mathf.Abs(shapesSpring[i].GetComponent<Shape4D>().positionW));
                    for (int w = 0; w < shapesWinter.Length; w++)
                    {
                        if (winterTerm == 0) winterTerm = 1;
                        shapesWinter[w].GetComponent<Shape4D>().positionW += winterTerm*(transform.position.z - origin) * Time.deltaTime;
                        if (shapesWinter[w].GetComponent<Shape4D>().positionW > winterMaxW) shapesWinter[w].GetComponent<Shape4D>().positionW = winterMaxW;
                        else if (shapesWinter[w].GetComponent<Shape4D>().positionW < winterMinW) shapesWinter[w].GetComponent<Shape4D>().positionW = winterMinW;

                        if(shapesWinter[w].GetComponent<Shape4D>().positionW == winterMaxW)
                        {
                            GameObject.FindWithTag("Door").GetComponent<NextLevelTrigger>().open = true;
                            key.SetActive(true);
                        }
                        else
                        {
                            GameObject.FindWithTag("Door").GetComponent<NextLevelTrigger>().open = false;
                        }
                    }
                }
            }

            //1.3
            //2



            /*
            for (int i = 0; i < shapesW.Length; i++)
            {
                Vector3 newRotation = new Vector3(shapesSpring[i].GetComponent<Shape4D>().rotationW.x + (transform.position.z - origin) * Time.deltaTime, shapesSpring[i].GetComponent<Shape4D>().rotationW.y,
                shapesSpring[i].GetComponent<Shape4D>().rotationW.z);
                shapesSpring[i].GetComponent<Shape4D>().UpdateRotationW(newRotation);


                shapesW[i].GetComponent<Shape4D>().positionW += (transform.position.z - origin) * Time.deltaTime;
                if (shapesW[i].GetComponent<Shape4D>().positionW > 1.2f) shapesW[i].GetComponent<Shape4D>().positionW = 1.2f;
                else if (shapesW[i].GetComponent<Shape4D>().positionW < -0.03f) shapesW[i].GetComponent<Shape4D>().positionW = -0.03f;
            }*/
        }

        if (Input.GetKeyUp(KeyCode.Space)) c = 0;

    }
}
