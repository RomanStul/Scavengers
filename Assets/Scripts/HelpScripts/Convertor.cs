using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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

    public static void Lerp2D(Vector3 targetPosition, Transform toLerp, float speed)
    {
        Quaternion rotation = RotationConversion(targetPosition, toLerp);
        toLerp.transform.rotation = Quaternion.Lerp( toLerp.transform.rotation, rotation, Time.deltaTime * speed);
    }

    public static void Rotate2D(Vector3 targetPosition, Transform toRotate, float speed)
    {
        Quaternion rotation = RotationConversion(targetPosition, toRotate);
        toRotate.transform.rotation = Quaternion.Lerp( toRotate.transform.rotation, rotation, Time.deltaTime * speed);
    }

    public static Quaternion RotationConversion(Vector3 targetPosition, Transform toRotate)
    {
        targetPosition = new Vector3(targetPosition.x, 0, targetPosition.y);
        Quaternion rotation = Quaternion.LookRotation(targetPosition, Vector3.up);
        Vector3 angles = rotation.eulerAngles;
        return Quaternion.Euler(angles.x, angles.z, -angles.y);
    }
}
