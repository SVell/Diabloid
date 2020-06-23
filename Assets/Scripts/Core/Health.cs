using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [FormerlySerializedAs("health")] [SerializeField]
        private float healthPoints = 100f;

        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if(isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
