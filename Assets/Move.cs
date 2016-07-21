using UnityEngine;
using System.Collections;

public enum Shapes {
	Circle,
	Line,
	AttractorPoint
}

public class Move : MonoBehaviour {
	public float circle_position;
	public float R = 6.5f;
	public Vector3 attractorPoint;
	Vector3 P;
	float u_val = 0;
	Shapes shape = Shapes.Circle;


	// Use this for initialization
	void Start () {
		P = new Vector3 (10, 10, 10);
		circle_position = 0;
	}

	void setCirclePosition(Vector3 p) {
		Vector2 xz = new Vector2 (p.x, p.z);
		circle_position = Mathf.Acos(Vector2.Dot (new Vector2 (1, 0), xz.normalized));

		circle_position /= 2 * Mathf.PI;
	}

	public void setUVal(float value) {
		u_val = value;
	}

	public float getUVal() {
		return u_val;
	}

	public void setShape(Shapes s) {
		shape = s;
	}

	Vector3 shapeNorm() {
		switch (shape) {
		case Shapes.Circle:
			float theta = Mathf.PI * 2 * u_val;
			return new Vector3 (
				Mathf.Sin( theta ) * R, 0 , Mathf.Cos(theta) * R
			);
		case Shapes.Line:
			return new Vector3 (
				2 * R * ( 2 * u_val - 1 ), 0 , 0
			);
			
		case Shapes.AttractorPoint:
			return attractorPoint;
		}


		return Vector3.zero;

	}

	// Update is called once per frame
	void Update () {
//		float magV = this.transform.position.magnitude;
//		Vector3 norm = this.transform.position / magV * R;
		Vector3 norm = shapeNorm();
		norm.y = 1.5f;

		Vector3 attPoint = (norm -  this.transform.position) * 0.75f;

		this.GetComponent<Rigidbody>().AddForce( attPoint * 15,ForceMode.Acceleration);

		this.setCirclePosition (this.transform.position);
	}
}
