#pragma strict

var originalVertices : Vector3[];
var width: int;
var length: int;

function mapSphere(pos : Vector3[], uvs : Vector2[]) {
    var phi:float; 
    var theta:float;
    var radius:float = 1.0;

    for (var i = 0; i < pos.Length; i++) {
    	phi = uvs[i].x * Mathf.PI;
    	theta = -uvs[i].y * Mathf.PI * 2;
        pos[i].x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
        pos[i].y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
        pos[i].z = radius * Mathf.Cos(phi);
    }
}

function generatePlane() {
	var width1:int = width + 1; 
	var length1:int = length + 1;

    var mf: MeshFilter = GetComponent.<MeshFilter>();
    var mesh = new Mesh();
    mf.mesh = mesh;

    var vertices : Vector3[] = new Vector3[width1 * length1];
    var normals: Vector3[] = new Vector3[width1 * length1];
    var uvs : Vector2[] = new Vector2[width1 * length1];
    var triangles: int[] = new int[6 * width * length];

    var k:int = 0;
    for (var i:int=0; i < width1; i++){
        for (var j:int=0; j < length1; j++){
            vertices[k] = new Vector3( parseFloat(i) / width, 0, parseFloat(j) / length);
            uvs[k] = new Vector2(parseFloat(i) / width, parseFloat(j) / length);
            k++;
       }
    }

    k=0;
    for (i=0; i < width; i++){
        for (j=0; j < length; j++){

            // face vertices
            var a:int = i * length1 + j;
            var b:int = i * length1 + j + 1;
            var c:int = (i+1) * length1 + j;
            var d:int = (i+1) * length1 + j + 1;

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

function Start () {
	var mf: MeshFilter = GetComponent.<MeshFilter>();
    var mesh = generatePlane();
    mf.mesh = mesh;
    
	var vertices : Vector3[] = mesh.vertices;
	var uvs : Vector2[] = mesh.uv;
	originalVertices = new Vector3[vertices.Length];

	mapSphere(vertices, uvs);

	for (var i = 0; i < vertices.Length; i++) {
		originalVertices[i].x = vertices[i].x;
		originalVertices[i].y = vertices[i].y;
		originalVertices[i].z = vertices[i].z;
	}

	mesh.vertices = vertices;
	mesh.RecalculateNormals();
}


function Update () {
	var mesh : Mesh = GetComponent.<MeshFilter>().mesh;
	var vertices : Vector3[] = mesh.vertices;
	var uvs : Vector2[] = mesh.uv;

	for (var i = 0; i < vertices.Length; i++){
		var scale = 0.03 * Mathf.Sin(uvs[i].x * 7.0 + Time.time * 5) ;
        scale += 0.035 * Mathf.Sin(uvs[i].y * 9.0 + Time.time * 7) ;

        for (var k=3; k < 11; k+=4){
            scale += (0.03*k) * Mathf.Sin(uvs[i].x*9 * k  + (k + Time.time * 5)) ;
            scale += (0.035*k) * Mathf.Sin(uvs[i].y*7*k + (k + Time.time * 7) ) ;
        }

        scale *= scale * 0.7 * Mathf.Sin(Time.time*4+uvs[i].x*4);

		vertices[i].x = originalVertices[i].x * (1 + scale);
		vertices[i].y = originalVertices[i].y * (1 + scale);
		vertices[i].z = originalVertices[i].z * (1 + scale);
	}

	mesh.vertices = vertices;
	mesh.RecalculateNormals();
}