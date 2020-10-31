using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	private Animator anim;
	private CharacterController controller;

	public float speed = 600.0f;
	public float turnSpeed = .2f;

	void Start()
	{
		controller = GetComponent<CharacterController>();
		anim = gameObject.GetComponentInChildren<Animator>();
	}

	void Update()
	{
		if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d"))
		{
			anim.SetInteger("AnimationPar", 1);
		}
		else
		{
			anim.SetInteger("AnimationPar", 0);
		}

		if (Input.GetAxis("Horizontal") > 0)
		{
			var desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, 90f, transform.eulerAngles.z);
			transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * turnSpeed);
		}
		else if (Input.GetAxis("Horizontal") < 0)
		{
			var desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, -90f, transform.eulerAngles.z);
			transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * turnSpeed);
		}
		else
		{

			if (Input.GetAxis("Vertical") > 0)
			{
				var desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
				transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * turnSpeed*10f);
			}
			else if (Input.GetAxis("Vertical") < 0)
			{
				var desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
				transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * turnSpeed*10f);
			}
		}


	}
}
