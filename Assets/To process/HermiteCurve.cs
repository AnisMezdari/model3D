using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
public class HermiteCurve : MonoBehaviour
{
    public Vector2 p0, p1, v0, v1;
    public float lineSize = 0.2f;
    public int nbPoints = 20;
    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRenderer.positionCount = nbPoints;
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float t = i / (nbPoints - 1.0f);
            /*  
            *  p(u)=F1(u)p0+F2(u)p1+F3(u)v0+F4(u)v1 avec :
            *  F1(u)=2u^3−3u^2+1
            *  F2(u)=−2u^3+3u^2
            *  F3(u)=u^3−2u^2+u
            *  F4(u)=u^3−u^2 
            */
            Vector3 posPoint = (2.0f * Mathf.Pow(t, 3) - 3.0f * Mathf.Pow(t, 2) + 1.0f) * p0
                + (Mathf.Pow(t, 3) - 2.0f * Mathf.Pow(t, 2) + t) * v0
                + (-2.0f * Mathf.Pow(t, 3) + 3.0f * Mathf.Pow(t, 2)) * p1
                + (Mathf.Pow(t, 3) - Mathf.Pow(t, 2)) * v1;
            lineRenderer.SetPosition(i, posPoint);
        }
    }
}