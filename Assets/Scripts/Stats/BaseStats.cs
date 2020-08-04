using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
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
        [SerializeField] private bool shouldUseModifiers = false;

        private Experience exp;
        LazyValue<int> currentLevel;

        public event Action OnLevelUp;

        private void Awake()
        {
            exp = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
        }

        // Called after Awake
        private void OnEnable()
        {
            if (exp != null)
            {
                // Delegate
                exp.OnExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (exp != null)
            {
                // Delegate
                exp.OnExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
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
            return GetBaseStat(stat) + GetAdditiveModifier(stat) * (1 + GetPercentageModifier(stat)/100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel.value;
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
            if (!shouldUseModifiers) return 0;
            
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
        
        
        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
    }
}
