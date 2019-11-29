using System;
using UnityEngine;


public class BezierCurve : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Vector3[] points;
    public float lineSize = 0.2f;
    public int nbPoints = 10;
    public BezierCurve otherCurve;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
    }

    public void Fusionner()
    {
        float distAB = Vector3.Distance(otherCurve.points[otherCurve.points.Length - 1], otherCurve.points[otherCurve.points.Length - 2]);
        Vector3 direction = Vector3.Normalize(otherCurve.points[otherCurve.points.Length - 1] - otherCurve.points[otherCurve.points.Length - 2]);
        points[0] = otherCurve.points[otherCurve.points.Length - 1];
        points[1] = points[0] + direction * distAB;
    }

    // Update is called once per frame
    void Update()
    {
        points = transform.parent.GetComponentInChildren<BezierCurvePoints>().points;
        lineRenderer.positionCount = nbPoints + 1;
        for (int i = 0; i < nbPoints; i++)
        {
            Vector3 p_i = GetPointAtT(points, (double)i / (double)nbPoints);
            p_i.z -= 0.001f;
            lineRenderer.SetPosition(i, p_i);
            lineRenderer.startWidth = lineSize;
            lineRenderer.endWidth = lineSize;
        }
        Vector3 p_n = points[points.Length - 1];
        p_n.z -= 0.001f;
        lineRenderer.SetPosition(nbPoints, p_n);
    }

    double BernsteinPolynom(int i, int n, double t)
    {
        double result = factorial(n) / (factorial(i) * factorial(n - i));
        result *= Math.Pow(t, i) * Math.Pow(1f - t, n - i);
        return result;
    }

    Vector3 GetPointAtT(Vector3[] points, double t)
    {
        int n = points.Length;
        Vector3 bPoint = Vector3.zero;
        for (int i = 0; i < points.Length; i++)
        {
            double b_i = BernsteinPolynom(i, n - 1, t);
            bPoint += new Vector3((float)(points[i].x * b_i), (float)(points[i].y * b_i), (float)(points[i].z * b_i));
        }
        return bPoint;
    }

    private double[] FactorialLookup;

    public BezierCurve()
    {
        CreateFactorialTable();
    }

    // just check if n is appropriate, then return the result
    private double factorial(int n)
    {
        if (n < 0) { throw new Exception("n is less than 0"); }
        if (n > 32) { throw new Exception("n is greater than 32"); }

        return FactorialLookup[n]; /* returns the value n! as a SUMORealing point number */
    }

    // create lookup table for fast factorial calculation
    private void CreateFactorialTable()
    {
        // fill untill n=32. The rest is too high to represent
        double[] a = new double[33];
        a[0] = 1.0;
        a[1] = 1.0;
        a[2] = 2.0;
        a[3] = 6.0;
        a[4] = 24.0;
        a[5] = 120.0;
        a[6] = 720.0;
        a[7] = 5040.0;
        a[8] = 40320.0;
        a[9] = 362880.0;
        a[10] = 3628800.0;
        a[11] = 39916800.0;
        a[12] = 479001600.0;
        a[13] = 6227020800.0;
        a[14] = 87178291200.0;
        a[15] = 1307674368000.0;
        a[16] = 20922789888000.0;
        a[17] = 355687428096000.0;
        a[18] = 6402373705728000.0;
        a[19] = 121645100408832000.0;
        a[20] = 2432902008176640000.0;
        a[21] = 51090942171709440000.0;
        a[22] = 1124000727777607680000.0;
        a[23] = 25852016738884976640000.0;
        a[24] = 620448401733239439360000.0;
        a[25] = 15511210043330985984000000.0;
        a[26] = 403291461126605635584000000.0;
        a[27] = 10888869450418352160768000000.0;
        a[28] = 304888344611713860501504000000.0;
        a[29] = 8841761993739701954543616000000.0;
        a[30] = 265252859812191058636308480000000.0;
        a[31] = 8222838654177922817725562880000000.0;
        a[32] = 263130836933693530167218012160000000.0;
        FactorialLookup = a;
    }
}
