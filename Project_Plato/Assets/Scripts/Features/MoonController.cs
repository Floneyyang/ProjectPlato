using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonController : MonoBehaviour
{
    public Gradient gradientNight;
    public Gradient gradientGround;

    public float wSpeed = 1.5f;
    float wDirection;

    //night settings
    public float nightMaxW = 3.8f;
    public float nightMinW = 0f;

    //day settings
    public float dayMaxW = 0f;
    public float dayMinW = -2f;

    public GameObject[] shapesNight;
    public GameObject[] shapesDay;
    public GameObject[] ground;
    public CircleAnimation animation;

    public AnimalEvent deerEvent;
    public AnimalEvent bearEvent;
    public DayManager dayManager;

    private Camera cam;
    private Color color;
    private float previousDayW;
    private float previousNightW;

    private bool callday = true;
    private bool callnight = false;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Period)) wDirection = wSpeed;
        else if (Input.GetKey(KeyCode.Comma)) wDirection = -wSpeed;

        if (Input.GetKey(KeyCode.Period) || Input.GetKey(KeyCode.Comma))
        {
            if (!animation.activate) animation.activate = true;
            //Night
            for (int i = 0; i < shapesNight.Length; i++)
            {
                shapesNight[i].GetComponent<Shape4D>().positionW += wDirection * Time.deltaTime;
                if (shapesNight[i].GetComponent<Shape4D>().positionW > nightMaxW) shapesNight[i].GetComponent<Shape4D>().positionW = nightMaxW;
                else if (shapesNight[i].GetComponent<Shape4D>().positionW < nightMinW) shapesNight[i].GetComponent<Shape4D>().positionW = nightMinW;

                if (i == 0)
                {
                    float nightW = Mathf.Abs(shapesNight[i].GetComponent<Shape4D>().positionW);
                    float dayW = Mathf.Abs(shapesDay[i].GetComponent<Shape4D>().positionW);
                    float colorW = Mathf.InverseLerp(0, nightMaxW, nightW);
                    color = gradientNight.Evaluate(colorW);
                    cam.backgroundColor = color;

                    Color color2 = gradientGround.Evaluate(colorW);
                    for(int g = 0; g < ground.Length; g++)
                    {
                        ground[g].GetComponent<Shape4D>().color = color2;
                    }

                    if (previousNightW != nightW && nightW == nightMaxW)
                    {
                        deerEvent.AnimalAwake();
                        bearEvent.AnimalSleep();
                        if (callday)
                        {
                            dayManager.DayIncrement();
                            dayManager.nighttime = false;
                            callnight = true;
                            callday = false;
                        }

                    }
                    else if (previousNightW != nightW && nightW == nightMinW)
                    {
                        deerEvent.AnimalSleep();
                        bearEvent.AnimalAwake();
                        if (callnight)
                        {
                            dayManager.DayIncrement();
                            dayManager.nighttime = true;
                            callday = true;
                            callnight = false;
                        }
                    }

                    previousDayW = dayW;
                    previousNightW = nightW;

                    float dayTerm = Mathf.InverseLerp(0, nightMaxW, nightW);

                    for (int w = 0; w < shapesDay.Length; w++)
                    {
                        if (dayTerm == 0) dayTerm = 1;
                        shapesDay[w].GetComponent<Shape4D>().positionW += dayTerm * wDirection * Time.deltaTime;
                        if (shapesDay[w].GetComponent<Shape4D>().positionW > dayMaxW) shapesDay[w].GetComponent<Shape4D>().positionW = dayMaxW;
                        else if (shapesDay[w].GetComponent<Shape4D>().positionW < dayMinW) shapesDay[w].GetComponent<Shape4D>().positionW = dayMinW;
                        /*
                        if (shapesDay[w].GetComponent<Shape4D>().positionW == dayMaxW)
                        {
                            GameObject.FindWithTag("Door").GetComponent<NextLevelTrigger>().open = true;
                            //key.SetActive(true);
                        }
                        else
                        {
                            GameObject.FindWithTag("Door").GetComponent<NextLevelTrigger>().open = false;
                        }*/
                    }

                }


            }
        }
    }
}
