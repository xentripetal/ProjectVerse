using System;
using UnityEngine;

public class ThingDef {
    public string name { get; private set; }
    public Sprite sprite { get; private set; }
    public 

    public ThingDef(String name, String resourcePath) {
        this.name = name;
        sprite = Resources.Load<Sprite>(resourcePath);
    }
}