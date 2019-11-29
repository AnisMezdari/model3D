using System;
using System.Globalization;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class OFFMesh : MonoBehaviour {
	public TextAsset file;
	public Material mat;
	public Bbox bounding;
	Vector3 _gravityCenter = Vector3.zero;

	Vector3[] _vertices;
	int[] _tris;
	int _nbVertex, _nbTris;
	NumberStyles style = NumberStyles.Any;
	MeshFilter _mf;
	MeshRenderer _mr;
	Mesh _mesh;

	void Awake() {
		_mf = gameObject
			.GetComponent<MeshFilter>();
		_mr = gameObject.GetComponent<MeshRenderer>();

		string[] lines = file.text.Split('\n');

		if (lines[0].ToUpper().Equals("OFF")) {
			Debug.LogError("Not an Off Mesh");
			Destroy(gameObject);
		}

		string[] nbs = lines[1].Split(' ');
		_nbVertex = int.Parse(nbs[0]);
		_nbTris = int.Parse(nbs[1]);

		_vertices = new Vector3[_nbVertex];
		_tris = new int[_nbTris * 3];
		float biggestCoordinates = float.NegativeInfinity;
		float biggestCoordinatesX = float.NegativeInfinity;
		float biggestCoordinatesY = float.NegativeInfinity;
		float biggestCoordinatesZ = float.NegativeInfinity;

		float smallestCoordinatesX = float.PositiveInfinity;
		float smallestCoordinatesY = float.PositiveInfinity;
		float smallestCoordinatesZ = float.PositiveInfinity;
		for (int i = 0; i < _nbVertex; i++) {
			string[] vtx = lines[i + 2].Split(' ');
			
			decimal x = decimal.Parse(vtx[0], style, CultureInfo.InvariantCulture);
			decimal y = decimal.Parse(vtx[1], style, CultureInfo.InvariantCulture);
			decimal z = decimal.Parse(vtx[2], style, CultureInfo.InvariantCulture);
			_vertices[i] = (new Vector3((float) x, (float) y, (float) z));
			float Ax = Mathf.Abs((float) x);
			float Ay = Mathf.Abs((float) y);
			float Az = Mathf.Abs((float) z);

			biggestCoordinates = (Ax > biggestCoordinates) ? Ax : biggestCoordinates;
			biggestCoordinates = (Ay > biggestCoordinates) ? Ay : biggestCoordinates;
			biggestCoordinates = (Az > biggestCoordinates) ? Az : biggestCoordinates;
			biggestCoordinates = Mathf.Abs(biggestCoordinates);


			_gravityCenter += _vertices[i];
		}

		_gravityCenter /= _nbVertex;
		for (int i = 0; i < _vertices.Length; i++) {
			_vertices[i] -= _gravityCenter;
			_vertices[i] /= biggestCoordinates;

			biggestCoordinatesX = (_vertices[i].x > biggestCoordinatesX) ? _vertices[i].x : biggestCoordinatesX;
			biggestCoordinatesY = (_vertices[i].y > biggestCoordinatesY) ? _vertices[i].y : biggestCoordinatesY;
			biggestCoordinatesZ = (_vertices[i].z > biggestCoordinatesZ) ? _vertices[i].z : biggestCoordinatesZ;

			smallestCoordinatesX = (_vertices[i].x < smallestCoordinatesX) ? _vertices[i].x : smallestCoordinatesX;
			smallestCoordinatesY = (_vertices[i].y < smallestCoordinatesY) ? _vertices[i].y : smallestCoordinatesY;
			smallestCoordinatesZ = (_vertices[i].z < smallestCoordinatesZ) ? _vertices[i].z : smallestCoordinatesZ;
		}

		for (int i = 0; i < _nbTris; i++) {
			string[] vtx = lines[i + 2 + _nbVertex].Split(' ');
			_tris[3 * i] = (int.Parse(vtx[1]));
			_tris[3 * i + 1] = (int.Parse(vtx[2]));
			_tris[3 * i + 2] = (int.Parse(vtx[3]));
		}

		bounding = new Bbox(
			new Vector3(smallestCoordinatesX, smallestCoordinatesY, smallestCoordinatesZ),
			new Vector3(biggestCoordinatesX, biggestCoordinatesY, biggestCoordinatesZ)
		);

		_mesh = new Mesh {vertices = _vertices, triangles = _tris};
		_mesh.RecalculateNormals();
		_mf.mesh = _mesh;

		if (mat)
			_mr.material = mat;
		else
			_mr.material = new Material(Shader.Find("Diffuse"));
	}
}

public struct Bbox {
	public Vector3 min, max;

	public Bbox(Vector3 min, Vector3 max) {
		this.min = min - Vector3.one * 0.01f;
		this.max = max + Vector3.one * 0.01f;
	}
}