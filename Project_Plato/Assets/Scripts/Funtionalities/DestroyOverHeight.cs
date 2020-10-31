using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverHeight : MonoBehaviour
{
    public float height = -3f;
    public float fallSpeed;

    private void Update()
    {
        transform.Translate(Vector3.down * Random.Range(0.05f, fallSpeed), Space.World);
        if (transform.position.y <= height)
        {
            Destroy(gameObject);
        }
    }
}
