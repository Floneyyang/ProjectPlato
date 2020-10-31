using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public enum collectibleType
    {
        leave,
        sun,
        key
    };

    private HigerDimensionController leaveController;
    private SunController sunController;

    private void Start()
    {
        leaveController = GameObject.FindWithTag("Player").GetComponent<HigerDimensionController>();
        sunController = GameObject.FindWithTag("Player").GetComponent<SunController>();
    }
    public collectibleType type;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(type == collectibleType.leave)
            {
                leaveController.enabled = true;
            }
            if (type == collectibleType.sun)
            {
                sunController.enabled = true;
            }
            if (type == collectibleType.key)
            {
                GameObject.FindWithTag("Door").GetComponent<BoxCollider>().enabled = true;
            }
            Destroy(gameObject);
        }
    }
}
