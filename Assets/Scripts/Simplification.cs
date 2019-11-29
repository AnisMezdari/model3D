using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(OFFMesh))]
public class Simplification : MonoBehaviour {
	public float cellSize;

	private Dictionary<Vector3Int, List<int>> _cells = new Dictionary<Vector3Int, List<int>>();
	private MeshFilter _mf;
	private MeshRenderer _mr;
	private Mesh _mesh;
	[SerializeField] private OFFMesh _offMesh;

	private float  _minX, _minY, _minZ;

	private Vector3 _newMin;
	private Vector3Int[] _vertexCells;

	// Start is called before the first frame update
	void Awake() {
		_mf = gameObject.GetComponent<MeshFilter>();
		_mr = gameObject.GetComponent<MeshRenderer>();
		_offMesh = GetComponent<OFFMesh>();
	}

	private void Start() {
		var mesh = _mf.mesh;
		_mesh = mesh;
		_vertexCells = new Vector3Int[mesh.vertices.Length];


		_minX = _offMesh.bounding.min.x - cellSize + _offMesh.bounding.min.x % cellSize;
		_minY = _offMesh.bounding.min.y - cellSize + _offMesh.bounding.min.y % cellSize;
		_minZ = _offMesh.bounding.min.z - cellSize + _offMesh.bounding.min.z % cellSize;
		_newMin = new Vector3(_minX, _minY, _minZ);
        var time = Time.realtimeSinceStartup; 
		SimplifyMesh();
        Debug.Log(Time.realtimeSinceStartup - time);


		//Debug.Log(_offMesh.bounding.min.x + "  " + _offMesh.bounding.min.y + "  " + _offMesh.bounding.min.z);
		//Debug.Log(GetBoxMin(_offMesh.bounding.max));

		//Debug.Log(GetBoxMin(new Vector3(_minX, _minY, _minZ)));


		//Debug.Log(GetBoxMin(new Vector3(_minX + 0.01f, _minY, _minZ)));
		//Debug.Log(GetBoxMin(new Vector3(_minX + 0.07f, _minY, _minZ)));
	}

	private void SimplifyMesh() {
		int i = 0;
		foreach (Vector3 vertex in _mf.mesh.vertices) {
			Vector3Int res = GetBoxMin(vertex);
			_vertexCells[i] = res;
			if (_cells.TryGetValue(res, out List<int> value)) {
				value.Add(i++);
			}
			else {
				value = new List<int> {i++};
				_cells[res] = value;
			}
		}


        foreach (var cell in _cells.Keys)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.parent = transform;
            go.transform.localScale = cellSize * Vector3.one;
            go.transform.position = cellSize * (Vector3)cell + new Vector3(_minX, _minY, _minZ) +
                                    cellSize * 0.5f * Vector3.one;
        }
        var triList = _mesh.triangles.ToList();
        foreach (var list in _cells.Values)
        {
            if (list.Count > 1)
            {
                _mesh.triangles = _mesh.triangles.Select(x =>
                {
                    if (list.Contains(x))
                        return list[0];
                    return x;
                }).ToArray();
            }
        }


        var trisList = _mesh.triangles.Select(x => _cells[_vertexCells[x]][0])
                               .ToArray();
        //List<int> trisList = new List<int>();

        //for (int index = 0; index < trisList.Count; index++)
        //{
        //    if (trisList[index] != trisList[index - 1] && trisList[index] != trisList[index - 2] &&
        //        trisList[index - 1] != trisList[index - 2])
        //    {

        //        trisList.Add(index);
        //        trisList.Add(index - 1);
        //        trisList.Add(index - 2);
        //    }
        //}


        for (int index = trisList.Length - 1; index > 3; index -= 3)
        {
            if (trisList[index] != trisList[index - 1] && trisList[index] != trisList[index - 2] &&
                trisList[index - 1] != trisList[index - 2])
            {
                continue;
            }

            trisList[index] = -1;
            trisList[index - 1] = -1;
            trisList[index - 2] = -1;
        }
        _mesh.triangles = trisList.Where(x=> x!=-1).ToArray();
	}

	private Vector3Int GetBoxMin(Vector3 point) {
		return Vector3Int.FloorToInt((point - _newMin) / cellSize);
	}

	private bool IsInsideBox(Vector3 point, Vector3 min, Vector3 max) {
		return point.x < max.x && point.x > min.x
		                       && point.y < max.y && point.y > min.y
		                       && point.z < max.z && point.z > min.z;
	}
}