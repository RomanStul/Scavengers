using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convertor : MonoBehaviour
{
    public static Vector2 Vec3ToVec2(Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }
}
