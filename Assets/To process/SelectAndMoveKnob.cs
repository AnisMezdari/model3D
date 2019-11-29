using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAndMoveKnob : MonoBehaviour
{
    public int selectedIndex = 0;
    SpriteRenderer[] knobsRenderer;
    Vector3[] points;
    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        points = transform.parent.GetComponentInChildren<BezierCurvePoints>().points;
        knobsRenderer = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < knobsRenderer.Length; i++)
        {
            if (Input.GetKeyDown("["+i+"]"))
            {
                selectedIndex = i;
            }
        }

        UpdateKnobsColor();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            points[selectedIndex].x -= Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            points[selectedIndex].x += Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            points[selectedIndex].y += Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            points[selectedIndex].y -= Time.deltaTime * speed;
        }
    }

    void UpdateKnobsColor()
    {
        for (int i = 0; i < knobsRenderer.Length; i++)
        {
            knobsRenderer[i].color = (i == selectedIndex) ? Color.red : Color.black;
        }
    }
}
