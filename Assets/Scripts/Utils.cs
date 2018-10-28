using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

    public static Vector3 SwapVectorDimension(Vector2 vec)
    {
        return new Vector3(vec.x, vec.y, 0);
    }
    public static Vector2 SwapVectorDimension(Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

}
