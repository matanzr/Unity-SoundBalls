using UnityEngine;
using System.Collections;

public class HueChange : MonoBehaviour {
	Color[] colors = new Color[]{Color.blue, Color.cyan, Color.green, Color.red, Color.magenta, Color.yellow};
	float interpolator = 0;
	float step = 0.01f;
	Color current;
	Color dest;

	void setRandomColors() {
		current = dest;
		dest = colors [Random.Range (0, colors.Length)];
		interpolator = 0;
	}

	// Use this for initialization
	void Start () {
		setRandomColors ();
	}
	
	// Update is called once per frame
	void Update () {
		Light l = this.GetComponent<Light>();
		if (interpolator <= 1) {
			interpolator += step;
			l.color = Color.Lerp (current, dest, interpolator);
		} else {
			setRandomColors ();
		}
		
	}
}
