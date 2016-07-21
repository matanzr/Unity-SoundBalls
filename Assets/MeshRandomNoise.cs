using UnityEngine;
using System.Collections;

public class MeshRandomNoise : MonoBehaviour {
	Vector3[] originalVertices;
	Vector3[] randomNormals;
	public GameObject soundSource;
	public float intensity = 10;


	// Use this for initialization
	void Start () {
		MeshFilter mf  = GetComponent<MeshFilter>();
		Mesh mesh = mf.mesh;

		Vector3[] vertices = mesh.vertices;
		Vector2[] uvs = new Vector2[vertices.Length];

		// save original vertices
		originalVertices = new Vector3[vertices.Length];
		for (int i = 0; i < vertices.Length; i++) {
			originalVertices[i].x = vertices[i].x;
			originalVertices[i].y = vertices[i].y;
			originalVertices[i].z = vertices[i].z;
		}

		for (int i = 0; i < uvs.Length; i++) {
			uvs [i] = Random.insideUnitCircle;
		}

		// generate random normals
		randomNormals = new Vector3[vertices.Length];
		for (int i = 0; i < vertices.Length; i++) {
			randomNormals[i] = Random.insideUnitSphere;
		}

		// set bounds to prevernt culling
		float boundwidth = 500.0f;
		mesh.bounds = new UnityEngine.Bounds(new Vector3(0,0,0), new Vector3(1,1,1) * boundwidth);

		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update () {
		float[] spectrum = soundSource.GetComponent<SoundAnalyse>().spectrum;
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices  = mesh.vertices;
		Vector2[] uvs  = mesh.uv;

		float spectrumRange = (float)spectrum.Length - 1;

		for (int i = 0; i < vertices.Length; i++){
			int index = (int)(spectrumRange * Mathf.Abs(Mathf.Abs(uvs[i].x) * 2 - 1));
			if (Mathf.Abs (Mathf.Abs (uvs [i].x) * 2 - 1) < 0) {
				print (uvs [i].x);
			}
			float scale = spectrum[index] * intensity;

			vertices[i].x = originalVertices[i].x * (1 + scale * randomNormals[index].x);
			vertices[i].y = originalVertices[i].y * (1 + scale * randomNormals[index].y);
			vertices[i].z = originalVertices[i].z * (1 + scale * randomNormals[index].z);
		}

		mesh.vertices = vertices;
		mesh.RecalculateNormals();
	}
}

