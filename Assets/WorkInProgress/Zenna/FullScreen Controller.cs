using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class FullScreenController : MonoBehaviour
{
    [Header("Time Stats")]
    [SerializeField] private float _hurtDisplayTime = 1.5f;
    [SerializeField] private float _hurtFadeOutTime = 0.5f;

    [Header("References")]
    [SerializeField] private ScriptableRendererFeature _fullScreenDamage;
    [SerializeField] private Material _materialFire;

    [SerializeField] private Material _materialIce;

    //private int _voronoiIntensity = Shader.PropertyToID("_VoronoiIntensity");
   // private int _vignetteIntensity = Shader.PropertyToID("_VignetteIntensity");

    //private const float VORONOI_INTENSITY_START_AMOUNT = 1.25f;
   // private const float VIGNETTE_INTENSITY_START_AMOUNT = 1.25f;

    private void Start()
    {
        _fullScreenDamage.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            ShaderStart();
        }


    }

    public void ShaderStart()
    {
        _fullScreenDamage.SetActive(true);
    }
}
