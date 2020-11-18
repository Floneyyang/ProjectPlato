using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimalType
{
    Deer,
    Bear
}

public class AnimalEvent : MonoBehaviour
{
    public AnimalType type;
    public GameObject[] animals;
    public float wakeTimePerAnimal = 2f;

    private bool sleep = false;
    private bool reset = false;

    private int n = 1;

    public void AnimalAwake()
    {
        sleep = false;
        if (type == AnimalType.Deer) reset = true;
        StartCoroutine(callAnimal());
    }

    public void AnimalSleep()
    {
        sleep = true;
        for (int i = 0; i < animals.Length; i++)
        {
            if (animals[i] != null) animals[i].SetActive(false);
        }
    }

    IEnumerator callAnimal()
    {
        Debug.Log("called :" + n);
        for (int i = 0; i < animals.Length; i++)
        {
            if(animals[i] != null)
            {
                if (sleep) yield break;
                animals[i].SetActive(true);
                if (reset) animals[i].GetComponent<DeerAnimationController>().ResetPosition();
            }
            yield return new WaitForSeconds(wakeTimePerAnimal);
        }
        n++;
    }


}
