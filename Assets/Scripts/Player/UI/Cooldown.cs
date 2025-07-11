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
            StartCoroutine(CountUp(time));
        }

        public void StartDuration(float time)
        {
            SetMaxValue(time);
            SetValue(time);
            StartCoroutine(CountDown());
        }

        private IEnumerator CountUp(float time)
        {
            while (value < maxValue)
            {
                yield return new WaitForSeconds(0.1f);
                value += 0.1f;
                UpdateFill();
            }
        }

        private IEnumerator CountDown()
        {
            while (value > 0)
            {
                yield return new WaitForSeconds(0.1f);
                value -= 0.1f;
                UpdateFill();
            }
        }
    }
}
