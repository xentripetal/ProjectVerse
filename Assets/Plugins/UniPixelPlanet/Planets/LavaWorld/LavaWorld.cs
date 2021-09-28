using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LavaWorld : MonoBehaviour {
    [SerializeField] private GameObject PlanetUnder;
    [SerializeField] private GameObject Craters;
    [SerializeField] private GameObject LavaRivers;
    private Material m_Planet;
    private Material m_Craters;
    private Material m_Rivers;
    private static string[] color_vars1 = new string[]{"_Color1", "_Color2", "_Color3"};
    private static string[] init_colors1 = new string[] {"#8f4d57", "#52333f", "#3d2936"};
    
    private static string[] color_vars2 = new string[]{"_Color1", "_Color2"};
    private static string[] init_colors2 = new string[] {"#52333f", "#3d2936"};
    
    private static string[] color_vars3 = new string[]{"_Color1", "_Color2", "_Color3"};
    private static string[] init_colors3 = new string[] {"#ff8933", "#e64539", "#ad2f45"};
    
    public float PixelSize = 96;
    public Vector2 LightPosition = Vector2.one;
    public float Seed = 124124;
    public float Rotation;
    public Color[] Colors = { };
        
    private void Awake()
    {
        m_Planet = PlanetUnder.GetComponent<SpriteRenderer>().sharedMaterial;
        m_Craters = Craters.GetComponent<SpriteRenderer>().sharedMaterial;
        m_Rivers = LavaRivers.GetComponent<SpriteRenderer>().sharedMaterial;
        ApplyChanges();
    }

    public void Update() {
        UpdateTime(Time.time);
    }


    public void ApplyChanges() {
        if (Colors.Length == 0) {
            SetInitialColors();
        }
        m_Planet.SetFloat(ShaderProperties.Key_Pixels, PixelSize);
        m_Craters.SetFloat(ShaderProperties.Key_Pixels, PixelSize);
        m_Rivers.SetFloat(ShaderProperties.Key_Pixels, PixelSize);
        
        m_Planet.SetVector(ShaderProperties.Key_Light_origin, LightPosition);
        m_Craters.SetVector(ShaderProperties.Key_Light_origin, LightPosition);
        m_Rivers.SetVector(ShaderProperties.Key_Light_origin, LightPosition);
        
        var converted_seed = Seed % 1000f / 100f;
        m_Planet.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Craters.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Rivers.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        
        m_Planet.SetFloat(ShaderProperties.Key_Rotation, Rotation);
        m_Craters.SetFloat(ShaderProperties.Key_Rotation, Rotation);
        m_Rivers.SetFloat(ShaderProperties.Key_Rotation, Rotation);
        
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_Planet.SetColor(color_vars1[i], Colors[i]);
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_Craters.SetColor(color_vars2[i], Colors[i + color_vars1.Length]);
        }
        for (int i = 0; i < color_vars3.Length; i++)
        {
            m_Rivers.SetColor(color_vars3[i], Colors[i + color_vars1.Length + color_vars2.Length]);
        }
    }

    public void UpdateTime(float time)
    {
        m_Planet.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        m_Craters.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
        m_Rivers.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
    }

    public void SetInitialColors() {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < color_vars1.Length; i++)
        {
            colors.Add(ColorUtil.FromRGB(init_colors1[i]));
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            colors.Add(ColorUtil.FromRGB(init_colors2[i]));
        }
        for (int i = 0; i < color_vars3.Length; i++)
        {
            colors.Add(ColorUtil.FromRGB(init_colors3[i]));
        }

        Colors = colors.ToArray();
    }
}
