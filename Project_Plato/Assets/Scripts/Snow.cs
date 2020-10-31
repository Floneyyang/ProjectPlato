using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
    public GameObject snow;
    public int number;
    public float instantiationRange = 10f;
    public float height;
    public float fallSpeed = 0.5f;
    public float maxSize = 1f;
    public float instantiateRate = 10f;
    public bool snowing = false;

    private float timer = 0f;
    private void Start()
    {
        timer = instantiateRate;
    }

    private void Update()
    {
        if (snowing)
        {
            timer += Time.deltaTime;
            if (timer >= instantiateRate)
            {
                timer = 0f;
                for (int i = 0; i < number; i++)
                {
                    Vector3 randomSpawnPosition = new Vector3(Random.Range(-instantiationRange, instantiationRange), Random.Range(height, height + 5f), Random.Range(-instantiationRange, instantiationRange));
                    Vector3 randomSpawnRotation = Vector3.up * Random.Range(0, 360);

                    GameObject newObject = (GameObject)Instantiate(snow, randomSpawnPosition, Quaternion.Euler(randomSpawnRotation));
                    float size = Random.Range(0.05f, maxSize);
                    newObject.transform.localScale = new Vector3(size, size, size);
                    newObject.AddComponent(typeof(DestroyOverHeight));
                    newObject.GetComponent<DestroyOverHeight>().fallSpeed = fallSpeed;
                    newObject.transform.parent = transform;

                }
            }
        }
        else
        {
            timer = instantiateRate;
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        
        
    }
}
