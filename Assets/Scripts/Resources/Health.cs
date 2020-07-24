using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Resources
{
    public class Health : MonoBehaviour , ISaveable
    {
        private float healthPoints = -1f;

        private bool isDead = false;

        private void Start()
        {
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        

        public float GetPercentage()
        {
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }
        
        private void Die()
        {
            if(isDead) return;
            isDead = true;
            //GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null) return;
            
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }
        
        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float) state;

            if (healthPoints <= 0)
            {
                Die();
            }
        }
    }
}
