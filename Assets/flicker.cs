using UnityEngine;
using System.Collections;

public class flicker : MonoBehaviour {
	public int rate = 10;
	float originalIntensity;
	bool isOn;

	// Use this for initialization
	void Start () {
		isOn = true;
	}
	
	// Update is called once per frame
	void Update () {
		Light light = GetComponent<Light>();
		if (Time.frameCount % rate == 0) {

			if (isOn) {
				originalIntensity = light.intensity;
				light.intensity = 0; 		
			} else {
				light.intensity = originalIntensity;
			}

			isOn = !isOn;
			
		}
		
	}
}
