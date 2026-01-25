using System;
using System.Collections;
using Player.Module;
using UnityEngine;

namespace HelpScripts
{
    public class ModuleManipulation : MonoBehaviour
    {
        
        public static IEnumerator GradualModuleStop(Vector3 position, Module moduleRef, Func<bool> shouldRun)
        {
            while (shouldRun())
            {
                Vector2 direction = Convertor.Vec3ToVec2(position - moduleRef.transform.position);
                moduleRef.moveRb.linearVelocity = direction.normalized * 0.5f + moduleRef.moveRb.linearVelocity.normalized * 0.5f;
                moduleRef.moveRb.angularVelocity *= 0.8f;
                if (direction.magnitude < 1f)
                {
                    if (direction.magnitude < 0.2)
                    {
                        break;
                    }
                    moduleRef.moveRb.linearVelocity *= Mathf.Max(direction.magnitude, 0.5f);
                }
                yield return new WaitForSeconds(0.05f);
            }
            moduleRef.moveRb.linearVelocity = Vector3.zero;
            moduleRef.moveRb.angularVelocity = 0;
        }
    }
}
