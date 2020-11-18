using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public GameObject deerNight2;
    public GameObject BearDay3;
    public GameObject[] bonesDay3;

    public AnimalEvent bearEvent;

    public float dayCount = 0;
    public bool nighttime = false;
    bool count = true;
    bool entered = false;

    public void DayIncrement()
    {
        if(count) dayCount += 0.5f;
    }

    private void Update()
    {
        if (dayCount == 1 && !entered) count = false;
        if (dayCount == 1.5)
        {
            entered = false;
            bearEvent.animals[bearEvent.animals.Length-1] = deerNight2;
        }
        if (dayCount == 2 && !entered) count = false;
        if(dayCount == 2.5 && entered)
        {
            for(int i = 0; i < bonesDay3.Length; i++)
            {
                bonesDay3[i].SetActive(true);
            }
            Destroy(deerNight2);
            Destroy(BearDay3);
            entered = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player");
            if((dayCount == 1 && nighttime) || (dayCount == 2 && nighttime))
            {
                Debug.Log("Entered day 2");
                entered = true;
                count = true;
            }
        }
    }


}
