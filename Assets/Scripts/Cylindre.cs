using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylindre : MonoBehaviour
{
    public int nbMerdians = 3;
    public int nbDisques = 3;
    int n = 0;
    public float radius = 1;
    public float radius2 = 1;
    public float height = 2;
    float h = 0, r = 0,r2= 0;


    public List<Vector3> peakPos;
    public Material mat;
    public Vector3[] vertices;            // Création des structures de données qui accueilleront sommets et  triangles
    public int[] tris;

    MeshFilter mf;
    MeshRenderer mr;

    Mesh mesh;
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
        float teta = 0f;
        peakPos = new List<Vector3>();
        for (int i = 0; i < nbMerdians; i++)
        {
            teta = 2 * Mathf.PI * i / nbMerdians;
            print("teta = " + Mathf.Rad2Deg * teta);

            upperPeakPos = new Vector3(radius * Mathf.Cos(teta),height / 2,radius * Mathf.Sin(teta));
            lowerPeakPos = new Vector3(radius2 * Mathf.Cos(teta),-height / 2, radius2 * Mathf.Sin(teta));
            peakPos.Add(lowerPeakPos);
            peakPos.Add(upperPeakPos);
        }

        vertices = new Vector3[nbMerdians * 4 + 2];            // Création des structures de données qui accueilleront sommets et  triangles
        tris = new int[nbMerdians * 4 * 3];
        int k = 0;
        for (int i = 0; i < vertices.Length - 2; i += 4)
        {
            vertices[i] = peakPos[(k) % peakPos.Count];
            vertices[i + 1] = peakPos[(k + 2) % peakPos.Count];
            vertices[i + 2] = peakPos[(k + 1) % peakPos.Count];
            vertices[i + 3] = peakPos[(k + 3) % peakPos.Count];
            k += 2;
        }
        vertices[vertices.Length - 2] = new Vector3(0, height / 2, 0);
        vertices[vertices.Length - 1] = new Vector3(0, -height / 2, 0);
        k = 0;
        for (int i = 0; i < tris.Length/2; i += 6)
        {
            tris[i] = (k);
            tris[i + 1] = (k + 2);
            tris[i + 2] = (k + 1);
            tris[i + 3] = (k + 2);
            tris[i + 4] = (k + 3);
            tris[i + 5] = (k + 1);
            k += 4;
        }

        k = 2;
        for (int i = tris.Length / 2; i < tris.Length - tris.Length / 4; i += 3)
        {
            tris[i]     = (k)                   %vertices.Length;
            tris[i + 2] = (k + 1)               %vertices.Length;
            tris[i + 1] = (vertices.Length - 2) %vertices.Length;
            k += 4;      
        }
        k = 0;
        for (int i = tris.Length - tris.Length / 4; i < tris.Length ; i += 3)
        {
            tris[i+1] = (k) % vertices.Length;
            tris[i + 2] = (k + 1) % vertices.Length;
            tris[i ] = (vertices.Length - 1) % vertices.Length;
            k += 4;
        }

        mesh.vertices = vertices;
        mesh.triangles = tris;
        h = height; r = radius; r2 = radius2; n = nbMerdians;
        mesh.RecalculateNormals();
        mf.mesh = mesh;
        mr.material = mat;
    }


    // Update is called once per frame
    void Update()
    {
        if (height != h || radius != r || nbMerdians != n|| r2 != radius2)
        {
            Draw();
        }
        mesh.triangles = tris;
    }

}
