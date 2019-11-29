using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AABB : MonoBehaviour
{

    [SerializeField]
    public List<Sphere> mySpheres;
    
    public float cubeSize;
    [SerializeField]
    List<GameObject> myCubes;

    [SerializeField]
    Vector3 Bmin, Bmax;
    public float threshold;
    public float epsilon;

    public bool Implicite;

    //int nVertex = 2*2*2;
    // Start is called before the first frame update
    void Awake()
    {

        Bmin = mySpheres.Select(s => s.centre - s.radius * Vector3.one).Aggregate(Vector3.positiveInfinity,
            (vec1, vec2) =>
            {
                return new Vector3(Mathf.Min(vec1.x, vec2.x), Mathf.Min(vec1.y, vec2.y), Mathf.Min(vec1.z, vec2.z));
            });
        Bmin = Bmin - Vector3.one * cubeSize;
        Bmax = mySpheres.Select(s => s.centre + s.radius * Vector3.one).Aggregate(Vector3.negativeInfinity,
            (vec1, vec2) =>
            {
                return new Vector3(Mathf.Max(vec1.x, vec2.x), Mathf.Max(vec1.y, vec2.y), Mathf.Max(vec1.z, vec2.z));
            });



        for (int i = 0; i <= ((Bmax.x-Bmin.x)/cubeSize)+1; i++)
        {
            for (int k = 0; k <= ((Bmax.y - Bmin.y) / cubeSize)+1 ; k++)
            {
                for (int l = 0; l <= ((Bmax.z - Bmin.z) / cubeSize) +1; l++)
                {
                    var Min = new Vector3(i*cubeSize + Bmin.x, k * cubeSize + Bmin.y, l * cubeSize + Bmin.z);
                    if (Implicite)
                    {
                        if (ItersectBoxSphere(Min, mySpheres))
                        {
                            myCubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                            myCubes[myCubes.Count - 1].transform.position = Min;
                            myCubes[myCubes.Count - 1].transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
                        }
                    }
                    else
                    {
                        if (ImpliciteSurface(Min, mySpheres))
                        {
                            myCubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                            myCubes[myCubes.Count - 1].transform.position = Min;
                            myCubes[myCubes.Count - 1].transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
                        }
                    }

                }
            }
        }

    }

    private bool ImpliciteSurface(Vector3 min, List<Sphere> mySpheres)
    {
        float influence = 0;
        Vector3 centre = min + Vector3.one*.5f * cubeSize;
        foreach (var sph in mySpheres)
        {
            var X = (Vector3.Distance(sph.centre, centre) - sph.radius);
            influence += (Mathf.Exp(-X));
        }
        //Debug.Log(influence);
        return influence > threshold;
    }

    private bool ItersectBoxSphere(Vector3 min,List<Sphere> spheres)
    {
        bool intersect = false;
        Vector3 max = min + Vector3.one *  cubeSize;
        foreach (var sph in spheres)
        {
            float dist_squared = squared(sph.radius);
            if (sph.centre.x < min.x) dist_squared -= squared(sph.centre.x - min.x);
            else if (sph.centre.x > max.x) dist_squared -= squared(sph.centre.x - max.x);
            if (sph.centre.y < min.y) dist_squared -= squared(sph.centre.y - min.y);
            else if (sph.centre.y > max.y) dist_squared -= squared(sph.centre.y - max.y);
            if (sph.centre.z < min.z) dist_squared -= squared(sph.centre.z - min.z);
            else if (sph.centre.z > max.z) dist_squared -= squared(sph.centre.z - max.z);


            intersect = intersect || dist_squared >= 0 ;
        }
        return intersect;
    }

    private float squared(float v)
    {
        return v * v;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    [Serializable]
    public class Sphere
    {
        public Vector3 centre;
        public float radius;
    }

}

