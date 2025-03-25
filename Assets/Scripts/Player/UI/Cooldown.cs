using System.Collections;
using UnityEngine;

namespace Player.UI
{
    public class Cooldown : Bar
    {
        public void StartCooldown(float time)
        {
            SetMaxValue(time);
            SetValue(0);
            StartCoroutine(Countdown(time));
        }

        private IEnumerator Countdown(float time)
        {
            while (value < maxValue)
            {
                yield return new WaitForSeconds(0.1f);
                value += 0.1f;
                UpdateFill();
            }
        }
    }
}
