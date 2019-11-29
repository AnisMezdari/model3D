using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public int nbMerdians = 3;
    public int nbDisques = 3;
    int n = 0;
    public float radius = 1;
    //public float radius2 = 1;
    public float height = 2;
    float h = 0, r = 0, r2 = 0;


    public List<Vector3> peakPos;
    public Material mat;
    public Vector3[] vertices;            // Création des structures de données qui accueilleront sommets et  triangles
    public int[] tris;

    MeshFilter mf;
    MeshRenderer mr;

    Mesh mesh;
    public int k;

    // Use this for initialization
    void Start()
    {

        mf = gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        mr = gameObject.AddComponent<MeshRenderer>();

        Draw();

    }

    private void Draw()
    {
        mesh = new Mesh();
        Vector3 upperPeakPos = Vector3.zero;
        Vector3 lowerPeakPos = Vector3.zero;
        float teta = 0f, phi = 0f;
        peakPos = new List<Vector3>();

        for (int j = 0; j <= nbDisques; j++)
        {
            for (int i = 0; i <= nbMerdians; i++)
            {

                teta = 2 * Mathf.PI * i / nbMerdians;
                if (i == nbMerdians)
                    teta = 2 * Mathf.PI ;
                phi = Mathf.PI * j / nbDisques;
                if (j == nbDisques)
                    phi = Mathf.PI;

                upperPeakPos = new Vector3(radius * Mathf.Cos(teta) * Mathf.Sin(phi), radius * Mathf.Cos(phi), radius * Mathf.Sin(teta) * Mathf.Sin(phi));

                peakPos.Add(upperPeakPos);
            }
        }
        vertices = new Vector3[4*nbMerdians*nbDisques];            // Création des structures de données qui accueilleront sommets et  triangles
        tris = new int[2 * 3 * nbMerdians * nbDisques];
        int k = 1,l=0;

        for (int i = 0; i < vertices.Length; i += 4)
        {

            vertices[i] = peakPos[((nbMerdians+1) * k)+l];  // 0 0 0
            vertices[i + 1] = peakPos[((nbMerdians + 1) * k )  + l+1]; // w 0 0
            vertices[i + 2] = peakPos[((nbMerdians + 1) * (k - 1))  + l]; // 0 h 0
            vertices[i + 3] = peakPos[((nbMerdians + 1) * (k - 1)) + l +1]; //w h 0
            if(l == (nbMerdians-1))
            k += 1 ;
            l = (l+1)% nbMerdians;
        }


        k = 0;
        for (int i = 0; i <tris.Length; i += 6)
        {
            tris[i] = (k)     /*% vertices.Length*/;
            tris[i + 1] = (k + 2) /*% vertices.Length*/;
            tris[i + 2] = (k + 1) /*% vertices.Length*/;
            tris[i + 3] = (k + 3) /*% vertices.Length*/;
            tris[i + 4] = (k + 1) /*% vertices.Length*/;
            tris[i + 5] = (k + 2) /*% vertices.Length*/;
            k += 4;
        }


        mesh.vertices = vertices;
        mesh.triangles = tris;
        h = nbDisques; r = radius; r2 = radius; n = nbMerdians;
        mesh.RecalculateNormals();
        mf.mesh = mesh;
        mr.material = mat;
    }


    // Update is called once per frame
    void Update()
    {
        //print(Mathf.Rad2Deg * (2 * Mathf.PI/ nbMerdians));
        if (nbDisques != h || radius != r || nbMerdians != n)
        {
            Draw();
        }
        //mesh.triangles = tris;
    }

    //private void OnDrawGizmos()
    //{
    //    int i= 0;
    //    foreach (Vector3 item in peakPos)
    //    {
    //        Gizmos.color = Color.yellow;

    //        Gizmos.DrawSphere(item + transform.position, .05f);
    //        ///Gizmos.DrawLine(item, item);

    //        i++;
    //        if (i == k) break;

    //    }
    //}
}
