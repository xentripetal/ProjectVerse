using System;
using UnityEngine;

public class ThingDef {
    public string Name { get; private set; }
    public Sprite Sprite { get; private set; }
    public bool isTrigger { get; private set; }
    public IThingScript[] Scripts { get; private set; }


    public ThingDef(String name, String resourcePath, bool isTrigger, IThingScript[] scripts) {
        this.Name = name;
        Sprite = Resources.Load<Sprite>(resourcePath);
        this.isTrigger = isTrigger;
        this.Scripts = scripts;
    }
}