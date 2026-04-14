using System;
using UnityEngine;
using ScriptableObjects.Material;
using System.Collections;
using System.Collections.Generic;

namespace Player.Module.Tools
{
    public class ExplosiveTool : ModuleTool
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private bool explodeOnImpact;

        [SerializeField] private float explosionDelay;
        [SerializeField] MaterialSO.DamageType damageType;

        [SerializeField] private float damage;

        [SerializeField] private float explosionRadius;

        [SerializeField] private Rigidbody2D rb;
        
        [SerializeField] private ParticleSystem ps;
        [SerializeField] private ParticleSystem explosionRadiusPS;
        
        [SerializeField] private Animator animator;

        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private bool exploded = false;

        public override void Use(Module m)
        {
            rb.linearVelocity = Convertor.Vec3ToVec2((transform.position - m.transform.position).normalized) * 2;
        }
        
        
        private void Awake()
        {
            if (animator != null)
            {
                animator.SetFloat("timerSpeed", 1f/explosionDelay);
                animator.Play("");
            }
            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            if (explosionRadiusPS != null)
            {
                var shape = explosionRadiusPS.shape;
                shape.radius = explosionRadius;
                explosionRadiusPS.Play();
            }
            yield return new WaitForSeconds(explosionDelay);
            Detonate();
        }

        private void Detonate()
        {
            if (exploded)
            {
                return;
            }

            List<Collider2D> results = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();

            filter.NoFilter();

            Physics2D.OverlapCircle(Convertor.Vec3ToVec2(transform.position), explosionRadius, filter, results);

            foreach (Collider2D result in results)
            {
                Entities.HealthBar hb = result.transform.GetComponent<Entities.HealthBar>();

                
                if (hb != null)
                {
                    hb.TakeDamage(damage, damageType);
                }
            }

            exploded = true;

            if (ps != null)
            {
                ps.Play();
                var main = ps.main;
                float totalDuration = main.duration + main.startLifetimeMultiplier;
                Destroy(gameObject, totalDuration);
            }
            else
            {
                Destroy(gameObject);
            }
            

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (explodeOnImpact)
            {
                Detonate();
            }
        }
    }
}
