using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.API.Models;

public static class Utils {
    public static float zPositionMultiplier = .01f;
    public static float zPositionOffset = -10f;

    public static Vector3 SwapVectorDimension(Vector2 vec) {
        return new Vector3(vec.x, vec.y, 0);
    }

    public static Vector2 SwapVectorDimension(Vector3 vec) {
        return new Vector2(vec.x, vec.y);
    }

    public static Sprite InfoToSprite(SpriteInfo info) {
        Texture2D image = Resources.Load<Texture2D>(info.SpritePath);
        Rect rect = new Rect(0, 0, image.width, image.height);
        Sprite sprite = Sprite.Create(image, rect, info.PivotPoint, info.PixelsPerUnit);
        if (info.ColliderShape != null) {
            sprite.OverridePhysicsShape(info.ColliderShape
                .Select(posList => posList.Select(pos => (Vector2) pos).ToArray()).ToList());
        }

        return sprite;
    }

    public static IEnumerable<T> DropLast<T>(this IEnumerable<T> source) {
        if (source == null)
            throw new ArgumentNullException("source");

        return InternalDropLast(source);
    }

    private static IEnumerable<T> InternalDropLast<T>(IEnumerable<T> source) {
        T buffer = default(T);
        bool buffered = false;

        foreach (T x in source) {
            if (buffered)
                yield return buffer;

            buffer = x;
            buffered = true;
        }
    }
}