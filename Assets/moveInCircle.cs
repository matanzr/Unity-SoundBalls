using UnityEngine;
using System.Collections;

public class moveInCircle : MonoBehaviour {
	public float radius;
	Vector3 dest;
	Vector3 origin;
	float ttl;
	float interpolator;
	float step = 0.01f;

	// Use this for initialization
	void Start () {
		setNewDest();
		dest = new Vector3 (0, this.transform.position.y, 0);
	}

	void setNewDest() {
		interpolator = 0;
		float angle = Random.value * 2 * Mathf.PI;

		origin = new Vector3(dest.x, dest.y, dest.z);
		dest = new Vector3 (radius * Mathf.Sin (angle), this.transform.position.y, radius * Mathf.Cos (angle));
	}
	
	// Update is called once per frame
	void Update () {
		interpolator += step;

		if (interpolator < 1) {
			this.transform.position = Vector3.Lerp (origin, dest, interpolator);
		} else {
			setNewDest ();
		}
	}
}
