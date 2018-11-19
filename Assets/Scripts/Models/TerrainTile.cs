using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile {
    public string name {
        get;
        private set;
    }

    public Sprite sprite{
        get;
        private set;
    }

   public TerrainTile(String name, String resourcePath) {
       this.name = name;
       sprite = Resources.Load<Sprite>(resourcePath);
   }
}
