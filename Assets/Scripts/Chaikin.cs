using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaikin : MonoBehaviour
{
    public LineRenderer line;
    [Range(0,5)]
    public int Iterations = 1;
    Vector3[] aux;
    public Vector3[] positions;
    public Vector3[] newPositions;
    int newPositionCount;
    // Start is called before the first frame update
    void Start()
    {
        aux = new Vector3[line.positionCount];
        line.GetPositions(aux);
    }

    private void Shake()
    {
        line.positionCount = aux.Length;
        line.SetPositions(aux);
        for (int k = 0; k < Iterations; k++)
        {
            positions = new Vector3[line.positionCount];
            line.GetPositions(positions);
            newPositionCount = positions.Length * 2;
            newPositions = new Vector3[newPositionCount];
            for (int i = 0; i < positions.Length; i++)
            {
                newPositions[(2 * i)] = (3f / 4f) * positions[i] + (1f / 4f) * positions[(i + 1) % positions.Length];

                newPositions[(2 * i) + 1] = (1f / 4f) * positions[i] + (3f / 4f) * positions[(i + 1) % positions.Length];
                // newPositions[]
            }
            positions = newPositions;
            line.positionCount = newPositionCount;
            line.SetPositions(positions); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(line.positionCount != Mathf.Pow(aux.Length,Iterations+1))
        Shake();
    }
}
