using System.Collections;
using UnityEngine;

namespace Player.Module
{
    public class CameraShake : ModuleBaseScript
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private float magnitudeMultiplier = 0.25f;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private bool isShaking = false;
        
        public void ShakeCamera(float magnitude, float duration)
        {
            if (!isShaking)
            {
                StartCoroutine(CameraShakeCoroutine(magnitude, duration));
            } 
        }

        private IEnumerator CameraShakeCoroutine(float magnitude, float duration)
        {
            magnitude *= magnitudeMultiplier;
            isShaking = true;
            Vector3 initialPosition = transform.localPosition;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float offsetMult = (duration - elapsedTime) / duration;
                
                float x = Random.Range(-magnitude, magnitude) * offsetMult;
                float y = Random.Range(-magnitude, magnitude) * offsetMult;
                
                transform.localPosition = new Vector3(x, y, initialPosition.z);
                
                elapsedTime += Time.deltaTime;

                yield return null;
            }
            
            transform.localPosition = initialPosition;
            isShaking = false;
        }
    }
}
