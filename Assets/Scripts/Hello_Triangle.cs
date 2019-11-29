using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_Triangle : MonoBehaviour
{
    [Range(1, 10)]
    public int nbRows;
    [Range(1, 10)]
    public int nbCol;

    [Range(1, 100)]
    public int LevelOfDetail;
    int nbTri;
    public Material mat;
    public Vector3[] vertices = new Vector3[6];            // Création des structures de données qui accueilleront sommets et  triangles
    public int[] triangles = new int[6];

    Vector3 point;
    Vector3 point1;
    Vector3 point2;
    Vector3 point3;

    MeshFilter mf;
    MeshRenderer mr;

    Mesh msh;
    // Use this for initialization
    void Start()
    {
        mf = gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        mr = gameObject.AddComponent<MeshRenderer>();

        UpdateMesh();
    }

    private void UpdateMesh()
    {

        if (nbTri !=  nbRows * nbCol * 3 * 2 || true)
        {
            nbTri =  nbRows * nbCol * 3 * 2;
            vertices = new Vector3[nbTri];            // Création des structures de données qui accueilleront sommets et  triangles
            triangles = new int[nbTri];
            int k = 0;
            for (int i = 0; i < nbRows; i++)
            {
                for (int j = 0; j < nbCol; j++)
                {
                    for (int m = 0; m < 1; m++)
                    {
                        point= new Vector3(i * LevelOfDetail, 0, j * LevelOfDetail);
                        point1 = new Vector3(i * LevelOfDetail, 0, (j + 1) * LevelOfDetail);
                        point2 = new Vector3((i + 1) * LevelOfDetail, 0, j * LevelOfDetail);
                        point3 = new Vector3((i + 1) * LevelOfDetail, 0, (j + 1) * LevelOfDetail);

                        vertices[k] = point;          // Remplissage de la structure sommet 
                        vertices[k + 1] = point1;
                        vertices[k + 2] = point2;
                        vertices[k + 3] = point3;            // Remplissage de la structure sommet 
                        vertices[k + 4] = point2;
                        vertices[k + 5] = point1;

                        triangles[k] = k;                               // Remplissage de la structure triangle. Les sommets sont représentés par leurs indices
                        triangles[k + 1] = k + 1;                               // les triangles sont représentés par trois indices (et sont mis bout à bout)
                        triangles[k + 2] = k + 2;
                        triangles[k + 3] = k + 3;                               // Remplissage de la structure triangle. Les sommets sont représentés par leurs indices
                        triangles[k + 4] = k + 4;                               // les triangles sont représentés par trois indices (et sont mis bout à bout)
                        triangles[k + 5] = k + 5;

                        k += 6;
                    }




                }
            }


            msh = new Mesh();                          // Création et remplissage du Mesh

            msh.vertices = vertices;
            msh.triangles = triangles;

            mf.mesh = msh;           // Remplissage du Mesh et ajout du matériel
            mr.material = mat;
        }

    }

    private void Update()
    {

       UpdateMesh();


    }
}