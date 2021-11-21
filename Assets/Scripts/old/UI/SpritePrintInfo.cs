using System.Collections.Generic;
using UnityEngine;

public class SpritePrintInfo : MonoBehaviour {
    public Sprite Sprite;

    public void PrintInfo() {
        var shape = new List<Vector2>();
        Sprite.GetPhysicsShape(0, shape);
        var output = "[";
        for (var i = 0; i < shape.Count; i++) {
            var pos = shape[i];
            output += "{\n";
            output += "\t\"x\": " + pos.x + ",\n";
            output += "\t\"y\": " + pos.y + "\n";
            output += "}";
            if (i != shape.Count - 1) output += ", ";
        }

        output += "]";
        Debug.Log(output);
    }
}