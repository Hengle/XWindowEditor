using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StpTesr {

    [MenuItem("Test/Open")]
    static void Open()
    {
        string path = EditorUtility.OpenFilePanel("", "", "jpg");
        if (string.IsNullOrEmpty(path))
            return;
        byte[] buffer = System.IO.File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(1,1);
        tex.LoadImage(buffer);
        tex.Apply();

        for (int i = tex.height - 1; i >= 0; i--)
        {
            string line = "";
            for (int j = 0; j < tex.width; j++)
            {
                Color col = tex.GetPixel(j, i);
                float l = col.r*0.22f + col.g*0.707f + col.b*0.071f;
                if (l < 0.4f)
                {
                    line += "▉▉▉▉";
                }
                else
                {
                    line += "       ";
                }
            }
            Debug.Log(line);
        }
    }
}
