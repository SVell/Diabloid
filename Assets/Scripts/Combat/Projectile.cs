using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Resources;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private bool isHoming = false;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float speed = 1;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private float lifeAfterImpact = 2f;
        [SerializeField] private GameObject[] destroyOnHit = null;
        
        
        Health target = null;
        private float damage = 0;
    
        private void Start()
        {
            transform.LookAt(GetAimLockation());
        }
    
        public void Update()
        {
            if(target == null) return;
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLockation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    
        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
            
            Destroy(gameObject,maxLifeTime);
        }
        
        private Vector3 GetAimLockation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return target.transform.position;
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if(target.IsDead()) return;
            target.TakeDamage(damage);
    
            speed = 0;
            
            if (hitEffect != null)
            {
                Instantiate(hitEffect,GetAimLockation(),transform.rotation);
            }
    
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject,lifeAfterImpact);
        }
    }
}
