using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(GetComponent<MeshFilter>().mesh.vertices.Length);
        print(GetComponent<MeshFilter>().mesh.triangles.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
