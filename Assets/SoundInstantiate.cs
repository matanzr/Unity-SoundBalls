using UnityEngine;
using System.Collections;

public class SoundInstantiate : MonoBehaviour {

	// Use this for initialization
	public GameObject sphere;
	public GameObject soundSource;
	public int numSpheres;
	GameObject[] spheres;
	public Shapes shape = Shapes.Circle;
	public float radius = 6.5f;
	public Vector3 attractor;
	public float minSize;
	public float maxSize;

	float MIN_CUTOFF = 0.05f;

	void setShapes() {
		for (int i = 0; i < numSpheres; i++) {
			spheres [i].GetComponent<Move> ().setShape (shape);
		}
	}

	void setRadius() {
		for (int i = 0; i < numSpheres; i++) {
			spheres [i].GetComponent<Move> ().R = radius;
		}
	}

	void setAttractorPoint() {
		for (int i = 0; i < numSpheres; i++) {
			spheres [i].GetComponent<Move> ().attractorPoint = attractor;
		}	
	}

	void Start() {
		spheres = new GameObject[numSpheres];
		for (int y = 0; y < numSpheres; y++) {
			spheres[y] = Instantiate(sphere, new Vector3(2, y, 0), Quaternion.identity) as GameObject;
			spheres [y].transform.parent = this.transform;
			(spheres [y].GetComponent<Move> ()).setUVal (y / (float) numSpheres);
		}

		setRadius ();
		setShapes ();
		setAttractorPoint();
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3(this.transform.position.x, Mathf.Sin (Time.time), this.transform.position.z);
		this.transform.Rotate( new Vector3(Mathf.Sin (Time.time)*0.1f, 0, 0));
		float[] spectrum = soundSource.GetComponent<SoundAnalyse>().spectrum;
		float maxSpectrum = spectrum.Length - 1;

		if (spectrum.Length != 0) {
			int i = 0;

			for (i = 0; i < spheres.Length; i++) {
				float u = spheres [i].GetComponent<Move> ().getUVal ();
				int si = (int) (u * maxSpectrum);
				si = si < MIN_CUTOFF ? 0 : si;

				float s = Mathf.Max((2-u) * minSize + spectrum [si] * maxSize, spheres [i].transform.localScale.x * 0.9f);
				spheres [i].transform.localScale = new Vector3 (s,s,s);
			}
		}

	}
}
