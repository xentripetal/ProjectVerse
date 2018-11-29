using System;
using API.Models;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class TestJson {
    public Position pos;
    public string randomval;

    public TestJson(Position val, string stringval) {
        pos = val;
        randomval = stringval;
    }
}