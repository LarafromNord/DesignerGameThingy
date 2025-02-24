using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class LightInScene : MonoBehaviour
{
    [SerializeField] bool environmentReflections;
    [SerializeField] float reflectionRefreshRate;
    [SerializeField] VolumeProfile skyboxProfile;
    [SerializeField] Cubemap defaultSky;
    [SerializeField] List<PlanarReflectionProbe> OnCubemapUpdate = new List<PlanarReflectionProbe>();

    private bool isUpdating = false;

    void Start()
    {
        if (environmentReflections)
        {
            StartReflectionUpdates();
        }
    }

    void OnEnable()
    {
        if (environmentReflections)
        {
            StartReflectionUpdates();
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void StartReflectionUpdates()
    {
        if (!isUpdating)
        {
            StartCoroutine(UpdateReflections());
        }
    }

    private IEnumerator UpdateReflections()
    {
        isUpdating = true;

        while (environmentReflections)
        {
            // Update Skybox reflections
            UpdateSkyboxReflection();

            // Update Planar Reflection Probes
            foreach (var probe in OnCubemapUpdate)
            {
                if (probe != null)
                    probe.RequestRenderNextUpdate();
            }

            yield return new WaitForSeconds(reflectionRefreshRate);
        }

        isUpdating = false;
    }

    private void UpdateSkyboxReflection()
    {
        if (skyboxProfile != null)
        {
            Cubemap newSkybox = GetRealWorldSkybox();
            if (newSkybox != null)
            {
                skyboxProfile.TryGet(out UnityEngine.Rendering.HighDefinition.HDRISky hdriSky);
                if (hdriSky != null)
                {
                    hdriSky.hdriSky.overrideState = true;
                    hdriSky.hdriSky.value = newSkybox;
                }
            }
        }
    }

    private Cubemap GetRealWorldSkybox()
    {
        // Placeholder for grabbing Varjo XR-3 pass-through skybox data
        // Varjo SDK might have a function to generate a cubemap from real-world light
        
        return defaultSky;
    }

    public void SetEnvironmentReflections(bool enable)
    {
        environmentReflections = enable;

        if (enable)
            StartReflectionUpdates();
        else
            StopAllCoroutines();
    }
}
