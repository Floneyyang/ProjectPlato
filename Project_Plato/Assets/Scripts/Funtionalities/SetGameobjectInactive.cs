using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameobjectInactive : MonoBehaviour
{
    public GameObject inactiveObject;

    bool first = true;

    private void Update()
    {
        if (this.gameObject.activeSelf && first)
        {
            first = false;
            Destroy(inactiveObject);
        }
    }
}
