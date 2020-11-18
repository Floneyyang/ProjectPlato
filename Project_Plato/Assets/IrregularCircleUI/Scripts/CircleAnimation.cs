using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleAnimation : MonoBehaviour {

    public GameObject[] animObjects;
    public bool activate = false;
    public float wSpeed = 1f;
    float wDirection;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Period)) wDirection = -wSpeed;
        else if (Input.GetKey(KeyCode.Comma)) wDirection = wSpeed;

        if ((Input.GetKey(KeyCode.Period) || Input.GetKey(KeyCode.Comma)) && activate)
        {
            foreach (GameObject go in animObjects)
            {
                go.GetComponent<Image>().enabled = true;
                Vector3 angle = go.transform.eulerAngles;

                angle.z += Time.deltaTime * 50f * (wDirection*5f+1f);

                go.transform.eulerAngles = angle;
            }
        }

        if (Input.GetKeyUp(KeyCode.Period) || Input.GetKeyUp(KeyCode.Comma))
        {
            foreach (GameObject go in animObjects)
            {
                go.GetComponent<Image>().enabled = false;
            }
        }

    }
}
