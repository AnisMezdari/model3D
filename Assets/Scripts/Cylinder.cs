using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    public Material material;
    public int subdivisionCount;
    public int sectionCount;
    public AnimationCurve radiusCurve;
    public float radiusFactor;
    public float height;

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        //Ensure that the Cylinder is buildable
        //At least 3 subdivisions
        if (subdivisionCount < 3)
            subdivisionCount = 3;

        //At least 2 sections
        if (sectionCount < 2)
            sectionCount = 2;

        // n vertices per section times m sections.
        Vector3[] vertices = new Vector3[subdivisionCount * sectionCount];

        float half_height = height / 2;
        float unit_angle = 360 / subdivisionCount;
        for (int v = 0; v < subdivisionCount; v++)
        {
            Quaternion rotation = Quaternion.Euler(0, v * unit_angle, 0);

            for (int s = 0; s < sectionCount; s++)
            {
                float t = s / (float) (sectionCount - 1);
                float height = Mathf.Lerp(-half_height, +half_height, t);
                float radius = radiusCurve.Evaluate(t) * radiusFactor;
                Vector3 point = rotation * new Vector3(radius, height, 0);
                vertices[v + s * subdivisionCount] = point;
            }
        }
        int sideQuadCount = subdivisionCount * sectionCount; // amount of quads (2 triangles) between 2 sections
        int indexCountPerQuad = 2 * 3; // 2 triangles
        int toppingTriangleCount = subdivisionCount - 2; // amount of triangles required to create faces at the top

        int[] indices = new int[(sideQuadCount * indexCountPerQuad) + 3 * (2 * toppingTriangleCount)]; 

        //Global indice walker
        int quadIndex = 0;

        //Draw a quad for each face (+ fuze face)
        for (int s = 0; s < sectionCount - 1; s++) // end before 
        {
            for (int v = 0; v < subdivisionCount; v++)
            {
                int v0 = ( ((v + 0) % subdivisionCount + (s + 0) * subdivisionCount) ) % vertices.Length;
                int v1 = ( ((v + 1) % subdivisionCount + (s + 0) * subdivisionCount) ) % vertices.Length;
                int v2 = ( ((v + 1) % subdivisionCount + (s + 1) * subdivisionCount) ) % vertices.Length;
                int v3 = ( ((v + 0) % subdivisionCount + (s + 1) * subdivisionCount) ) % vertices.Length;
                Debug.Log("indices " + v0 + " " + v1 + " " + v2 + " " + v3);
                //BR
                indices[quadIndex + 0] = v0;
                indices[quadIndex + 1] = v1;
                indices[quadIndex + 2] = v2;
                //TL
                indices[quadIndex + 3] = v2;
                indices[quadIndex + 4] = v3;
                indices[quadIndex + 5] = v0;
                quadIndex += 6;
            }
        }

        //Draw topping & bottoming faces
        int lastSectionOffset = subdivisionCount * (sectionCount - 1);
        for (int v = 2, vp = 1; v < subdivisionCount; v++, vp++)
        {
            Debug.Log("indices 0 " + vp + " " + v);
            
            //bottom triangle (facing down)
            indices[quadIndex + 0] = v;
            indices[quadIndex + 1] = vp;
            indices[quadIndex + 2] = 0; // v0

            //top triangle (facin up)
            indices[quadIndex + 0] = lastSectionOffset + 0; // v0
            indices[quadIndex + 1] = lastSectionOffset + vp;
            indices[quadIndex + 2] = lastSectionOffset + v;
            quadIndex += 6;
        }

        

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
    }


    private void OnDrawGizmos() {
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();

        if (mf != null)
        {
            Mesh mesh = mf.mesh;
            if (mesh != null)
            {
                Vector3[] vertices = mesh.vertices;
                if (vertices != null)
                {                
                    for (int i = 0; i < vertices.Length; ++i) {
                        Gizmos.DrawSphere(vertices[i], 0.1f);
                    }
                }
            }
        }
    }
}
