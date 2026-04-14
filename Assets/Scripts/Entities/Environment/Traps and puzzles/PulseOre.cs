using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Entities.Environment.Traps_and_puzzles
{
    public class PulseOre : MonoBehaviour
    {
        //================================================================CLASSES

        [Serializable]
        public class PulsePadConstants
        {
            public float forceStrength;
            public float timeBeforeDischarging;
            public float dischargeSpeed;
            public float lightIntensityMult;
        }
        //================================================================EDITOR VARIABLES

        [SerializeField] protected PulsePadConstants constants;
        [SerializeField] protected HealthBar healthBar;

        [SerializeField] protected SpriteRenderer visuals;

        [SerializeField] protected Sprite[] images;
        
        [SerializeField] protected Light2D intensityLight;
            
        [SerializeField] protected ParticleSystem particles;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        protected float lastCharge;


        protected void Update()
        {
            if (Time.time - lastCharge > constants.timeBeforeDischarging)
            {
                healthBar.HealHealth(Time.deltaTime * constants.dischargeSpeed);
                ChangeVisuals();
            }
        }
        
        public virtual void MakePulse()
        {
            List<Collider2D> results = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();

            filter.NoFilter();

            Physics2D.OverlapCircle(Convertor.Vec3ToVec2(transform.position), 2.5f, filter, results);

            foreach (var res in results)
            {
                Rigidbody2D rb2d = res.transform.GetComponent<Rigidbody2D>();
                if (res.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    rb2d = res.transform.parent.GetComponent<Rigidbody2D>();
                }
                
                if (rb2d != null)
                {
                    rb2d.AddForce((res.transform.position - transform.position).normalized * constants.forceStrength, ForceMode2D.Impulse);
                }
                
            }
            
            ParticleSystem destroyParticles = Instantiate(particles, transform.position, transform.rotation);
            destroyParticles.Play();
            Destroy(destroyParticles.gameObject, 2f);
        }

        public void BuildCharge()
        {
            lastCharge = Time.time;
            ChangeVisuals();
        }

        protected void ChangeVisuals()
        {
            float hp = healthBar.GetHealth();
            float max = healthBar.GetMaxHealth();
            int index = Mathf.FloorToInt((hp / max) * (images.Length - 1));
            visuals.sprite = images[index];

            intensityLight.intensity = (1-(hp/max)) * constants.lightIntensityMult;
        }
    }
}
