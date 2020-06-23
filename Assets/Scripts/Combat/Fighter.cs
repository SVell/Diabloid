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
        private Health target;

        [SerializeField] 
        private float timeBetweenAttacks = 1f;
        private float timeSinceLastAttack = Mathf.Infinity;
        
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
            if(target.IsDead()) return;
            
            if (target != null && !IsInRange())
            {
                mover.MoveTo(target.transform.position);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                // Trigger Hit event
                timeSinceLastAttack = 0;
                TriggetAttack();
            }
        }

        private void TriggetAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        private void Hit()
        {
            if(target == null) return;
            target.TakeDamage(weaponDamage);
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
        }
    }
}
