using UnityEngine;

namespace Entities.Environment.Traps_and_puzzles
{
    public class PermanentGeyser : Geyser
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private bool isBlocked = true;
        [SerializeField] private Destructible block;

        //================================================================GETTER SETTER

        public void SetParticleSpeed(float speed)
        {
            main = particle.main;
            main.startSpeed = speed;
        }

        public void SetParticleCount(int count)
        {
            emission = particle.emission;
            emission.rateOverTime = count;
        }

        public void SetParticleEmissionRate(float rate)
        {
            emission = particle.emission;
            emission.rateOverTimeMultiplier = rate;
        }
        
        public bool Blocked {
            get => isBlocked && !block.CheckIfShouldBeDestroyed();
            set => isBlocked = value;
        }
        //================================================================FUNCTIONALITY
        private ParticleSystem.EmissionModule emission;
        
        public void SetReferences()
        {
            main = particle.main;
            emission = particle.emission;
        }

        protected override void Awake()
        {
            
        }

        public void StartEruption()
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }
    }
}
