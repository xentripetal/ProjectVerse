using System;
using UnityEngine;

public class Thing {

    public string name {
        get;
        private set;
    }

    public Sprite sprite{
        get;
        private set;
    }

   public Thing(String name, String resourcePath, ScriptableObject) {
       this.name = name;
       sprite = Resources.Load<Sprite>(resourcePath);
   }
    
    
}
