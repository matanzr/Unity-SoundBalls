using UnityEngine;
using System.Collections;

public class infinteRotation : MonoBehaviour {
	public float speed = 1;
	private Quaternion dest;
	private Quaternion origin;
	private float animTime;
	private float startTime;

	void setNewRotation(){
		startTime = Time.time;
		origin = dest;
		dest = Random.rotation;
		animTime = Random.Range (1, 4);
	}


	// Use this for initialization
	void Start () {
		dest = Random.rotation;
		setNewRotation ();
	}
	
	// Update is called once per frame
	void Update () {
		if ( (Time.time - startTime) * speed < animTime) {
			float fracTime = (Time.time - startTime) * speed / animTime;
			this.transform.rotation = Quaternion.Lerp (origin, dest, fracTime);
		} else {
			setNewRotation ();
		}
	}
}
