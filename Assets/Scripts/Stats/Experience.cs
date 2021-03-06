﻿using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour , ISaveable
    {
        [SerializeField] private float experiencePoints = 0;

        //public delegate void ExperienceGainedDelegate();
        public event Action OnExperienceGained;
        
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            OnExperienceGained();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }

        public float GetExperience()
        {
            return experiencePoints;
        }
    }
}
