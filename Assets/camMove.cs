using UnityEngine;
using System.Collections;

public class camMove : MonoBehaviour {
	Quaternion origin = Quaternion.identity;

	void Start()
	{
		Input.compensateSensors = true;
		Input.gyro.enabled = true;
	}

	void FixedUpdate()
	{
		transform.Rotate (-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.z);
	}
}