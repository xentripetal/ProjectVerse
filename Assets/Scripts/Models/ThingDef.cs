using System;
using UnityEngine;

public class ThingDef {
    public string name { get; private set; }
    public Sprite sprite { get; private set; }
    public IThingScript[] scripts { get; private set; }


    public ThingDef(String name, String resourcePath, IThingScript[] scripts) {
        this.name = name;
        sprite = Resources.Load<Sprite>(resourcePath);
        this.scripts = scripts;
    }
}