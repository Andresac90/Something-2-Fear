using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotationImage : MonoBehaviour
{
	[SerializeField]
    private float xRotation;
	[SerializeField]
    private float yRotation;
	[SerializeField]
    private float zRotation;
 	void Update ()
	{
		Vector3 eulerAngles = transform.eulerAngles;
		transform.eulerAngles = new Vector3( xRotation, yRotation, zRotation);
	}
}
