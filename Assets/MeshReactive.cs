using UnityEngine;
using System.Collections;

public class MeshReactive : MonoBehaviour {
	Vector3[] originalVertices;
	Vector3[] randomNormals;
	public int width;
	public int length;
	public GameObject soundSource;

	void mapSphere(Vector3[] pos, Vector2[] uvs) {
		float phi; 
		float theta;
		float radius = 1.0f;

		for (int i = 0; i < pos.Length; i++) {
			phi = uvs[i].x * Mathf.PI;
			theta = -uvs[i].y * Mathf.PI * 2;
			pos[i].x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
			pos[i].y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
			pos[i].z = radius * Mathf.Cos(phi);
		}
	}

	Mesh generatePlane() {
		int width1 = width + 1; 
		int length1 = length + 1;

		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		mf.mesh = mesh;

		Vector3[] vertices = new Vector3[width1 * length1];
		Vector3[] normals = new Vector3[width1 * length1];
		Vector2[] uvs = new Vector2[width1 * length1];
		int[] triangles = new int[6 * width * length];

		int k = 0;
		for (int i=0; i < width1; i++){
			for (int j=0; j < length1; j++){
				vertices[k] = new Vector3( (float)i / width, 0, (float)j / length);
				uvs[k] = new Vector2((float)j / width, (float)i / length);
				k++;
			}
		}

		k=0;
		for (int i=0; i < width; i++){
			for (int j=0; j < length; j++){

				// face vertices
				int a = i * length1 + j;
				int b = i * length1 + j + 1;
				int c = (i+1) * length1 + j;
				int d = (i+1) * length1 + j + 1;

				// face indices
				triangles[k] = a;
				triangles[k+1] = b;
				triangles[k+2] = c;

				triangles[k+3] = b;
				triangles[k+4] = d;
				triangles[k+5] = c;

				k += 6;
			}
		}

		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		return mesh;
	}

	// Use this for initialization
	void Start () {
		MeshFilter mf  = GetComponent<MeshFilter>();
		Mesh mesh = generatePlane();
		mf.mesh = mesh;

		Vector3[] vertices = mesh.vertices;
		Vector2[] uvs = mesh.uv;

		mapSphere(vertices, uvs);

		// save original vertices
		originalVertices = new Vector3[vertices.Length];
		for (int i = 0; i < vertices.Length; i++) {
			originalVertices[i].x = vertices[i].x;
			originalVertices[i].y = vertices[i].y;
			originalVertices[i].z = vertices[i].z;
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
//			float scale = 0.03f * Mathf.Sin(uvs[i].x * 7.0f + Time.time * 5) ;
//			scale += 0.035f * Mathf.Sin(uvs[i].y * 9.0f + Time.time * 7) ;
//
//			for (int k=3; k < 11; k+=4){
//				scale += (0.03f*k) * Mathf.Sin(uvs[i].x*9 * k  + (k + Time.time * 5)) ;
//				scale += (0.035f*k) * Mathf.Sin(uvs[i].y*7*k + (k + Time.time * 7) ) ;
//			}
//
//			scale *= scale * 0.7f * Mathf.Sin(Time.time*4+uvs[i].x*4);
			int index = (int)(spectrumRange * Mathf.Abs(uvs[i].x * 2 - 1));
			float scale = spectrum[index] * 10;

			vertices[i].x = originalVertices[i].x * (1 + scale * randomNormals[index].x);
			vertices[i].y = originalVertices[i].y * (1 + scale * randomNormals[index].y);
			vertices[i].z = originalVertices[i].z * (1 + scale * randomNormals[index].z);
		}

		mesh.vertices = vertices;
		mesh.RecalculateNormals();
	}
}
