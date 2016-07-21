using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]	
public class SoundAnalyse : MonoBehaviour {
	float[] buffer;
	public float[] spectrum;
	int FFTSIZE = 2048;
	int effectiveSize;
	int SAMPLE_RATE = 44100;
	int MID_START_FREQ = 600;
	int MID_END_FREQ = 1000;
	int MAX_FREQ = 1500;

	int midStartIndex;
	int midEndIndex;
	int maxIndex;

	int binSize;

	// Use this for initialization
	void Start () {
//		audio = GetComponent<AudioSource>();
		GetComponent<AudioSource>().clip = Microphone.Start(null, true, 1, SAMPLE_RATE);
		GetComponent<AudioSource>().loop = true;
		while(!(Microphone.GetPosition (null) > 0) ) {}
		GetComponent<AudioSource>().Play(); // Play the audio source!

		binSize = (SAMPLE_RATE / 2) / FFTSIZE;
		maxIndex = (int)Mathf.Ceil(MAX_FREQ / binSize);
		midStartIndex = (int)Mathf.Ceil(MID_START_FREQ / binSize);
		midEndIndex = (int)Mathf.Ceil(MID_END_FREQ / binSize);

		effectiveSize = midStartIndex + (midEndIndex - midStartIndex) / 2 + (maxIndex - midEndIndex) / 4;

		spectrum = new float[effectiveSize];
		buffer = new float[FFTSIZE];
	}
		
	void Update () {
		GetComponent<AudioSource>().GetSpectrumData(buffer, 0, FFTWindow.BlackmanHarris);

		int bufferIndex = 0;

		for (int i = 0; i < effectiveSize; i++) {
			if (bufferIndex < midStartIndex) {
				spectrum [i] = buffer [i];
				bufferIndex++;
			} else if (bufferIndex < midStartIndex) {
				spectrum [i] = buffer [bufferIndex];
				spectrum [i] += buffer [bufferIndex+1];
				bufferIndex += 2;
			} else {
				spectrum [i] = buffer [bufferIndex];
				spectrum [i] += buffer [bufferIndex+1];
				spectrum [i] += buffer [bufferIndex+2];
				spectrum [i] += buffer [bufferIndex+3];
				bufferIndex += 4;
			}
		}

	}
}
