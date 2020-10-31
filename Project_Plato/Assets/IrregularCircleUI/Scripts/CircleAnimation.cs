using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleAnimation : MonoBehaviour {

    public GameObject[] animObjects;
    public bool activate = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space) && activate)
        {
            foreach (GameObject go in animObjects)
            {
                go.GetComponent<Image>().enabled = true;
                Vector3 angle = go.transform.eulerAngles;

                angle.z += Time.deltaTime * 50f * (Input.GetAxisRaw("Vertical")*5f+1f);

                go.transform.eulerAngles = angle;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (GameObject go in animObjects)
            {
                go.GetComponent<Image>().enabled = false;
            }
        }

    }
}
