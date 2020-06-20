using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        
        [SerializeField] 
        private float weaponDamage = 5;
        [SerializeField] 
        private float weaponRange = 2;
        private Mover mover;
        private Transform target;

        [SerializeField] 
        private float timeBetweenAttacks = 1f;
        private float timeSinceLastAttack;
        
        void Start()
        {
            mover = GetComponent<Mover>();
        }
        
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            MoveToTheTarget();
        }

        private void MoveToTheTarget()
        {
            if(target == null) return;
            
            if (target != null && !IsInRange())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                // Trigger Hit event
                timeSinceLastAttack = 0;
                GetComponent<Animator>().SetTrigger("attack");
            }
        }
        
        // Animation Event
        private void Hit()
        {
            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combateTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combateTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}
