using UnityEngine;

public class LightPlacement : MonoBehaviour
{
    LightInScene lightInScene;
    Color lightSource;
    GameObject mainLight;

    private void Start()
    {
        lightInScene = GetComponent<LightInScene>();
        lightSource = lightInScene.lightSource;
        Instantiate(mainLight);

        //place on ceiling/lightsource with raycast
    }
}
