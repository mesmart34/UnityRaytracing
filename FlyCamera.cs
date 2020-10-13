using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
	public float mainSpeed = 10.0f;
	public float camSens = 2;
	private Vector2 rotation = Vector2.zero;

	void Awake()
	{
		Debug.Log("FlyCamera Awake() - RESETTING CAMERA POSITION"); // nop?
																	// nop:
																	//transform.position.Set(0,8,-32);
																	//transform.rotation.Set(15,0,0,1);
		transform.position = new Vector3(0, 8, -32);
		transform.rotation = Quaternion.Euler(25, 0, 0);
	}


	void Update()
	{
		var dx = Input.GetAxis("Mouse X") * camSens;
		var dy = -Input.GetAxis("Mouse Y") * camSens;
		rotation.y += dx;
		rotation.x += dy;
		rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
		var step = GetBaseInput() * mainSpeed * Time.deltaTime;					
		//if(step.sqrMagnitude + Mathf.Abs(dx) + Mathf.Abs(dy) > 0.02f)
        {
			transform.eulerAngles = new Vector2(0, rotation.y);
			transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
			transform.Translate(step, transform);
		}			
	}

	private Vector3 GetBaseInput()
	{
		Vector3 p_Velocity = new Vector3();
		if (Input.GetKey(KeyCode.W))
		{
			p_Velocity += new Vector3(0, 0, 1);
		}
		if (Input.GetKey(KeyCode.S))
		{
			p_Velocity += new Vector3(0, 0, -1);
		}
		if (Input.GetKey(KeyCode.A))
		{
			p_Velocity += new Vector3(-1, 0, 0);
		}
		if (Input.GetKey(KeyCode.D))
		{
			p_Velocity += new Vector3(1, 0, 0);
		}
		return p_Velocity;
	}
}
