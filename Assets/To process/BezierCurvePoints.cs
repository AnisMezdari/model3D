using System;
using System.Collections.Generic;
using UnityEngine;


public class BezierCurvePoints : MonoBehaviour
{
    public GameObject knobPrefab;
    LineRenderer lineRenderer;
    public Vector3[] points = {
        new Vector3(-2f, -2f, 0),
        new Vector3(-1f, 1f, 0),
        new Vector3(1f, 1f, 0),
        new Vector3(2f, -2f, 0)
    };
    public float lineSize = 0.2f;
    public int nbPoints = 10;
    
    List<GameObject> knobs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        foreach (Vector3 point in points)
        {
            GameObject knob = Instantiate(knobPrefab, point, new Quaternion(), transform);
            knob.transform.Translate(0, 0, -1f);
            knobs.Add(knob);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < knobs.Count; i++)
        {
            knobs[i].transform.position = points[i];
            knobs[i].transform.Translate(0, 0, -1f);
        }
        lineRenderer.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
            lineRenderer.startWidth = lineSize;
            lineRenderer.endWidth = lineSize;
        }
    }
}
