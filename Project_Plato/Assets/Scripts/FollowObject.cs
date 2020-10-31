using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public bool enableFixedUpdate = false;

    protected Vector3 initialDistance;

    public GameObject initialObject;
    protected GameObject _followObject;
    public GameObject followObject
    {
        get
        {
            return _followObject;
        }
        set
        {
            if (_followObject != value)
            {
                _followObject = value;
                if (_followObject)
                {
                    initialDistance = transform.position - followObject.transform.position;
                }
            }
        }
    }

    void Start()
    {
        followObject = initialObject;
    }

    void LateUpdate()
    {
        _DoFollow();
    }

    void FixedUpdate()
    {
        if (!enableFixedUpdate)
        {
            return;
        }
        _DoFollow();
    }

    protected virtual void _DoFollow()
    {
        if (!followObject)
        {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, followObject.transform.position + initialDistance, 1f);
    }
}
