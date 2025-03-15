using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convertor : MonoBehaviour
{
    public static Vector2 Vec3ToVec2(Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }

    public static Vector3 Vec2ToVec3(Vector2 vec2)
    {
        return new Vector3(vec2.x, vec2.y, 0);
    }
}
