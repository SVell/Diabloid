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
        [SerializeField] private GameObject levelUpParticle = null;

        private Experience exp;
        private int currentLevel = 1;

        public event Action OnLevelUp;

        private void Start()
        {
            exp = GetComponent<Experience>();
            currentLevel = CalculateLevel();
            if (exp != null)
            {
                // Delegate
                exp.OnExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticle, transform);
            OnLevelUp();
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel()) + GetAdditiveModifier(stat);
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }
        
        private int CalculateLevel()
        {
            if (exp == null) return startingLevel;

            float currentXp = exp.GetExperience();
            for (int i = 1; i <= progression.GetLevels(Stat.ExperienceToLevelUp, characterClass); i++)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, i);
                if (xpToLevelUp > currentXp)
                {
                    return i;
                }
            }

            return progression.GetLevels(Stat.ExperienceToLevelUp, characterClass) + 1;
        }
        
        private float GetAdditiveModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
    }
}
