using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

public class FullScreenController : MonoBehaviour
{
    [Header("Time Stats")]
    [SerializeField] private float _hurtDisplayTime = 10.5f;
    [SerializeField] private float _hurtFadeOutTime = 10.5f;



    //Setting Full Screeen Renderer Feature in inspector
    [Header("References")]
    [SerializeField] public ScriptableRendererFeature FullScreenDamageFire;
    [SerializeField] public ScriptableRendererFeature FullScreenRendererPlant;
    [SerializeField] public ScriptableRendererFeature FullScreenRendererIce;
    [SerializeField] public ScriptableRendererFeature FullScreenRendererWind;
    [SerializeField] public ScriptableRendererFeature FullScreenRendererElectricity;
    [SerializeField] public ScriptableRendererFeature FullScreenRendererEarth;
    [SerializeField] public ScriptableRendererFeature FullScreenRendererHealing;

    //Below is work in progress for fading in and out the shader (Currently ignore)

    [SerializeField] private Material _materialFire;

    [SerializeField] private Material _materialIce;

    public FullScreenRenderer renderer;

    [Header("Intensity Stats")]

    [SerializeField] private float _voronoiIntensityStat = 2.5f;
    [SerializeField] private float _vignetteIntensityStat = 1.5f;

    private int _voronoiIntensity = Shader.PropertyToID("_VoronoiIntensity");
    private int _vignetteIntensity = Shader.PropertyToID("_VignetteIntensity");

    //private const float VORONOI_INTENSITY_START_AMOUNT = 1.25f;
   // private const float VIGNETTE_INTENSITY_START_AMOUNT = 1.25f;

    private void Start()
    {

        // Set all shaders to false

        /*
        FullScreenDamageFire.SetActive(false);
        FullScreenRendererPlant.SetActive(false);
        FullScreenRendererIce.SetActive(false);
        FullScreenRendererWind.SetActive(false);
        FullScreenRendererElectricity.SetActive(false);
        FullScreenRendererEarth.SetActive(false);
        FullScreenRendererHealing.SetActive(false);
        */
    }

    private void Update()
    {
        //Active shaders by keys for testing purposes

        if (Input.GetKeyDown("1"))
        {
            StartCoroutine(DamageFire());
        }

        if (Input.GetKeyDown("2"))
        {
            StartCoroutine(DamageIce());
        }

        if (Input.GetKeyDown("3"))
        {
            StartCoroutine(DamageWind());
        }

        if (Input.GetKeyDown("4"))
        {
            StartCoroutine(DamageElectricity());
        }

        if (Input.GetKeyDown("5"))
        {
            StartCoroutine(DamageEarth());
        }

        if (Input.GetKeyDown("6"))
        {
            StartCoroutine(DamagePlant());
        }

        if (Input.GetKeyDown("7"))
        {
            StartCoroutine(DamageHealing());
        }




    }

    //Functions for activating Full Screen renderer shaders for a few seconds

    private IEnumerator DamageFire()
    {
        FullScreenDamageFire.SetActive(true);
        //_materialFire.SetFloat(_voronoiIntensity, _voronoiIntensityStat);
        //_materialFire.SetFloat(_vignetteIntensity, _vignetteIntensityStat);
        Debug.Log("FIRE");
        float elapsedTime = 0f;
        while(elapsedTime < _hurtFadeOutTime)
        {
            elapsedTime += Time.deltaTime; 

            //float lerpedVoronoi = Mathf.Lerp(_voronoiIntensityStat, 0f, (elapsedTime / _hurtFadeOutTime));
            //float lerpedVignette = Mathf.Lerp(_vignetteIntensityStat, 0f, (elapsedTime / _hurtFadeOutTime));

            //_materialFire.SetFloat(_voronoiIntensity, _voronoiIntensity);
            //_materialFire.SetFloat(_vignetteIntensity, _vignetteIntensity);

            yield return null;
        }
        FullScreenDamageFire.SetActive(false);

    }

    private IEnumerator DamageIce()
    {
        FullScreenRendererIce.SetActive(true);
        Debug.Log("ICE");
        float elapsedTime = 0f;
        while (elapsedTime < _hurtFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        FullScreenRendererIce.SetActive(false);

    }

    private IEnumerator DamageWind()
    {
        FullScreenRendererWind.SetActive(true);
        Debug.Log("WIND");
        float elapsedTime = 0f;
        while (elapsedTime < _hurtFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        FullScreenRendererWind.SetActive(false);

    }

    private IEnumerator DamageElectricity()
    {
        FullScreenRendererElectricity.SetActive(true);
        Debug.Log("WIND");
        float elapsedTime = 0f;
        while (elapsedTime < _hurtFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        FullScreenRendererElectricity.SetActive(false);

    }

    private IEnumerator DamageEarth()
    {
        FullScreenRendererEarth.SetActive(true);
        Debug.Log("WIND");
        float elapsedTime = 0f;
        while (elapsedTime < _hurtFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        FullScreenRendererEarth.SetActive(false);

    }

    private IEnumerator DamagePlant()
    {
        FullScreenRendererPlant.SetActive(true);
        Debug.Log("WIND");
        float elapsedTime = 0f;
        while (elapsedTime < _hurtFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        FullScreenRendererPlant.SetActive(false);

    }

    private IEnumerator DamageHealing()
    {
        FullScreenRendererHealing.SetActive(true);
        Debug.Log("WIND");
        float elapsedTime = 0f;
        while (elapsedTime < _hurtFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        FullScreenRendererHealing.SetActive(false);

    }






    //Start of enum


    public enum FullScreenRenderer
    {
        Fire = 1,
        Ice = 2,
        Wind = 3,
        Electricity = 4,
        Earth = 5,
        Plant = 6
    }

    public void Action()
    {
        if(Input.GetKeyDown("1"))
        {
            switch (renderer)
            {
                case FullScreenRenderer.Fire:
                    Debug.Log("Fire");
                    StartCoroutine(DamageFire());

                    break;
                case FullScreenRenderer.Ice:
                    Debug.Log("Ice");
                    StartCoroutine(DamageIce());
                    break;
                case FullScreenRenderer.Wind:
                    Debug.Log("Wind");
                    break;
                case FullScreenRenderer.Electricity:
                    Debug.Log("Electricity");
                    break;
                case FullScreenRenderer.Earth:
                    Debug.Log("Eartj");
                    break;
                case FullScreenRenderer.Plant:
                    Debug.Log("Plant");
                    StartCoroutine(DamagePlant());
                    break;
            }
        }

        
    }

    

}
