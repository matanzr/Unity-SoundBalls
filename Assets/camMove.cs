using UnityEngine;
using System.Collections;

public class camMove : MonoBehaviour {
	public float rotationMin = 0.5f, rotationMax = 2f;
	Quaternion origin = Quaternion.identity;
	Vector3 nextRotation = new Vector3();


	void Start()
	{
		Input.compensateSensors = true;
		Input.gyro.enabled = true;
	}

	public void FoundBeat() {
		//		if (nextRotation.x < 0.1f && nextRotation.y < 0.1f && nextRotation.z < 0.1f ) {
		//			Debug.Log("Boom");	
		nextRotation = new Vector3(Random.Range(0.5f, 2f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));
		//		}
	}


	void FixedUpdate()
	{
		transform.Rotate(nextRotation);
		nextRotation.x = Mathf.Lerp(nextRotation.x, 0, Time.deltaTime);
		nextRotation.y = Mathf.Lerp(nextRotation.y, 0, Time.deltaTime);
		nextRotation.z = Mathf.Lerp(nextRotation.z, 0, Time.deltaTime);


	}
}