using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BezierCurve myTarget = (BezierCurve)target;
        DrawDefaultInspector();
        if (myTarget.otherCurve != null)
        {
            if (GUILayout.Button("Fusionner avec premiere courbe"))
            {
                myTarget.Fusionner();
            }
        }
    }
}
