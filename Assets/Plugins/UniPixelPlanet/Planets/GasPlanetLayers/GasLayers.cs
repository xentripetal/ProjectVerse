using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GasLayers : MonoBehaviour{
    [SerializeField] private GameObject _GasLayers;
    [SerializeField] private GameObject _Ring;
    
    public Gradient GasColorScheme;
    public Gradient DarkGasColorScheme;
    public Gradient RingColorScheme;
    public Gradient DarkRingColorScheme;

    private Material m_GasLayers;
    private Material m_Ring;
    private string gradient_vars = "_ColorScheme";
    private string gradient_dark_vars = "_Dark_ColorScheme";
    private GradientColorKey[] colorKey1 = new GradientColorKey[3];
    private GradientColorKey[] colorKey2 = new GradientColorKey[3];
    
    private GradientAlphaKey[] alphaKey = new GradientAlphaKey[3];
    
    private string[] _colors1 = new[] {"#eec39a", "#d9a066", "#8f563b"};
    private string[] _colors2 = new[] {"#663931", "#45283c", "#222034"};
    private float[] _color_times = new float[] { 0, 0.5f, 1.0f };
    
    public float PixelSize = 96;
    public Vector2 LightPosition = Vector2.one;
    public float Seed = 124124;
    public float Rotation;
    
    private void Awake()
    {
        m_GasLayers = _GasLayers.GetComponent<SpriteRenderer>().sharedMaterial;
        m_Ring = _Ring.GetComponent<SpriteRenderer>().sharedMaterial;
    }

    public void Update() {
        UpdateTime(Time.time);
    }

    public void ApplyChanges() {
        m_GasLayers.SetFloat(ShaderProperties.Key_Pixels, PixelSize);
        m_Ring.SetFloat(ShaderProperties.Key_Pixels, PixelSize * _Ring.transform.localScale.x );
        
        m_GasLayers.SetVector(ShaderProperties.Key_Light_origin, LightPosition* 1.3f  );
        m_Ring.SetVector(ShaderProperties.Key_Light_origin, LightPosition* 1.3f );
        
        var converted_seed = Seed % 1000f / 100f;
        m_GasLayers.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Ring.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        
        m_GasLayers.SetFloat(ShaderProperties.Key_Rotation, Rotation);
        m_Ring.SetFloat(ShaderProperties.Key_Rotation, Rotation + 0.7f);
        
        setShaderGradient(GasColorScheme, m_GasLayers, gradient_vars);
        setShaderGradient(DarkGasColorScheme, m_GasLayers, gradient_dark_vars);
        
        setShaderGradient(RingColorScheme, m_Ring, gradient_vars);
        setShaderGradient(DarkRingColorScheme, m_Ring, gradient_dark_vars);
    }

    public void UpdateTime(float time)
    {
        m_GasLayers.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        m_Ring.SetFloat(ShaderProperties.Key_time, time  * 0.5f * -3f);
    }
    public void SetInitialColors()
    {
        setGragientColor();
    }

    private void setGragientColor()
    {
        for (int i = 0; i < colorKey1.Length; i++)
        {
            colorKey1[i].color = default;
            ColorUtility.TryParseHtmlString(_colors1[i], out colorKey1[i].color);

            colorKey1[i].time = _color_times[i];
            alphaKey[i].alpha = 1.0f;
            alphaKey[i].time = _color_times[i];
        }
        
        
        for (int i = 0; i < colorKey2.Length; i++)
        {
            colorKey2[i].color = default;
            ColorUtility.TryParseHtmlString(_colors2[i], out colorKey2[i].color);

            colorKey2[i].time = _color_times[i];
            alphaKey[i].alpha = 1.0f;
            colorKey2[i].time = _color_times[i];
        }
        
        GasColorScheme.SetKeys(colorKey1, alphaKey);
        DarkGasColorScheme.SetKeys(colorKey2, alphaKey);
        
        RingColorScheme.SetKeys(colorKey1, alphaKey);
        DarkRingColorScheme.SetKeys(colorKey2, alphaKey);
    }

    private void setShaderGradient(Gradient gradient, Material material, string key) {
        var texture = new Texture2D(128, 1);
        for (int h = 0; h < texture.height; h++)
        {
            for (int w = 0; w < texture.width; w++)
            {
                texture.SetPixel(w, h, gradient.Evaluate((float)w / texture.width));
            }
        }

        texture.Apply();
        texture.wrapMode = TextureWrapMode.Clamp;
        material.SetTexture(key, texture);
    }

}