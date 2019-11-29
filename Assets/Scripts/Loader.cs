using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public TextAsset file;

    public void function()
    {
        // ...
        string[] lines = file.text.Split('\n');
        //...
    }
}