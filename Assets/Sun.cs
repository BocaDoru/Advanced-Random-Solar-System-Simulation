using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float heatMultiplier;
    public float[,] heatMap = new float[5000,5000];
    public Texture2D texture2D;
    void Start()
    {
        texture2D = new Texture2D(5000, 5000);
        for (int y = 0; y < 5000; y++)
        {
            for (int x = y; x < 5000; x++)
            {
                heatMap[y, x] = Mathf.Abs(Mathf.Clamp01(x * x + y * y)-1);
                texture2D.SetPixel(x, y, Color.LerpUnclamped(Color.white, Color.black, x * x + y * y));
            }
        }
    }

    void Update()
    {
        
    }
}
