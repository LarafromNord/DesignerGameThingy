using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class LightInScene : MonoBehaviour
{
    public RenderTexture rt;

    private Texture2D tex;
    private Rect rectReadPicture;
    bool scanActive = true;
    Color pixel;
    public Color whitestPixel;

    List<Color> pixels = new List<Color>();

    private void Awake()
    {
        //Makes a 2D texture
        tex = new Texture2D(rt.width, rt.height);

        //stores texture size
        rectReadPicture = new Rect(0, 0, rt.width, rt.height);
    }
    private void Start()
    {
        if (scanActive)
        {
            RenderTexture.active = rt;

            //reads rt
            tex.ReadPixels(rectReadPicture, 0, 0);
            tex.Apply();

            Debug.Log(tex.GetPixel(0, 0));
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    pixels.Add(tex.GetPixel(x, y));
                }
            }
            for (int i = 0; i < pixels.Count; i++)
            {
                if (whitestPixel == null)
                {
                    whitestPixel = pixels[i];
                    break;
                }

                if (IsBrighter(pixels[i], whitestPixel))
                {
                    whitestPixel = pixels[i];
                }
            }

            RenderTexture.active = null;
            rt.Release();
        }
    }

    bool IsBrighter(Color toCheck, Color toCompare)
    {
        Color.RGBToHSV(toCheck, out float toCheckH, out float toCheckS, out float toCheckV);
        Color.RGBToHSV(toCompare, out float toCompareH, out float toCompareS, out float toCompareV);

        return toCheckV >= toCompareV;
    }
}
