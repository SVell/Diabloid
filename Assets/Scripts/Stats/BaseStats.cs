using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;

        private void Update()
        {
            if(gameObject.tag == "Player")
                print(GetLevel());
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            Experience exp = GetComponent<Experience>();

            if (exp == null) return startingLevel;

            float currentXp = exp.GetExperience();

            for (int i = 1; i <= progression.GetLevels(Stat.ExperienceToLevelUp, characterClass); i++)
            {
                float XpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, i);
                if (XpToLevelUp > currentXp)
                {
                    return i;
                }
            }

            return progression.GetLevels(Stat.ExperienceToLevelUp, characterClass) + 1;
        }
    }
}
