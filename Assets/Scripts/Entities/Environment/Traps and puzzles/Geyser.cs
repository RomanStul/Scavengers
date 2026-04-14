using System;
using System.Collections;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    //================================================================CLASSES
    //================================================================EDITOR VARIABLES

    [SerializeField] protected ParticleSystem particle;

    [SerializeField] private float delay, startDelay;
    [SerializeField] private float duration;
    
    //================================================================GETTER SETTER
    //================================================================FUNCTIONALITY
    protected ParticleSystem.MainModule main;

    protected virtual void Awake()
    {
        main = particle.main;
        main.duration = duration;
        main.loop = false;
        StartCoroutine(Burst());
    }

    private IEnumerator Burst()
    {
        yield return new WaitForSeconds(startDelay);
        particle.gameObject.SetActive(true);
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(delay);
            particle.Play();
            yield return new WaitForSeconds(duration);
        }
    }
}
