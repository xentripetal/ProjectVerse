using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LandRivers : MonoBehaviour {
    [SerializeField] private GameObject Land;
    [SerializeField] private GameObject Cloud;
    
    private Material m_Land;
    private Material m_Cloud;
    
    public float PixelSize = 96;
    public Vector2 LightPosition = Vector2.one;
    public float Seed = 124124;
    public float Rotation;
    public Color[] Colors = { };

    private static string[] color_vars1 = new string[]{"_Color1", "_Color2", "_Color3","_Color4","_River_color","_River_color_dark"};
    private static string[] init_colors1 = new string[] {"#63AB3F", "#3B7D4F", "#2F5753", "#283540", "#4FA4B8", "#404973"};
    private static string[] color_vars2 = new string[]{"_Base_color", "_Outline_color", "_Shadow_Base_color","_Shadow_Outline_color"};
    private static string[] init_colors2 = new string[] {"#FFFFFF", "#DFE0E8", "#686F99","#404973"};
    private void Awake()
    {
        m_Land = Land.GetComponent<SpriteRenderer>().sharedMaterial;
        m_Cloud = Cloud.GetComponent<SpriteRenderer>().sharedMaterial;
        SetInitialColors();
    }

    public void Update() {
        UpdateTime(Time.time);
    }

    private void OnValidate() {
        m_Land = Land.GetComponent<SpriteRenderer>().sharedMaterial;
        m_Cloud = Cloud.GetComponent<SpriteRenderer>().sharedMaterial;
        ApplyChanges();
    }

    public void ApplyChanges()
    {
        if (m_Land == null) return;
        if (m_Cloud == null) return;

        m_Land.SetFloat(ShaderProperties.Key_Pixels, PixelSize);
        m_Cloud.SetFloat(ShaderProperties.Key_Pixels, PixelSize);
        
        m_Land.SetVector(ShaderProperties.Key_Light_origin, LightPosition);
        m_Cloud.SetVector(ShaderProperties.Key_Light_origin, LightPosition);
        
        var converted_seed = Seed % 1000f / 100f;
        m_Land.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Cloud.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Cloud.SetFloat(ShaderProperties.Key_Cloud_cover, Random.Range(0.35f, 0.6f));
        
        m_Land.SetFloat(ShaderProperties.Key_Rotation, Rotation);
        m_Cloud.SetFloat(ShaderProperties.Key_Rotation, Rotation);
        
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_Land.SetColor(color_vars1[i], Colors[i]);
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_Cloud.SetColor(color_vars2[i], Colors[i + color_vars1.Length]);
        }
    }


    public void UpdateTime(float time)
    {
        m_Cloud.SetFloat(ShaderProperties.Key_time, time * 0.25f);
        m_Land.SetFloat(ShaderProperties.Key_time, time * 0.5f);
    }

    public void SetInitialColors()
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_Land.SetColor(color_vars1[i], ColorUtil.FromRGB(init_colors1[i]));
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_Cloud.SetColor(color_vars2[i], ColorUtil.FromRGB(init_colors2[i]));
        }
    }
    
}
